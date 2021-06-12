using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAllyAction : Action
{
    private Transform AgentTransform;
    private Transform AllyTransform;
    private Vector3 allyDirection;
    private Vector3 directionOffset;
    private Vector3 direction;
    private Vector3 TargetPosition;

    public ShieldAllyAction(Transform Agent, Transform TargetAgent)
    {
        name = "ShieldAllyAction";
        AgentTransform = Agent;
        AllyTransform = TargetAgent;


    }
    public override State Perform()
    {
        if (AllyTransform == null || AllyTransform.GetComponent<Agent>().GetIsDead())
        {
            Debug.Log("My ally is dead I can no longer protect him, I have failed ;_;");
            return Action.State.NotBeingExecuted;
        }
        allyDirection = AllyTransform.position - AgentTransform.position;
        allyDirection.Normalize();
        //offset to the middle (basic prediction off where more danger might come from)
        directionOffset = new Vector3(0.0f, 1.0f, 7.0f) - AllyTransform.position;
        directionOffset.Normalize();
        TargetPosition = AllyTransform.position + directionOffset;
        direction = TargetPosition - AgentTransform.position;

        AgentTransform.position += direction * movementSpeed * Time.deltaTime;
        return Action.State.BeingExecuted;
    }
}
