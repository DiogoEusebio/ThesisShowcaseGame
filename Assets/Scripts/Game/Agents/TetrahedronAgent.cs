using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrahedronAgent : Agent
{
    protected override void Start()
    {
        SetMaxHP(50.0f);
        SetCurrentHPtoMax();
        GenerateBasicAgentGoals();
        GetActionsFromGoals();
        Debug.Log(GetAgentType());
    }

    protected override void Update()
    {
        if (GetIsDead())
        {
            respawnTimer -= Time.deltaTime;
            RespawnAgent(respawnTimer);
        }
        else
        {
            GatherResources();
        }
    }
    protected override void GenerateBasicAgentGoals()
    {
        GameObject objective = GameObject.FindWithTag("Objective");
        //Debug.Log(objective);
        Vector3 ObjPos = objective.transform.position;
        //Debug.Log(ObjPos);
        //GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x - 3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
        GoalList.Add(new CollectResourcesGoal(transform));
    }
    private void GatherResources()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "CollectResourcesGoal");
        ActionToExecute = ActionList.Find((action) => action.GetName() == "CollectResourceAction");
        ActionToExecute.UpdateDirection();
        if(ActionToExecute.GetClosestResorcePosition() != new Vector3(1000f, 1000f, 1000f))
        {
            if (ActionToExecute.Perform() == Action.State.Executed)
            {
                GoalBeingPursued.SetGoalState(Goal.State.Achieved);
                RemoveAchivedGoal(GoalBeingPursued);
                RemoveActionsAssociatedToGoal(GoalBeingPursued);
                //GameObject objective = GameObject.FindWithTag("Objective"); //remove later and add look at resource action
                //Vector3 ObjPos = objective.transform.position;
                GoalList.Add(new CollectResourcesGoal(transform));
                GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "CollectResourcesGoal"));
                Debug.Log("Collected Resorce");
            }
        }
        Debug.Log("Waiting for resources to spawn");
    }
}
