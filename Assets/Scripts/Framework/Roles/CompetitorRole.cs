using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorRole : Role
{
    public CompetitorRole(Transform SelfAgent)
    {
        selfAgent = SelfAgent;
        name = "CompetitorRole";
        if (SelfAgent.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Sphere)
        {
            Transform clossestEnemyTransform = GameObject.FindWithTag("AgentManager").GetComponent<AgentManager>().GetClossestEnemy(SelfAgent, SelfAgent.gameObject.tag);
            SelfAgent.GetComponent<Agent>().GetGoalList().Add(new AttackEnemyGoal(SelfAgent, clossestEnemyTransform));
            SelfAgent.GetComponent<Agent>().GetGoalList().Add(new CaptureFlagGoal(SelfAgent));
        }
        if (SelfAgent.GetComponent<Agent>().GetAgentType() == Agent.AgentType.Cone)
        {
            SelfAgent.GetComponent<Agent>().GetGoalList().Add(new AttackEnemyGoal(SelfAgent, null)); //enemy transform null for now, might change shoot bullet action later to acount for it
            SelfAgent.GetComponent<Agent>().GetGoalList().Add(new CaptureFlagGoal(SelfAgent));
        }
    }
    public override void ComputeTargetAgentsList(Transform AgentTransform)
    {
        List<GameObject> CompetitorsList = new List<GameObject>();

        if (AgentTransform.CompareTag("BlueTeam"))
        {
            CompetitorsList = GetListOfCompetitors(GameObject.FindGameObjectsWithTag("RedTeam"), GameObject.FindGameObjectsWithTag("GreenTeam"));
        }
        if (AgentTransform.CompareTag("RedTeam"))
        {
            CompetitorsList = GetListOfCompetitors(GameObject.FindGameObjectsWithTag("BlueTeam"), GameObject.FindGameObjectsWithTag("GreenTeam"));
        }
        if (AgentTransform.CompareTag("GreenTeam"))
        {
            CompetitorsList = GetListOfCompetitors(GameObject.FindGameObjectsWithTag("RedTeam"), GameObject.FindGameObjectsWithTag("BlueTeam"));
        }
        TargetAgentsList = CompetitorsList;
    }
    public override void AddCompetitor(Transform NewCompetitor)
    {
        foreach (GameObject Competitor in TargetAgentsList)
        {
            if (Competitor.transform == NewCompetitor)
            {
                return; //ignore if duplicate
            }
        }
        TargetAgentsList.Add(NewCompetitor.gameObject);
    }
    public override void RemoveCompetitor(Transform OldCompetitor)
    {
        foreach (GameObject Competitor in TargetAgentsList)
        {
            if (Competitor.transform == OldCompetitor)
            {
                TargetAgentsList.Remove(OldCompetitor.gameObject);
            }
        }
    }
    //--------- Helpfull methods-------//
    List<GameObject> GetListOfCompetitors(GameObject[] team1, GameObject[] team2)
    {
        List<GameObject> Agents = new List<GameObject>();
        foreach (GameObject obj in team1)
        {
            Agents.Add(obj);
        }
        foreach (GameObject obj in team2)
        {
            Agents.Add(obj);
        }
        return Agents;
    }

    //-------------- Debug Methods ----------------//
    public override void DebugLogCompetitors()
    {
        string competitorsString = "";
        foreach (GameObject Competitor in TargetAgentsList)
        {
            competitorsString += Competitor.transform + " ";
        }
        Debug.Log("Agent: " + selfAgent + " team: " + selfAgent.tag + " has: " + competitorsString + "as Competitors!");
    }
}

