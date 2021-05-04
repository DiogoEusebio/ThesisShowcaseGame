using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRole : Role
{
    public override List<GameObject> ComputeTargetAgentsList(Transform AgentTransform)
    {
        List<GameObject> CollidersList = new List<GameObject>();
        return CollidersList;
    }
}
