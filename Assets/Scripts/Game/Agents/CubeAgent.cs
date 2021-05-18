using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAgent : Agent
{
    protected override void Start()
    {
        SetMaxHP(100.0f);
        SetCurrentHPtoMax();
        GenerateBasicAgentGoals();
        GetActionsFromGoals();
    }

    protected override void Update()
    {
        if (GetIsDead())
        {
            respawnTimer -= Time.deltaTime;
            RespawnAgent(respawnTimer);
        }
        else
        {
            //ContestObjective();
            CaptureFlag();
        }
    }
    protected override void GenerateBasicAgentGoals()
    {
        GameObject objective = GameObject.FindWithTag("Objective");
        //Debug.Log(objective);
        Vector3 ObjPos = objective.transform.position;
        //Debug.Log(ObjPos);
        //GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x - 3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
        GoalList.Add(new CaptureFlagGoal(transform));
    }
}
