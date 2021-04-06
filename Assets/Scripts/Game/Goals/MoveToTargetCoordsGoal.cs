using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetCoordsGoal : Goal
{
    public MoveToTargetCoordsGoal(Transform AgentTransform,  Vector3 v)
    {
        name = "MoveToPositionAction";
        ActionList.Add(new MoveToPositionAction(AgentTransform, v));
    }
}
