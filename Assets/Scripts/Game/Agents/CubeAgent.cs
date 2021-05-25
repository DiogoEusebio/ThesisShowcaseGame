using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAgent : Agent
{
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
            //ContestObjective();
            CaptureFlag();
        }
    }
    protected override void GenerateBasicAgentGoals()
    {
        GoalList.Add(new CaptureFlagGoal(transform));
    }
}
