using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAgent : Agent
{
    private float timeLeftToShootAgain = 1.0f;
    private float fireRate = 1.0f;

    protected override void Update()
    {
        WalkRandomly();
        //ContestObjective();
        if (timeLeftToShootAgain <= 0.0f)
        {
            shootbullet();
            timeLeftToShootAgain = fireRate;
        }
        timeLeftToShootAgain -= Time.deltaTime;
        
    }
    public void shootbullet()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "ShootEnemyGoal");
        ActionToExecute = GoalBeingPursued.GetActionsFromGoal().Find((action) => action.GetName() == "ShootBulletAction");
        ActionToExecute.Perform();
    }
}
