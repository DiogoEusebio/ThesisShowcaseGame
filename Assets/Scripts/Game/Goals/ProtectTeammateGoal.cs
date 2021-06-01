using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectTeammateGoal : Goal
{
    // Start is called before the first frame update
    public ProtectTeammateGoal(Transform Agent, Transform TargetAgent)
    {
        name = "ProtectTeammateGoal";
        ActionList.Add(new ShieldAllyAction(Agent, TargetAgent));
    }

}
