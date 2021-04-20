using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyGoal : Goal
{
    public AttackEnemyGoal(Transform AgentTransform)
    {
        name = "AttackEnemyGoal";
        if(AgentTransform.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Cone)
        {
            ActionList.Add(new ShootBulletAction(AgentTransform));
        }
        else if(AgentTransform.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Sphere)
        {
            Transform EnemyTransform = null;
            if (AgentTransform.gameObject.CompareTag("BlueTeam"))
            {
                EnemyTransform = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClossestEnemy(AgentTransform, "RedTeam");
            }
            else if (AgentTransform.gameObject.CompareTag("RedTeam"))
            {
                EnemyTransform = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClossestEnemy(AgentTransform, "BlueTeam");
            }
            if(EnemyTransform == null)
            {
                //might have to take care of case of not having enemies
                Debug.Log("Removing this goal");
                AgentTransform.GetComponent<Agent>().RemoveAchivedGoal(this);
            }
            ActionList.Add(new ChargeAttackAction(AgentTransform, EnemyTransform));
        }
    }
}
