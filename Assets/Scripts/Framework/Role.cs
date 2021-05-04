using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role
{
    protected List<GameObject> TargetAgentsList;

    public List<GameObject> GetTargetAgents() { return TargetAgentsList; }
    public abstract List<GameObject> ComputeTargetAgentsList(Transform AgentTransform);
}
