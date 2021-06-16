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
        //GenerateBasicAgentGoals();
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
            CaptureFlag();
            if(Vector3.Distance(GameObject.FindGameObjectWithTag("AgentManager").GetComponent<AgentManager>().GetClossestEnemy(transform).position, transform.position) < 5.0f)
            {
                ChargeAtEnemy();
            }
        }
    }
    void ChargeAtEnemy()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ChargeAttackAction");
        attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown();
        //TODO: consider looking at enemy at the same time
        //check if there are enemies I can charge at first
        if (!attackIsOnCoolDown)
        {
            //Debug.Log("Attacking enemy");
            Action.State performState = ActionToExecute.Perform();
            if (performState == Action.State.Executed)
            {
                //Debug.Log("CHARGE!!!");
                attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown(); // = true;
                GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            }

        }
        else
        {
            ActionToExecute.UpdateCooldown();
            //Debug.Log("enemy team aced: " + GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag) + " | cooldown: " + attackIsOnCoolDown);
            //Debug.Log("Contesting objective");
            CaptureFlag();
        }
    }
}
