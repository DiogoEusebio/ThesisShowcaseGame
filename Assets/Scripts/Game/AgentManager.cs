using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public GameObject CubePrefab;
    public GameObject SpherePrefab;
    public GameObject ConePrefab;
    public GameObject TetraHedronPrefab;
    public GameObject Redteam;
    public GameObject Blueteam;
    public GameObject Greenteam;
    public uint RedTeamSize;
    public uint BlueTeamSize;
    public uint GreenTeamSize;
    public Material RedMat;
    public Material BlueMat;
    public Material GreenMat;
    private RoleManager roleManager;

    private Vector3 RedTeamSpawnPoint = new Vector3(12.87f, 1.0f, 14.77f);
    private Vector3 BlueTeamSpawnPoint = new Vector3(-12.87f, 1.0f, 14.77f);
    private Vector3 GreenTeamSpawnPoint = new Vector3(0.0f, 1.0f, -7.0f);
    // Start is called before the first frame update
    void Start()
    {
        //GenerateRandomComps();
        GenerateCompsOfTypes(3, 4, 3);

        List<GameObject> agentList = new List<GameObject>();

        //roleManager = new RoleManager(agentList);
        foreach(Agent ag in Blueteam.GetComponentsInChildren<Agent>())
        {
            agentList.Add(ag.gameObject);
        }
        foreach(Agent ag in Redteam.GetComponentsInChildren<Agent>())
        {
            agentList.Add(ag.gameObject);
        }

        roleManager = new RoleManager(agentList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsEnemyTeamAced(string AgentTag)
    {
        if (AgentTag == "BlueTeam")
        {
            if (GetNumberOfDeadEnemies("RedTeam") == Redteam.transform.childCount && GetNumberOfDeadEnemies("GreenTeam") == Greenteam.transform.childCount)
                return true;
        }
        else if (AgentTag == "RedTeam")
        {
            if (GetNumberOfDeadEnemies("BlueTeam") == Blueteam.transform.childCount && GetNumberOfDeadEnemies("GreenTeam") == Greenteam.transform.childCount)
                return true;
        }
        else if (AgentTag == "GreenTeam")
        {
            if (GetNumberOfDeadEnemies("RedTeam") == Blueteam.transform.childCount && GetNumberOfDeadEnemies("BlueTeam") == Blueteam.transform.childCount)
                return true;
        }
        return false;
    }

    public int GetNumberOfDeadEnemies(string EnemyTeamTag)
    {
        int numberOfDeadEnemies = 0;
        if(EnemyTeamTag == "BlueTeam")
        {
            foreach(Transform enemy in Blueteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead())
                {
                    numberOfDeadEnemies++;
                }
            }
        }
        if(EnemyTeamTag == "RedTeam")
        {
            foreach (Transform enemy in Redteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead())
                {
                    numberOfDeadEnemies++;
                }
            }
        }
        if (EnemyTeamTag == "GreenTeam")
        {
            foreach (Transform enemy in Greenteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead())
                {
                    numberOfDeadEnemies++;
                }
            }
        }
        return numberOfDeadEnemies;
    }
    public Transform GetClossestEnemy(Transform agentTransform, string MyTeam)
    {
        float distance = 1000.0f;
        Transform ClossestEnemy = null;
        if(MyTeam == "BlueTeam")
        {
            foreach (Transform enemy in Redteam.GetComponentInChildren<Transform>())
            {
                if(enemy.GetComponent<Agent>().GetIsDead() == false)
                {
                    float newDistance = Vector3.Distance(agentTransform.position, enemy.position);
                    if (newDistance < distance)
                    {
                        ClossestEnemy = enemy;
                        distance = newDistance;
                    }
                }
            }
            foreach (Transform enemy in Greenteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead() == false)
                {
                    float newDistance = Vector3.Distance(agentTransform.position, enemy.position);
                    if (newDistance < distance)
                    {
                        ClossestEnemy = enemy;
                        distance = newDistance;
                    }
                }
            }
        }
        else if (MyTeam == "RedTeam")
        {
            foreach (Transform enemy in Blueteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead() == false)
                {
                    float newDistance = Vector3.Distance(agentTransform.position, enemy.position);
                    if (newDistance < distance)
                    {
                        ClossestEnemy = enemy;
                        distance = newDistance;
                    }
                }
            }
            foreach (Transform enemy in Greenteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead() == false)
                {
                    float newDistance = Vector3.Distance(agentTransform.position, enemy.position);
                    if (newDistance < distance)
                    {
                        ClossestEnemy = enemy;
                        distance = newDistance;
                    }
                }
            }
        }
        else if (MyTeam == "GreenTeam")
        {
            foreach (Transform enemy in Blueteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead() == false)
                {
                    float newDistance = Vector3.Distance(agentTransform.position, enemy.position);
                    if (newDistance < distance)
                    {
                        ClossestEnemy = enemy;
                        distance = newDistance;
                    }
                }
            }
            foreach (Transform enemy in Redteam.GetComponentInChildren<Transform>())
            {
                if (enemy.GetComponent<Agent>().GetIsDead() == false)
                {
                    float newDistance = Vector3.Distance(agentTransform.position, enemy.position);
                    if (newDistance < distance)
                    {
                        ClossestEnemy = enemy;
                        distance = newDistance;
                    }
                }
            }
        }
        return ClossestEnemy;
    }

    //-------------------- AGENT GENERATION -------------------------//

    void GenerateCompsOfTypes(int blueType, int redType, int greenType)
    {
        GameObject newObj;
        for (uint i = 0; i < RedTeamSize; i++)
        {
            newObj = GenerateAgentOfType(RedTeamSpawnPoint + new Vector3(Random.Range(-2.5f, 2.5f), 0.0f, Random.Range(-2.5f, 2.5f)), Redteam.transform, redType);
            newObj.GetComponent<Renderer>().material = RedMat;
            newObj.tag = "RedTeam";
        }
        for (uint i = 0; i < BlueTeamSize; i++)
        {
            newObj = GenerateAgentOfType(BlueTeamSpawnPoint + new Vector3(Random.Range(-2.5f, 2.5f), 0.0f, Random.Range(-2.5f, 2.5f)), Blueteam.transform, blueType);
            newObj.GetComponent<Renderer>().material = BlueMat;
            newObj.tag = "BlueTeam";
        }
        for (uint i = 0; i < GreenTeamSize; i++)
        {
            newObj = GenerateAgentOfType(GreenTeamSpawnPoint + new Vector3(Random.Range(-2.5f, 2.5f), 0.0f, Random.Range(-2.5f, 2.5f)), Greenteam.transform, greenType);
            newObj.GetComponent<Renderer>().material = GreenMat;
            newObj.tag = "GreenTeam";
        }
    }
    void GenerateRandomComps()
    {
        GameObject newObj;
        for (uint i = 0; i < RedTeamSize; i++)
        {
            newObj = GenerateAgentOfRandomType(RedTeamSpawnPoint, Redteam.transform);
            newObj.GetComponent<Renderer>().material = RedMat;
            newObj.tag = "RedTeam";
        }
        for (uint i = 0; i < BlueTeamSize; i++)
        {
            newObj = GenerateAgentOfRandomType(BlueTeamSpawnPoint, Blueteam.transform);
            newObj.GetComponent<Renderer>().material = BlueMat;
            newObj.tag = "BlueTeam";
        }
        for (uint i = 0; i < GreenTeamSize; i++)
        {
            newObj = GenerateAgentOfRandomType(GreenTeamSpawnPoint, Greenteam.transform);
            newObj.GetComponent<Renderer>().material = GreenMat;
            newObj.tag = "GreenTeam";
        }
    }
    GameObject GenerateAgentOfRandomType(Vector3 spawnPoint, Transform parent)
    {
        GameObject newObj;
        int agentType = Mathf.FloorToInt(Random.Range(0.0f, 3.0f));
        //Debug.Log(agentType);
        if (agentType == 1)
        {
            newObj = Instantiate(CubePrefab, spawnPoint, Quaternion.identity, parent);
            newObj.GetComponent<Agent>().SetAgentType(Agent.AgentType.Cube);
        }
        else if (agentType == 2)
        {
            newObj = Instantiate(SpherePrefab, spawnPoint, Quaternion.identity, parent);
            newObj.GetComponent<Agent>().SetAgentType(Agent.AgentType.Sphere);
        }
        else
        {
            newObj = Instantiate(ConePrefab, spawnPoint, Quaternion.identity, parent);
            newObj.GetComponent<Agent>().SetAgentType(Agent.AgentType.Cone);
        }
        newObj.SetActive(true);
        return newObj;
    }

    GameObject GenerateAgentOfType(Vector3 spawnPoint, Transform parent, int agentType)
    {
        GameObject newObj;
        //Debug.Log(agentType);
        if (agentType == 1)
        {
            newObj = Instantiate(CubePrefab, spawnPoint, Quaternion.identity, parent);
            newObj.GetComponent<Agent>().SetAgentType(Agent.AgentType.Cube);
        }
        else if (agentType == 2)
        {
            newObj = Instantiate(SpherePrefab, spawnPoint, Quaternion.identity, parent);
            newObj.GetComponent<Agent>().SetAgentType(Agent.AgentType.Sphere);
        }
        else if (agentType == 3)
        {
            newObj = Instantiate(ConePrefab, spawnPoint, Quaternion.identity, parent);
            newObj.GetComponent<Agent>().SetAgentType(Agent.AgentType.Cone);
        }
        else
        {
            newObj = Instantiate(TetraHedronPrefab, spawnPoint, Quaternion.identity, parent);
            newObj.GetComponent<Agent>().SetAgentType(Agent.AgentType.Tetrahedron);
        }
        newObj.SetActive(true);
        return newObj;
    }
}
