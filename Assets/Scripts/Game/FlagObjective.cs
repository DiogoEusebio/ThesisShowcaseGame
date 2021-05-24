using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagObjective : MonoBehaviour
{
    private bool captured = false;
    private Transform CaptureAgentTransform = null;
    private Agent CaptureAgent;
    void Start()
    {
        //Debug.Log("FLAG EXISTS");
    }

    // Update is called once per frame
    void Update()
    {
        if (captured)
        {
            if(!CaptureAgent.GetIsDead())
                transform.position = CaptureAgentTransform.position;
        }
        if (CaptureAgent.GetIsDead())
        {
            //maybe change this check to agent class
            Action captureAction = CaptureAgent.GetActionList().Find((action) => action.GetName() == "CaptureFlagAction");
            captureAction.DropFlag();
        }
    }

    public bool IsCaptured() { return captured; }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Flag Collision: " + other);
        if (other.TryGetComponent(out Agent Agent))
        {
            if (!Agent.HasCapturedFlag())
            {
                Agent.SetCapturedFlag(this.transform);
                if (Agent.GetAgentType() == Agent.AgentType.Cube)
                {
                    if (!IsCaptured())
                    {
                        //assign agent to captureAgent
                        captured = true;
                        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        Debug.Log("Flag collision " + other);
                        CaptureAgentTransform = other.transform;
                        CaptureAgent = Agent;
                        Debug.Log(CaptureAgentTransform);
                        Action captureAction = Agent.GetActionList().Find((action) => action.GetName() == "CaptureFlagAction");
                        if (captureAction != null)
                        {
                            captureAction.SetCapturedFlag(this.transform);
                        }
                    }
                }
            }
        }
    }
}
