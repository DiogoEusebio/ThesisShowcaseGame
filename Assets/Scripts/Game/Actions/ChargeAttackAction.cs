using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackAction : Action
{   
    private Transform agentTransform;
    private Transform enemyTransform;
    private float ChargeSpeed = 25.0f;
    private float WalkUpSpeed = 10.0f;
    private float ChargeDamage = 50.0f;
    private float CooldownTimer = 1.0f;
    private float CooldownTime = 0.0f;
    public ChargeAttackAction(Transform AgentTransform, Transform EnemyTransform)
    {
        name = "ChargeAttackAction";
        agentTransform = AgentTransform;
        enemyTransform = EnemyTransform;
    }
    //PROBLEM CHARGE HAS NO COOLDOWN, SO LOOKS LIKE A ONE SHOT KILL
    public override State Perform()
    {
        Debug.Log(CooldownTime);
        CooldownTime -= Time.deltaTime;
        if(CooldownTime <= 0.0f && Vector3.Distance(agentTransform.position, enemyTransform.position) < 5.0f)
        {
            Vector3 direction = enemyTransform.position - agentTransform.position;
            direction.Normalize();
            agentTransform.position += direction * ChargeSpeed * Time.deltaTime;
            //simple way to check collision, but results in single target damage even when "colliding" with multiple enemies at once
            if(Vector3.Distance(agentTransform.position, enemyTransform.position) <= 0.49f) 
            {
                enemyTransform.GetComponent<Agent>().TakeDamage(ChargeDamage);
                CooldownTime = CooldownTimer;
                return State.Executed;
            }
            return State.BeingExecuted;
        }
        else if (Vector3.Distance(agentTransform.position, enemyTransform.position) > 3.0f)
        {
            Vector3 direction = enemyTransform.position - agentTransform.position;
            direction.Normalize();
            agentTransform.position += direction * WalkUpSpeed * Time.deltaTime;
            return State.BeingExecuted;
        }
        return State.NotBeingExecuted;
    }
}
