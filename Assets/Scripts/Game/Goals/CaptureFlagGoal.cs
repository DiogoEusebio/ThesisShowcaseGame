using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagGoal : Goal
{
    public CaptureFlagGoal(Transform Agent)
    {
        name = "CaptureFlagGoal";
        ActionList.Add(new CaptureFlagAction(Agent));

    }
}
