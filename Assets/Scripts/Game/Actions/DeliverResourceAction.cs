using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverResourceAction : Action
{
    private Transform AgentTransform;
    private Transform TargetAgentTransform;
    private Transform ResourceTransform;
    private Vector3 direction;
    // Start is called before the first frame update
    public DeliverResourceAction(Transform Agent, Transform Resource, Transform TargetAgent)
    {
        name = "DeliverResourceAction";
        AgentTransform = Agent;
        TargetAgentTransform = TargetAgent;
        ResourceTransform = Resource;
        direction = TargetAgentTransform.position - AgentTransform.position;
        direction.Normalize();
    }
    private void LookAt()
    {
        var lookPos = TargetAgentTransform.position - AgentTransform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        AgentTransform.rotation = Quaternion.Slerp(AgentTransform.rotation, rotation, Time.deltaTime * 10);
    }
    public override Transform GetTargetAgent() { return TargetAgentTransform; }
    public override State Perform()
    {
        //Debug.Log(TargetAgentTransform + " | " + ResourceTransform); //consider this case: || ResourceTransform == null
        //PROBLEM HERE; HELPPP!!!
        if (TargetAgentTransform == null || ResourceTransform == null || TargetAgentTransform.GetComponent<Agent>().GetIsDead())
        {
            Debug.Log("either there are no agents to delivere resources to, or this agent has no resource collected");
            return State.NotBeingExecuted;
        }
        else
        {
            direction = TargetAgentTransform.position - AgentTransform.position; //need to update direction since the other agent is likely moving
            direction.Normalize();
            AgentTransform.position += direction * (movementSpeed * 1.01f) * Time.deltaTime; //+1% movespeed to garantee that the agent catches up to the target
            LookAt();
            if (Vector3.Distance(AgentTransform.position, TargetAgentTransform.position) < 1.0f) //relaxed distance check so that agents dont clup for long
            {
                ResourceTransform.parent = null;
                ResourceTransform.GetComponentInChildren<Resource>().ConsumedBy(TargetAgentTransform);
                AgentTransform.GetComponent<Agent>().LogAgentActionResult("Delivered Resource");
                return State.Executed;
            }
            return State.BeingExecuted;
        }
    }
}
