using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeammateRole : Role
{
    public TeammateRole(Transform SelfAgent)
    {
        //create Goals
        selfAgent = SelfAgent;
        name = "TeammateRole";
        if (SelfAgent.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Cube)
        {
            Transform AllyToDefend = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClosestTeammate(SelfAgent);
            //Debug.Log(AllyToDefend);
            SelfAgent.GetComponent<Agent>().GetGoalList().Add(new ProtectTeammateGoal(SelfAgent, AllyToDefend));
        }
    }
    public override void ComputeTargetAgentsList(Transform AgentTransform)
    {
        //as it is agents are teamates of themselves (consider change)
        List<GameObject> TeammatesList = new List<GameObject>();

        if (AgentTransform.CompareTag("BlueTeam"))
        {
            GameObject[] TeammatesArray = GameObject.FindGameObjectsWithTag("BlueTeam");
            foreach (GameObject go in TeammatesArray)
            {
                if(go.transform != selfAgent)
                {
                    TeammatesList.Add(go);
                }
            }
        }
        if (AgentTransform.CompareTag("RedTeam"))
        {
            GameObject[] TeammatesArray = GameObject.FindGameObjectsWithTag("RedTeam");
            foreach (GameObject go in TeammatesArray)
            {
                if (go.transform != selfAgent)
                {
                    TeammatesList.Add(go);
                }
            }
        }
        if (AgentTransform.CompareTag("GreenTeam"))
        {
            GameObject[] TeammatesArray = GameObject.FindGameObjectsWithTag("GreenTeam");
            foreach (GameObject go in TeammatesArray)
            {
                if (go.transform != selfAgent)
                {
                    TeammatesList.Add(go);
                }
            }
        }
        TargetAgentsList = TeammatesList;
    }

    public override void AddTeammate(Transform NewTeammate)
    {
        foreach(GameObject Teammate in TargetAgentsList)
        {
            if(Teammate.transform == NewTeammate)
            {
                return; //ignore if duplicate
            }
        }
        TargetAgentsList.Add(NewTeammate.gameObject);
    }
    public override void RemoveTeammate(Transform OldTeammate)
    {
        foreach(GameObject Teammate in TargetAgentsList)
        {
            if (Teammate.transform == OldTeammate)
            {
                TargetAgentsList.Remove(OldTeammate.gameObject);
            }
        }
    }
    //--------------------------------DEBUG METHODS------------------------------//
    public override void DebugLogTeammates()
    {
        string teammatesString = "";
        foreach(GameObject Teammate in TargetAgentsList)
        {
            teammatesString += Teammate.transform + " ";
        }
        Debug.Log("Agent: " + selfAgent + "has: " + teammatesString + "as teammates!");
    }
}
