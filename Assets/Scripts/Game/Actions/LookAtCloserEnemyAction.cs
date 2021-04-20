using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCloserEnemyAction : Action
{
    private float enemyDistance;
    private Transform clossestEnemy;
    private Transform agentTransform;
    //if no enemies alive, look at objective
    public LookAtCloserEnemyAction(Transform AgentTransform)
    {
        name = "LookAtCloserEnemyAction";
        ActionState = State.BeingExecuted;
        enemyDistance = 1000.0f; //consider change to maxFloat
        agentTransform = AgentTransform;
    }

    public override State Perform()
    {   
        if (agentTransform.gameObject.tag == "RedTeam")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("BlueTeam");
            if (enemies.Length > 0)
            {
                foreach(GameObject enemy in enemies)
                {
                    if(Vector3.Distance(enemy.transform.position,agentTransform.position) < enemyDistance)
                    {
                        enemyDistance = Vector3.Distance(enemy.transform.position, agentTransform.position);
                        if (enemy.GetComponent<Agent>().GetIsDead() == false)
                        {
                            clossestEnemy = enemy.transform;
                        }
                    }
                }
            }
            if (enemies.Length == 0 || GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("RedTeam") == enemies.Length)
            {
                clossestEnemy = GameObject.FindWithTag("Objective").transform;
            }
        }
        if (agentTransform.gameObject.tag == "BlueTeam")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("RedTeam");
            if (enemies.Length > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(enemy.transform.position, agentTransform.position) < enemyDistance)
                    {
                        enemyDistance = Vector3.Distance(enemy.transform.position, agentTransform.position);
                        if (enemy.GetComponent<Agent>().GetIsDead() == false)
                        {
                            clossestEnemy = enemy.transform;
                        }
                    }
                }
            }
            if(enemies.Length == 0 || GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("RedTeam") == enemies.Length)
            {
                clossestEnemy = GameObject.FindWithTag("Objective").transform;
            }
        }
        agentTransform.LookAt(clossestEnemy);
        return State.BeingExecuted;
    }
}
