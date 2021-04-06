using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyGoal : Goal
{
    public ShootEnemyGoal(Transform AgentTransform)
    {
        name = "ShootEnemyGoal";
        ActionList.Add(new ShootBulletAction(AgentTransform));
    }
}
