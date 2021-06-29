using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAgent : Agent
{
    protected override void Start()
    {
        SetMaxHP(100.0f);
        SetCurrentHPtoMax();
        //GenerateBasicAgentGoals();
        GetActionsFromGoals();
        //DebugLogRoles();
        InitAgentLog();
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
            //ContestObjective();
            //CaptureFlag();
            ProtectAlly();
        }
    }
    protected override void GenerateBasicAgentGoals()
    {
        Transform AllyToDefend = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClosestTeammate(transform);
        Debug.Log(AllyToDefend);
        GoalList.Add(new ProtectTeammateGoal(transform, AllyToDefend));
    }
    private void ProtectAlly()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "ProtectTeammateGoal");
        ActionToExecute = ActionList.Find((action) => action.GetName() == "ShieldAllyAction");
        if(ActionToExecute.Perform() == Action.State.NotBeingExecuted)
        {
            Debug.Log("CUBE NOT EXECUTING");
            //my ally probably died or I dont have allies
            //TODO: CAREFULL DEAL WITH ALLY DINAMICALY STOPING BEING ALLY
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);

            Transform AllyToDefend = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClosestTeammate(transform);
            GoalList.Add(new ProtectTeammateGoal(transform, AllyToDefend));
            GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "ProtectTeammateGoal"));
        }
    }
}
