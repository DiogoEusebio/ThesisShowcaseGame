using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperRole : Role
{
    protected override List<GameObject> ComputeTargetAgentsList(Transform AgentTransform)
    {
        List<GameObject> CompetitorsList = new List<GameObject>();
        return CompetitorsList;
    }
}
