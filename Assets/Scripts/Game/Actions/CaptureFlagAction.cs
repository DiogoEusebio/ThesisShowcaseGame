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
        direction.y = 1;
        direction.Normalize();
    }
    public override State Perform()
    {
        if (targetPosition == null) { return State.NotBeingExecuted; }
        AgentTransform.position += direction * movementSpeed * Time.deltaTime;
        //Stop condition
        if (Vector3.Distance(AgentTransform.position, targetPosition) < 0.2f)
        {
            ActionState = State.Executed;
        }
        return ActionState;
    }
    public void SetCapturedFlag(Transform flag) { CapturedFlag = flag; }
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
    public void DropFlag() { }
}
