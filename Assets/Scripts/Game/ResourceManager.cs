using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private List<Vector3> ResourcePoints = new List<Vector3>();
    public GameObject ResourcePrefab;
    // Start is called before the first frame update
    void Start()
    {
        ResourcePoints.Add(new Vector3(12.0f, 1.0f, 0.0f));
        ResourcePoints.Add(new Vector3(-12.0f, 1.0f, 0.0f));
        ResourcePoints.Add(new Vector3(0.0f, 1.0f, 22.0f));
        GenerateNewResource(ResourcePoints[0]);
        GenerateNewResource(ResourcePoints[1]);
        GenerateNewResource(ResourcePoints[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject GenerateNewResource(Vector3 spawnPoint)
    {
        //Debug.Log(spawnPoint);
        Transform parent = GameObject.Find("ResourceBag").transform;
        GameObject newObj = Instantiate(ResourcePrefab, spawnPoint, Quaternion.identity, parent);
        return newObj;
    }
}
