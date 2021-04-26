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

            ContestObjective();
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
        GameObject objective = GameObject.FindWithTag("Objective");
        Vector3 ObjPos = objective.transform.position;
        GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x - 3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
    }

    public void shootbullet()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "AttackEnemyGoal");
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ShootBulletAction");
        ActionToExecute.Perform();
    }
}
