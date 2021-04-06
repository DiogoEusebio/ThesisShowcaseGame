using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour
{
    private List<GameObject> TargetAgentsList;

    protected abstract List<GameObject> ComputeTargetAgentsList(Transform AgentTransform);
}
