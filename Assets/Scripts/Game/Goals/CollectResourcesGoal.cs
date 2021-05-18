using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResourcesGoal : Goal
{
    public CollectResourcesGoal(Transform Agent)
    {
        name = "CollectResourcesGoal";
        ActionList.Add(new CollectResourceAction(Agent));
    }
}
