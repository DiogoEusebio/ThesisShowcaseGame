using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProviderRole : Role
{
    public ProviderRole(Transform SelfAgent)
    {

        selfAgent = SelfAgent;
        name = "ProviderRole";
        if (SelfAgent.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Tetrahedron)
        {
            SelfAgent.GetComponent<Agent>().GetGoalList().Add(new CollectResourcesGoal(selfAgent));
        }
    }
    public override void ComputeTargetAgentsList(Transform AgentTransform)
    {//everyone in a team can be provided
        List<GameObject> CompetitorsList = GetListOfProvidables(GameObject.FindGameObjectsWithTag("RedTeam"),
                                                                GameObject.FindGameObjectsWithTag("BlueTeam"),
                                                                GameObject.FindGameObjectsWithTag("GreenTeam"));


    }

    //---------------------- Helpfull methods -------------------------//
    private List<GameObject> GetListOfProvidables(GameObject[] team1, GameObject[] team2, GameObject[] team3)
    {
        //here players means agents belonging to a team, consider finding a more sutiable name
        List<GameObject> Agents = new List<GameObject>();
        foreach (GameObject obj in team1)
        {
            Agents.Add(obj);
        }
        foreach (GameObject obj in team2)
        {
            Agents.Add(obj);
        }
        foreach (GameObject obj in team3)
        {
            Agents.Add(obj);
        }
        return Agents;
    }
}
