using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagObjective : MonoBehaviour
{
    private bool captured = false;
    private Transform CaptureAgentTransform = null;
    private Agent CaptureAgent;
    private string currentTeamHolding;
    void Start()
    {
        //Debug.Log("FLAG EXISTS");
        if (transform.parent.CompareTag("RedTeamBase"))
        {
            currentTeamHolding = "RedTeam";
        }
        if (transform.parent.CompareTag("BlueTeamBase"))
        {
            currentTeamHolding = "BlueTeam";
        }
        if (transform.parent.CompareTag("GreenTeamBase"))
        {
            currentTeamHolding = "GreenTeam";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (captured)
        {
            if(!CaptureAgent.GetIsDead())
                transform.position = CaptureAgentTransform.position;
        }
        /*else
        {
            if (Vector3.Distance(transform.position, new Vector3(12.87f, 1.0f, 14.77f)) < 2.5f)
            {
                GameObject.Find("RedTeamBase").GetComponentInChildren<TeamManager>().SpawnFlag(transform);
            }
            if (Vector3.Distance(transform.position, new Vector3(-12.87f, 1.0f, 14.77f)) < 2.5f)
            {
                GameObject.Find("BlueTeamBase").GetComponentInChildren<TeamManager>().SpawnFlag(transform);
            }
            if (Vector3.Distance(transform.position, new Vector3(0.0f, 1.0f, -7.0f)) < 2.5f)
            {
                GameObject.Find("GreenTeamBase").GetComponentInChildren<TeamManager>().SpawnFlag(transform);
            }
        }*/
    }
    public void SetCurrentHoldingTeamBasedOnParentName()
    {
        if (transform.parent.CompareTag("RedTeamBase"))
        {
            currentTeamHolding = "RedTeam";
        }
        if (transform.parent.CompareTag("BlueTeamBase"))
        {
            currentTeamHolding = "BlueTeam";
        }
        if (transform.parent.CompareTag("GreenTeamBase"))
        {
            currentTeamHolding = "GreenTeam";
        }
    }
    public void Dropped()
    {
        CaptureAgentTransform = null;
        CaptureAgent = null;
        captured = false;
    }
    public bool IsCaptured() { return captured; }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Flag Collision: " + other);
        if (other.TryGetComponent(out Agent Agent))
        {
            //if (!Agent.HasCapturedFlag() && !Agent.transform.CompareTag(currentTeamHolding))
            if (!Agent.HasCapturedFlag())
            {
                if (Agent.GetAgentType() != Agent.AgentType.Tetrahedron)
                {
                    if (!IsCaptured())
                    {
                        Agent.SetCapturedFlag(this.transform);
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
