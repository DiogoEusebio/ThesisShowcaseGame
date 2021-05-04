using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperRole : Role
{
    public override List<GameObject> ComputeTargetAgentsList(Transform AgentTransform)
    {
        List<GameObject> StoppersList = new List<GameObject>();
        return StoppersList;
    }
}
