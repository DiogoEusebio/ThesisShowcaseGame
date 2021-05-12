using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCloserEnemyAction : Action
{
    private float enemyDistance;
    private Transform clossestEnemy;
    private Transform agentTransform;
    private List<GameObject> enemies;
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
            enemies = GetListOfEnemies(GameObject.FindGameObjectsWithTag("BlueTeam"), GameObject.FindGameObjectsWithTag("GreenTeam"));
            if (enemies.Count> 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Agent>().GetIsDead() == false)
                    {
                        if (Vector3.Distance(enemy.transform.position, agentTransform.position) < enemyDistance)
                        {
                            enemyDistance = Vector3.Distance(enemy.transform.position, agentTransform.position);
                            clossestEnemy = enemy.transform;
                        }
                    }
                }
            }
            //Debug.Log(GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("BlueTeam"));
            if (enemies.Count == 0 || GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("BlueTeam") + GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("GreenTeam") == enemies.Count)
            {
                clossestEnemy = GameObject.FindWithTag("Objective").transform;
            }
        }
        if (agentTransform.gameObject.tag == "BlueTeam")
        {
            //GameObject[] enemies = GameObject.FindGameObjectsWithTag("RedTeam");
            enemies = GetListOfEnemies(GameObject.FindGameObjectsWithTag("RedTeam"), GameObject.FindGameObjectsWithTag("GreenTeam"));
            if (enemies.Count > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Agent>().GetIsDead() == false)
                    {
                        if (Vector3.Distance(enemy.transform.position, agentTransform.position) < enemyDistance)
                        {
                            enemyDistance = Vector3.Distance(enemy.transform.position, agentTransform.position);
                            clossestEnemy = enemy.transform;
                        }
                    }
                }
            }
            //Debug.Log(GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("RedTeam") + " | " + enemies.Length);
            if (enemies.Count == 0 || GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("RedTeam") + GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("GreenTeam") == enemies.Count)
            {
                clossestEnemy = GameObject.FindWithTag("Objective").transform;
            }
        }

        if (agentTransform.gameObject.tag == "GreenTeam")
        {
            //GameObject[] enemies = GameObject.FindGameObjectsWithTag("RedTeam");
            enemies = GetListOfEnemies(GameObject.FindGameObjectsWithTag("RedTeam"), GameObject.FindGameObjectsWithTag("BlueTeam"));
            if (enemies.Count > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Agent>().GetIsDead() == false)
                    {
                        if (Vector3.Distance(enemy.transform.position, agentTransform.position) < enemyDistance)
                        {
                            enemyDistance = Vector3.Distance(enemy.transform.position, agentTransform.position);
                            clossestEnemy = enemy.transform;
                        }
                    }
                }
            }
            //Debug.Log(GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("RedTeam") + " | " + enemies.Length);
            if (enemies.Count == 0 || GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("RedTeam") + GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetNumberOfDeadEnemies("BlueTeam") == enemies.Count)
            {
                clossestEnemy = GameObject.FindWithTag("Objective").transform;
            }
        }
        agentTransform.LookAt(clossestEnemy);
        return State.BeingExecuted;
        
    }
    List<GameObject> GetListOfEnemies(GameObject[] team1, GameObject[] team2)
    {
        List<GameObject> Enemies = new List<GameObject>();
        foreach(GameObject obj in team1)
        {
            Enemies.Add(obj);
        }
        foreach(GameObject obj in team2)
        {
            Enemies.Add(obj);
        }
        return Enemies;
    }
}
