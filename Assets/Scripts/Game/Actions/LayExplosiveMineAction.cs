using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayExplosiveMineAction : Action
{
    public GameObject MinePrefab;
    private Transform agentTransform;
    public LayExplosiveMineAction(Transform AgentTransform)
    {
        name = "LayExplosiveMineAction";
        agentTransform = AgentTransform;
        MinePrefab = (GameObject)Resources.Load("Prefabs/ExplosiveMine", typeof(GameObject));
    }
    public override State Perform()
    {
        Vector3 yOffset = new Vector3(0f, 0.38f, 0f);
        GameObject b = GameObject.Instantiate(MinePrefab, agentTransform.position - yOffset, Quaternion.identity);
        b.GetComponent<ExplosiveMine>().setParent(agentTransform);
        b.GetComponent<Renderer>().material = agentTransform.GetComponent<Renderer>().material;
        agentTransform.GetComponent<Agent>().LogAgentActionResult("layed mine | position " + agentTransform.position);
        ActionState = State.Executed;
        return ActionState;

    }
}
