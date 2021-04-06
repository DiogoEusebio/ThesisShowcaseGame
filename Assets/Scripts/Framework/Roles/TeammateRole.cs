using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeammateRole : Role
{
    protected override List<GameObject> ComputeTargetAgentsList(Transform AgentTransform)
    {
        List<GameObject> TeammatesList = new List<GameObject>();

        if (AgentTransform.CompareTag("BlueTeam"))
        {
            GameObject[] TeammatesArray = GameObject.FindGameObjectsWithTag("BlueTeam");
            foreach (GameObject go in TeammatesArray)
            {
                TeammatesList.Add(go);
            }
        }
        if (AgentTransform.CompareTag("RedTeam"))
        {
            GameObject[] TeammatesArray = GameObject.FindGameObjectsWithTag("RedTeam");
            foreach (GameObject go in TeammatesArray)
            {
                TeammatesList.Add(go);
            }
        }
        
        return TeammatesList;
    }
}
