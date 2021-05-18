using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResourceAction : Action
{
    private Transform AgentTransform;
    private Vector3 targetPosition;
    private Vector3 direction;
    private float movementSpeed = 10.0f;
    // Start is called before the first frame update
    public CollectResourceAction(Transform Agent)
    {
        name = "CollectResourceAction";
        AgentTransform = Agent;
        targetPosition = GetClosestResorcePosition();
        direction = targetPosition - AgentTransform.position;
        direction.Normalize();
    }
    public override State Perform()
    {
        if(targetPosition == null) { return State.NotBeingExecuted;  }
        AgentTransform.position += direction * movementSpeed * Time.deltaTime;
        //Stop condition
        if (Vector3.Distance(AgentTransform.position, targetPosition) < 0.5f)
        {
            ActionState = State.Executed;
        }
        return ActionState;
    }
    public override Vector3 GetClosestResorcePosition()
    {
        float MinDistance = 10000.0f; //consider changing to max float
        float currDist;
        Resource[] childs = GameObject.Find("ResourceBag").GetComponentsInChildren<Resource>();
        if(childs.Length == 0)
        {
            //change to exception or alike later
            Vector3 err = new Vector3(1000f, 1000f, 1000f);
            return err;
        }
        foreach (Resource r in childs)
        {
            currDist = Vector3.Distance(AgentTransform.position, r.transform.position);
            if (currDist < MinDistance)
            {
                MinDistance = currDist;
                targetPosition = r.transform.position;
            }
        }
        //Debug.Log(targetPosition);
        return targetPosition;
    }
    public override void UpdateDirection()
    {
        //Debug.Log(agentTransform.gameObject + "updating Direction");
        direction = targetPosition - AgentTransform.position;
        direction.Normalize();
        Debug.Log(direction);
        //quick "look at" action incorporated
        var lookPos = targetPosition - AgentTransform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        AgentTransform.rotation = Quaternion.Slerp(AgentTransform.rotation, rotation, Time.deltaTime * 10);
    }
}
