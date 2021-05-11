using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public GameObject ResourcePrefab;
    // Start is called before the first frame update
    void Start()
    {
        GenerateNewResource(new Vector3(2.0f, 1.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject GenerateNewResource(Vector3 spawnPoint)
    {
        Transform parent = GameObject.Find("ResourceBag").transform;
        GameObject newObj = Instantiate(ResourcePrefab, spawnPoint, Quaternion.identity, parent);
        return newObj;
    }
}
