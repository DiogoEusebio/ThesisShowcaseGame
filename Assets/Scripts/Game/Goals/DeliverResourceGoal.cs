using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverResourceGoal : Goal
{
    public DeliverResourceGoal(Transform Agent, Transform Resource, Transform TargetAgent)
    {
        name = "DeliverResourceGoal";
        ActionList.Add(new DeliverResourceAction(Agent, Resource, TargetAgent));
    }
}
