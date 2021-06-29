using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyGoal : Goal
{
    public AttackEnemyGoal(Transform AgentTransform, Transform EnemyTransform)
    {
        name = "AttackEnemyGoal";
        if(AgentTransform.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Cone)
        {
            ActionList.Add(new ShootBulletAction(AgentTransform));
            ActionList.Add(new LookAtCloserEnemyAction(AgentTransform));
        }
        else if(AgentTransform.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Sphere)
        {
            //ActionList.Add(new ChargeAttackAction(AgentTransform, EnemyTransform));
            ActionList.Add(new LayExplosiveMineAction(AgentTransform));
            //consider adding lookAtEnemyAction (just havent done it because I dont want to deal with filtering duplicate actions(just yet))
        }
    }
}
