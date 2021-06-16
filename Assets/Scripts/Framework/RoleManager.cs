using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager
{
    private List<GameObject> agentList;
    public RoleManager(List<GameObject> AgentList)
    {
        agentList = AgentList;
        SetRoles();
    }
    
    void SetRoles()
    {
        SetTeammateRoles();
        SetCompetitorRoles();
        SetProviderRoles();
        //Debug
        //Debug.Log("Agents and their relations with the first agent that was created");
        foreach (Role r in agentList[0].GetComponent<Agent>().GetRoleList())
        {
            foreach (GameObject go in r.GetTargetAgents())
            {
                //Debug.Log(go + " | " + go.transform.tag + " | " + r);
            }
        }
    }
    void SetTeammateRoles()
    {
        foreach(GameObject ag in agentList)
        {
            Role newTeammateRole = new TeammateRole(ag.transform);
            ag.GetComponent<Agent>().RoleList.Add(newTeammateRole); //consider adding getter
            newTeammateRole.ComputeTargetAgentsList(ag.transform);
        }

    }

    void SetCompetitorRoles()
    {
        foreach (GameObject ag in agentList)
        {
            Role newCompetitorRole = new CompetitorRole(ag.transform);
            ag.GetComponent<Agent>().RoleList.Add(newCompetitorRole); //consider adding getter
            newCompetitorRole.ComputeTargetAgentsList(ag.transform);
        }
    }
    void SetProviderRoles()
    {
        GameObject[] independentAgents = GameObject.FindGameObjectsWithTag("IndependentAgent");
        foreach (GameObject ag in independentAgents)
        {
            Role newProviderRole = new ProviderRole(ag.transform);
            ag.GetComponent<Agent>().GetRoleList().Add(newProviderRole);
            newProviderRole.ComputeTargetAgentsList(ag.transform);
        }
    }
}
