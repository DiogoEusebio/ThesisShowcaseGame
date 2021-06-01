using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private List<Vector3> ResourcePoints = new List<Vector3>();
    private List<Vector2> ResourceCooldowns = new List<Vector2>(); //x is used as a bool to know if this point is on cooldown or not, y is the cooldown timer
    public GameObject ResourcePrefab;
    private float respawnCooldown = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        ResourcePoints.Add(new Vector3(12.0f, 1.0f, 0.0f));
        ResourceCooldowns.Add(new Vector2(0f, 0f));

        ResourcePoints.Add(new Vector3(-12.0f, 1.0f, 0.0f));
        ResourceCooldowns.Add(new Vector2(0f, 0f));
        
        ResourcePoints.Add(new Vector3(0.0f, 1.0f, 22.0f));
        ResourceCooldowns.Add(new Vector2(0f, 0f));
        
        GenerateNewResource(ResourcePoints[0]);
        GenerateNewResource(ResourcePoints[1]);
        GenerateNewResource(ResourcePoints[2]);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCoolDowns();
    }
    GameObject GenerateNewResource(Vector3 spawnPoint)
    {
        //Debug.Log(spawnPoint);
        //very lazy code, consider 2D list
        /*for (int i = 0; i < ResourcePoints.Count; i++)
        {
            Vector2 currVec = ResourceCooldowns[i];
            if (Vector3.Distance(ResourcePoints[i],spawnPoint) < 0.5)
            {
                currVec.x = 0.0f;
                currVec.y = 0.0f;
            }
        }*/
        Transform parent = GameObject.Find("ResourceBag").transform;
        GameObject newObj = Instantiate(ResourcePrefab, spawnPoint, Quaternion.identity, parent);
        return newObj;
    }
    void UpdateCoolDowns()
    {
        for (int i = 0; i < ResourceCooldowns.Count; i++)
        {
            Vector2 currVec = ResourceCooldowns[i];
            //Debug.Log("WTFFF: " + currVec + " | " + ResourceCooldowns[i]);
            if (currVec.x == 1)
            {
                //Debug.Log("CoolDown: "+ currVec.y);
                currVec.y -= Time.deltaTime;
                if(currVec.y <= 0)
                {
                    currVec.x = 0;
                    GenerateNewResource(ResourcePoints[i]);
                }
                ResourceCooldowns[i] = currVec;
            }
        }
    }
    public void UpdateCooldownFlag(Vector3 Rpos)
    {
        //Debug.Log("Updating CoolDowns");
        for(int i = 0; i < ResourcePoints.Count; i++)
        {
            Vector2 currVec = ResourceCooldowns[i];
            //Debug.Log(Vector3.Distance(ResourcePoints[i], Rpos));
            if (Vector3.Distance(ResourcePoints[i], Rpos) < 0.5)
            {
                //shallowcopy problem?
                currVec.x = 1.0f;
                currVec.y = respawnCooldown;
                ResourceCooldowns[i] = currVec;
            }
        }
    }
}
