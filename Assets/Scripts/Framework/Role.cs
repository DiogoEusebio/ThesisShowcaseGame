using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role
{
    protected Transform selfAgent;
    protected string name;
    protected List<GameObject> TargetAgentsList;
    //protected List<Goal> GoalList;

    public List<GameObject> GetTargetAgents() { return TargetAgentsList; }

    public string GetName() { return name; }
    public abstract void ComputeTargetAgentsList(Transform AgentTransform);
    public virtual void AddTeammate(Transform newTeammate) { /*do nothing*/ }
    public virtual void RemoveTeammate(Transform oldTeammate) { /*do nothing*/ }
    public virtual void DebugLogTeammates() { /*do nothing*/ }
    public virtual void AddCompetitor(Transform newCompetitor) { /*do nothing*/ }
    public virtual void RemoveCompetitor(Transform oldCompetitor) { /*do nothing*/ }
    public virtual void DebugLogCompetitors() { /*do nothing*/ }
}
