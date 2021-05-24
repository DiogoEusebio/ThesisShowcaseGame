using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagAction : Action
{
    private Transform AgentTransform;
    private Vector3 targetPosition;
    private Vector3 direction;
    private float movementSpeed = 10.0f;
    private Transform CapturedFlag = null;

    public CaptureFlagAction(Transform Agent)
    {
        name = "CaptureFlagAction";
        AgentTransform = Agent;
        targetPosition = GetClosestFlagPosition();
        direction = targetPosition - AgentTransform.position;
        direction.y = 0;
        direction.Normalize();
    }
    public override State Perform()
    {
        if (targetPosition == null) { return State.NotBeingExecuted; }
        Debug.Log(CapturedFlag + " | " + targetPosition);
        AgentTransform.position += direction * movementSpeed * Time.deltaTime;
        //Capturing Flag Condition
        if (Vector3.Distance(AgentTransform.position, targetPosition) < 0.2f && CapturedFlag == null)
        {
            //not really the true stop condition
            ActionState = State.BeingExecuted;
        }
        //Returning Flag Condition
        if(Vector3.Distance(AgentTransform.position, targetPosition) < 0.2f && CapturedFlag != null)
        {
            Debug.Log("RETURNING TO BASE");
            DropFlag();
            ActionState = State.Executed;
        }
        return ActionState;
    }
    public override void SetCapturedFlag(Transform flag){
        CapturedFlag = flag;
        Debug.Log("Setting captured flag");
        //this method also updates the agent target position
        //as now having the flag we want to return it to our base
        targetPosition = GetBasePosition();
        Debug.Log(targetPosition);
        UpdateDirection();
    }
    public override Vector3 GetClosestFlagPosition()
    {
        float MinDistance = 10000.0f; //consider changing to max float
        float currDist;
        Transform[] Flags1;
        Transform[] Flags2;
        List<Transform> Flags = new List<Transform>();
        //Debug.Log(AgentTransform.CompareTag("RedTeam"));
        if (AgentTransform.CompareTag("RedTeam"))
        {
            Flags1 = GameObject.Find("BlueTeamBase").GetComponentsInChildren<Transform>();
            Flags2 = GameObject.Find("GreenTeamBase").GetComponentsInChildren<Transform>();
            foreach(Transform t in Flags1)
            {
                if(t == GameObject.Find("BlueTeamBase").transform)
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags2)
            {
                if (t == GameObject.Find("GreenTeamBase").transform)
                {
                    continue;
                }
                Flags.Add(t);
            }
        }
        else if (AgentTransform.CompareTag("BlueTeam"))
        {
            Flags1 = GameObject.Find("RedTeamBase").GetComponentsInChildren<Transform>();
            Flags2 = GameObject.Find("GreenTeamBase").GetComponentsInChildren<Transform>();
            foreach (Transform t in Flags1)
            {
                if (t == GameObject.Find("RedTeamBase").transform)
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags2)
            {
                if (t == GameObject.Find("GreenTeamBase").transform)
                {
                    continue;
                }
                Flags.Add(t);
            }
        }
        else if (AgentTransform.CompareTag("GreenTeam"))
        {
            Flags1 = GameObject.Find("BlueTeamBase").GetComponentsInChildren<Transform>();
            Flags2 = GameObject.Find("RedTeamBase").GetComponentsInChildren<Transform>();
            foreach (Transform t in Flags1)
            {
                if (t == GameObject.Find("BlueTeamBase").transform)
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags2)
            {
                if (t == GameObject.Find("RedTeamBase").transform)
                {
                    continue;
                }
                Flags.Add(t);
            }
        }
        if (Flags.Count == 0)
        {
            //change to exception or alike later
            Vector3 err = new Vector3(1000f, 1000f, 1000f);
            return err;
        }
        foreach (Transform t in Flags)
        {
            currDist = Vector3.Distance(AgentTransform.position, t.position);
            if (currDist < MinDistance)
            {
                MinDistance = currDist;
                targetPosition = t.position;
            }
        }
        //Debug.Log(targetPosition + " | " + AgentTransform.position);
        return targetPosition;
    }

    public Vector3 GetBasePosition()
    {
        if (AgentTransform.CompareTag("RedTeam")) { return new Vector3(12.87f, 1.0f, 14.77f); }
        if (AgentTransform.CompareTag("BlueTeam")) { return new Vector3(-12.87f, 1.0f, 14.77f); }
        if (AgentTransform.CompareTag("GreenTeam")) { return new Vector3(0.0f, 1.0f, -7.0f); }
        else
        {
            //THIS SHOULD NEVER HAPPEN
            Debug.Log("ERR: AGENT AS NO TAG");
            return new Vector3(0, 0, 0);
        }
    }
    public override void UpdateDirection()
    {
        //Debug.Log(agentTransform.gameObject + "updating Direction");
        direction = targetPosition - AgentTransform.position;
        direction.y = 0;
        direction.Normalize();
        //Debug.Log(direction);
        //quick "look at" action incorporated
        var lookPos = targetPosition - AgentTransform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        AgentTransform.rotation = Quaternion.Slerp(AgentTransform.rotation, rotation, Time.deltaTime * 10);
    }
    public void DropFlag() { Debug.Log("Should be droping flag"); }
}
