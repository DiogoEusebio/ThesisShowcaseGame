using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAgent : Agent
{
    private float timeLeftToShootAgain = 1.0f;
    private float fireRate = 1.0f;

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
            //DebugLogGoals();
            //DebugLogActions();

            CaptureFlag();
            PerformSimulActions();
            //ContestObjective();
            if (timeLeftToShootAgain <= 0.0f)
            {
                shootbullet();
                timeLeftToShootAgain = fireRate;
            }
            timeLeftToShootAgain -= Time.deltaTime;
        }
    }
    protected override void GenerateBasicAgentGoals()
    {
        GoalList.Add(new AttackEnemyGoal(transform, null)); //enemy transform null for now, might change shoot bullet action later to acount for it
        GoalList.Add(new CaptureFlagGoal(transform));
    }

    public void shootbullet()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ShootBulletAction");
        ActionToExecute.Perform();
    }
}
