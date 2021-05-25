using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public GameObject FlagPrefab;
    public Material TeamColorMat;
    public Vector3 TeamSpawnPoint;
    private Vector3 FirstFlagPos;
    private Vector3 AuxDir; //direction relative to the center of the arena (from);
    private List<GameObject> FlagsList = new List<GameObject>();
    void Start()
    {
        //Compute Position of First Flag
        AuxDir = Vector3.Normalize(TeamSpawnPoint - new Vector3(0.0f, 1.0f, 7.0f)); //Vector3(0.0f, 1.0f, 7.0f) is the center of the arena on this scene
        FirstFlagPos = TeamSpawnPoint + AuxDir*2 + new Vector3 (0.0f, -0.5f, 0.0f); //HACK: Vector3 (0.0f, -0.5f, 0.0f) is an adition transition to deal with flag prefab actual position;
        SpawnFlag();
        SpawnFlag();
        SpawnFlag();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnFlag()
    {
        GameObject newObj = Instantiate(FlagPrefab, Vector3.zero, Quaternion.identity, this.transform);
        newObj.GetComponent<Renderer>().material = TeamColorMat;
        FlagsList.Add(newObj);
        Vector3 FlagSpawnPoint = new Vector3(0,0,0);
        if(FlagsList.Count == 0)
        {
            FlagSpawnPoint = FirstFlagPos;
        }
        else if(FlagsList.Count % 2 == 1)
        {
            FlagSpawnPoint = FirstFlagPos + (Vector3.Cross(AuxDir, Vector3.up) / 2 * (FlagsList.Count / 2 - 1));
        }
        else if(FlagsList.Count % 2 == 0)
        {
            FlagSpawnPoint = FirstFlagPos - (Vector3.Cross(AuxDir, Vector3.up) / 2 * (FlagsList.Count / 2 + 1));
        }
        newObj.transform.position = FlagSpawnPoint;
    }
    public void SpawnFlag(Transform Flag)
    {

        FlagsList.Add(Flag.gameObject);
        Vector3 FlagSpawnPoint = new Vector3(0, 0, 0);
        if (FlagsList.Count == 0)
        {
            FlagSpawnPoint = FirstFlagPos;
        }
        else if (FlagsList.Count % 2 == 1)
        {
            FlagSpawnPoint = FirstFlagPos + Vector3.Cross(AuxDir, Vector3.up) / 2 * (FlagsList.Count / 2 - 1);
        }
        else if (FlagsList.Count % 2 == 0)
        {
            FlagSpawnPoint = FirstFlagPos - Vector3.Cross(AuxDir, Vector3.up) / 2 * (FlagsList.Count / 2 + 1);
        }
        Flag.parent.GetComponent<TeamManager>().LoseFlag(Flag);
        Flag.SetParent(this.transform);
        Flag.GetComponentInChildren<FlagObjective>().SetCurrentHoldingTeamBasedOnParentName();
        Flag.gameObject.GetComponent<Renderer>().material = TeamColorMat;
        Flag.position = FlagSpawnPoint;
    }
    void LoseFlag(Transform Flag)
    {
        FlagsList.Remove(Flag.gameObject);
    }
}
