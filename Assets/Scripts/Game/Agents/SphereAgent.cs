using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAgent : Agent
{   
    private float randomBehaviorFlag;
    protected override void Start()
    {
        randomBehaviorFlag = Random.Range(0.0f, 1.0f);
        SetMaxHP(100.0f);
        SetCurrentHPtoMax();
        GenerateBasicAgentGoals();
        GetActionsFromGoals();
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
            //50%50% contest/attack enemy
            if(GoalBeingPursued != null && GoalBeingPursued.GetGoalState() == Goal.State.Achieved)
            {
                Debug.Log("new random");
                randomBehaviorFlag = Random.Range(0.0f, 1.0f);
            }
            //Debug.Log(GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag));
            if (!GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag))
            {
                if (randomBehaviorFlag < 0.5f)
                {
                    //Debug.Log("Charging at enemy");
                    ChargeAtEnemy();
                }
            }
            else
            {
                //Debug.Log("Contesting objective");
                ContestObjective();
            }
        }
    }

    protected override void GenerateBasicAgentGoals()
    {
        GameObject objective = GameObject.FindWithTag("Objective");
        Vector3 ObjPos = objective.transform.position;
        GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x - 3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
        GoalList.Add(new AttackEnemyGoal(transform));
    }

    void ChargeAtEnemy()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
        PerformSimulActions(); //This might cause problems later because the simul action is not granted by this goal
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ChargeAttackAction");
        
        if(ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            if (!GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag))
            {
                GoalList.Add(new AttackEnemyGoal(transform));
            }
            //GetActionsFromGoals(); //this might be duplicating actions: take a look later
        }
    }
}
