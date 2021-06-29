using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackAction : Action
{   
    private Transform agentTransform;
    private Transform enemyTransform;
    private float ChargeSpeed = 50.0f;
    //private float WalkUpSpeed = 10.0f;
    private float ChargeDamage = 50.0f;
    private float CooldownTimer = 0.0f;
    private float CooldownTime = 10.0f;
    private float Speed;
    public ChargeAttackAction(Transform AgentTransform, Transform EnemyTransform)
    {
        name = "ChargeAttackAction";
        agentTransform = AgentTransform;
        enemyTransform = EnemyTransform;
    }
    //PROBLEM CHARGE HAS NO COOLDOWN, SO LOOKS LIKE A ONE SHOT KILL
    public override State Perform()
    {
        
        CooldownTimer = CooldownTimer - Time.deltaTime;
        Vector3 direction = enemyTransform.position - agentTransform.position;
        direction.Normalize();
        if(CooldownTimer > 0.0f)
        {
            Debug.Log(CooldownTimer); //suposedly this code nerver runs becuase perform is not called if action is on cooldown
            return State.NotBeingExecuted;
        }
        //if (CooldownTimer <= 0.0f && Vector3.Distance(agentTransform.position, enemyTransform.position) <= 5.0f)
        if(Vector3.Distance(agentTransform.position, enemyTransform.position) <= 5.0f)
        {
            Speed = ChargeSpeed;
            //simple way to check collision, but results in single target damage even when "colliding" with multiple enemies at once
            if (Vector3.Distance(agentTransform.position, enemyTransform.position) <= 0.49f)
            {
                Agent enemyAgent = enemyTransform.GetComponent<Agent>();
                agentTransform.GetComponent<Agent>().LogAgentActionResult("Charged at enemy: " + enemyAgent.GetAgentType() + enemyAgent.GetAgentID());
                enemyAgent.TakeDamage(ChargeDamage);
                CooldownTimer = CooldownTime;
                return State.Executed;
            }
        }
        //else if (Vector3.Distance(agentTransform.position, enemyTransform.position) > 5.0f)
        else
        {
            //Debug.Log("Enemy Far Away | My pos:" + agentTransform.position);
            Speed = movementSpeed;
        }
        agentTransform.position += direction * Speed * Time.deltaTime;
        return State.BeingExecuted;
    }
    public void SetClossestEnemy(Transform EnemyTransform)
    {
        enemyTransform = EnemyTransform;
    }
    public override bool GetIsOnCoolDown()
    {
        if(CooldownTimer <= 0.0f)
        {
            return false;
        }
        return true;
    }
    public override void UpdateCooldown()
    {
        CooldownTimer = CooldownTimer - Time.deltaTime;
    }
}
