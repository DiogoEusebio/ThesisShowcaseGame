using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAgent : Agent
{   
    private bool attackIsOnCoolDown = false;
    protected override void Start()
    {
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
            ChargeAtEnemyAndContest();
        }
    }

    protected override void GenerateBasicAgentGoals()
    {
        GameObject objective = GameObject.FindWithTag("Objective");
        Vector3 ObjPos = objective.transform.position;
        GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x - 3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
        Transform clossestEnemyTransform = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClossestEnemy(transform, transform.gameObject.tag);
        GoalList.Add(new AttackEnemyGoal(transform, clossestEnemyTransform));
    }

    void ChargeAtEnemyAndContest()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ChargeAttackAction");
        attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown();
        //TODO: consider looking at enemy at the same time
        //check if there are enemies I can charge at first
        if (!GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag) && !attackIsOnCoolDown)
        {
            //Debug.Log("Attacking enemy");
            Action.State performState = ActionToExecute.Perform();
            if (performState == Action.State.Executed)
            {
                attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown(); // = true;
                GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            }
            
        }
        else
        {
            ActionToExecute.UpdateCooldown();
            //Debug.Log("enemy team aced: " + GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag) + " | cooldown: " + attackIsOnCoolDown);
            //Debug.Log("Contesting objective");
            ContestObjective();
        }
    }
}
