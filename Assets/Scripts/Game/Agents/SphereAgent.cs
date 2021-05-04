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
            /*
            DebugLogGoals();
            DebugLogActions();
            
            //HACK
            GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
            ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ChargeAttackAction");
            attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown();
            //Debug.Log(GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag));
            if (!GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag) && !attackIsOnCoolDown)
            {
                    Debug.Log("Charging at enemy");
                    ChargeAtEnemy();
            }
            else
            {
                Debug.Log("Contesting objective");
                ContestObjective();
            }*/
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

    void ChargeAtEnemy()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ChargeAttackAction");
        attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown();

        //check if there are enemies I can charge at first
        //redundant if?
        if (GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag) || attackIsOnCoolDown)
        {   
            //cant execute this action
            return;
        }
        PerformSimulActions(); //This might cause problems later because the simul action is not granted by this goal

        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            attackIsOnCoolDown = true;
            /*GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            Transform clossestEnemyTransform = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClossestEnemy(transform, transform.gameObject.tag);
            GoalList.Add(new AttackEnemyGoal(transform, clossestEnemyTransform));
            GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal"));*/
        }
    }
    void ChargeAtEnemyAndContest()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ChargeAttackAction");
        attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown();

        //check if there are enemies I can charge at first
        //redundant if?
        if (!GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag) && !attackIsOnCoolDown)
        {
            Debug.Log("Attacking enemy");
            Action.State performState = ActionToExecute.Perform();
            if (performState == Action.State.Executed)
            {
                attackIsOnCoolDown = ActionToExecute.GetIsOnCoolDown(); // = true;
                GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            }
            Debug.Log(performState);
            
        }
        else
        {
            Debug.Log("enemy team aced: " + GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().IsEnemyTeamAced(transform.gameObject.tag) + " | cooldown: " + attackIsOnCoolDown);
            Debug.Log("Contesting objective");
        }
        ContestObjective();
    }
}
