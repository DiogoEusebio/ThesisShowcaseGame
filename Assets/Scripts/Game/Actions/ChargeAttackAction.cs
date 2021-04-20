using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackAction : Action
{   
    private Transform agentTransform;
    private Transform enemyTransform;
    private float ChargeSpeed = 25.0f;
    private float walkUpSpeed = 10.0f;
    private float ChargeDamage = 50.0f;
    public ChargeAttackAction(Transform AgentTransform, Transform EnemyTransform)
    {
        name = "ChargeAttackAction";
        agentTransform = AgentTransform;
        enemyTransform = EnemyTransform;
    }
    public override State Perform()
    {   
        if(Vector3.Distance(agentTransform.position, enemyTransform.position) < 5.0f)
        {
            Vector3 direction = enemyTransform.position - agentTransform.position;
            direction.Normalize();
            agentTransform.position += direction * ChargeSpeed * Time.deltaTime;
            //simple way to check collision, but results in single target damage even when "colliding" with multiple enemies at once
            if(Vector3.Distance(agentTransform.position, enemyTransform.position) <= 0.49)
            {
                enemyTransform.GetComponent<Agent>().TakeDamage(ChargeDamage);
                return State.Executed;
            }
            return State.BeingExecuted;
        }
        else if (Vector3.Distance(agentTransform.position, enemyTransform.position) > 3.0f)
        {
            Vector3 direction = enemyTransform.position - agentTransform.position;
            direction.Normalize();
            agentTransform.position += direction * walkUpSpeed * Time.deltaTime;
        }
        return State.NotBeingExecuted;
    }
}
