using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContestObjectiveGoal : Goal
{
    public ContestObjectiveGoal(Transform AgentTransform, Vector3 v)
    {
        name = "ContestObjectiveGoal";
        ActionList.Add(new MoveToPositionAction(AgentTransform, v));
        ActionList.Add(new LookAtCloserEnemyAction(AgentTransform));
    }
}
