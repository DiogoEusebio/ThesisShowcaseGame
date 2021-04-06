using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorRole : Role
{
    protected override List<GameObject> ComputeTargetAgentsList(Transform AgentTransform)
    {
        List<GameObject> CompetitorsList = new List<GameObject>();

        if (AgentTransform.CompareTag("BlueTeam"))
        {
            GameObject[] CompetitorsArray = GameObject.FindGameObjectsWithTag("RedTeam");
            foreach (GameObject go in CompetitorsArray)
            {
                if (go.GetComponent<Agent>().GetAgentType() == AgentTransform.GetComponent<Agent>().GetAgentType())
                {
                    CompetitorsList.Add(go);
                }
            }
        }
        if (AgentTransform.CompareTag("RedTeam"))
        {
            GameObject[] CompetitorsArray = GameObject.FindGameObjectsWithTag("BlueTeam");
            foreach (GameObject go in CompetitorsArray)
            {   
                if(go.GetComponent<Agent>().GetAgentType() == AgentTransform.GetComponent<Agent>().GetAgentType())
                {
                    CompetitorsList.Add(go);
                }
            }
        }

        return CompetitorsList;
    }
}

