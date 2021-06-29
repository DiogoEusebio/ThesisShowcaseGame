using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagAction : Action
{
    private Transform AgentTransform;
    private Vector3 targetPosition;
    private Vector3 direction;
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
        if(CapturedFlag == null)
        {
            targetPosition = GetClosestFlagPosition();
        }
        //Debug.Log(CapturedFlag + " | " + targetPosition);
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
            //Debug.Log("RETURNING TO BASE");
            if (AgentTransform.CompareTag("RedTeam"))
            {
                GameObject.Find("RedTeamBase").GetComponentInChildren<TeamManager>().SpawnFlag(CapturedFlag);
                DropFlag();
            }
            if (AgentTransform.CompareTag("BlueTeam"))
            {
                GameObject.Find("BlueTeamBase").GetComponentInChildren<TeamManager>().SpawnFlag(CapturedFlag);
                DropFlag();
            }
            if (AgentTransform.CompareTag("GreenTeam"))
            {
                GameObject.Find("GreenTeamBase").GetComponentInChildren<TeamManager>().SpawnFlag(CapturedFlag);
                DropFlag();
            }
            ActionState = State.Executed;
        }
        return ActionState;
    }
    public override void SetCapturedFlag(Transform flag){
        CapturedFlag = flag;
        //Debug.Log("Setting captured flag");
        //this method also updates the agent target position
        //as now having the flag we want to return it to our base
        targetPosition = GetBasePosition();
        //Debug.Log(targetPosition);
        UpdateDirection();
    }
    public override Vector3 GetClosestFlagPosition()
    {
        float MinDistance = 10000.0f; //consider changing to max float
        float currDist;
        Transform[] Flags1 = GameObject.Find("BlueTeamBase").GetComponentsInChildren<Transform>();
        Transform[] Flags2 = GameObject.Find("GreenTeamBase").GetComponentsInChildren<Transform>();
        Transform[] Flags3 = GameObject.Find("RedTeamBase").GetComponentsInChildren<Transform>();
        List<Transform> Flags = new List<Transform>();
        //Debug.Log(AgentTransform.CompareTag("RedTeam"));
        if (AgentTransform.CompareTag("RedTeam"))
        {
            foreach (Transform t in Flags1)
            {
                if (t == GameObject.Find("BlueTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured())
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags2)
            {
                if (t == GameObject.Find("GreenTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured())
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags3)
            {
                if (t == GameObject.Find("RedTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured() || Vector3.Distance(t.position, GetBasePosition()) < 4.0f)
                {
                    continue;
                }
                Flags.Add(t);
            }
        }
        if (AgentTransform.CompareTag("BlueTeam"))
        {
            foreach (Transform t in Flags1)
            {
                if (t == GameObject.Find("BlueTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured() || Vector3.Distance(t.position, GetBasePosition()) < 4.0f)
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags2)
            {
                if (t == GameObject.Find("GreenTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured())
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags3)
            {
                if (t == GameObject.Find("RedTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured())
                {
                    continue;
                }
                Flags.Add(t);
            }
        }
        if (AgentTransform.CompareTag("GreenTeam"))
        {
            foreach (Transform t in Flags1)
            {
                if (t == GameObject.Find("BlueTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured())
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags2)
            {
                if (t == GameObject.Find("GreenTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured() || Vector3.Distance(t.position, GetBasePosition()) < 4.0f)
                {
                    continue;
                }
                Flags.Add(t);
            }
            foreach (Transform t in Flags3)
            {
                if (t == GameObject.Find("RedTeamBase").transform || t.GetComponentInChildren<FlagObjective>().IsCaptured())
                {
                    continue;
                }
                Flags.Add(t);
            }
        }
        if (Flags.Count == 0)
        {
            //change to exception or alike later
            Vector3 err = new Vector3(0f, 0f, 0f);
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
        /*
        var lookPos = targetPosition - AgentTransform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        AgentTransform.rotation = Quaternion.Slerp(AgentTransform.rotation, rotation, Time.deltaTime * 10);
        */
    }
    public override void DropFlag()
    {
        Agent mySelf = AgentTransform.GetComponentInChildren<Agent>();
        mySelf.DropFlag();
        mySelf.LogAgentActionResult("Dropped Flag");
        //Debug.Log("Should be droping flag");
        CapturedFlag.gameObject.GetComponentInChildren<FlagObjective>().Dropped();
        CapturedFlag.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        CapturedFlag.position += new Vector3(0.0f, -0.5f, 0.0f); //HACK: flag height adjustment due to bad flag prefab modeling
        CapturedFlag = null;
    }
}
