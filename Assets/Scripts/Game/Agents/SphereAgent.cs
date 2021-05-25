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
            CaptureFlag();
        }
    }

    protected override void GenerateBasicAgentGoals()
    {
        Transform clossestEnemyTransform = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClossestEnemy(transform, transform.gameObject.tag);
        GoalList.Add(new AttackEnemyGoal(transform, clossestEnemyTransform));
        GoalList.Add(new CaptureFlagGoal(transform));
    }
}
