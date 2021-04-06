using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPositionAction : Action
{
    private Transform agentTransform;
    private Vector3 targetPosition;
    private Vector3 direction;
    public MoveToPositionAction(Transform AgentTransform, Vector3 v)
    {
        name = "MoveToPositionAction";
        ActionState = State.BeingExecuted; 
        targetPosition = v;
        agentTransform = AgentTransform;
        direction = targetPosition - agentTransform.position;
        direction.Normalize();
        //Debug.Log("CurrentPosition:" + agentTransform.position +"TargetPosition" + targetPosition);
    }

    public override State Perform()
    {
        agentTransform.LookAt(targetPosition);
        agentTransform.position += direction * 10.0f * Time.deltaTime;
        //Stop condition
        if (Vector3.Distance(agentTransform.position, targetPosition) < 0.1f)
        {
            ActionState = State.Executed;
        }
        return ActionState;
    }
    public override string ToString()
    {
        return "Move to Position Action";
    }
}
