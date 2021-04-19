using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAgent : Agent
{
    // Start is called before the first frame update
    protected override void Start()
    {
        GenerateSphereGoals();
        GetActionsFromGoals();
    }

    // Update is called once per frame
    protected override void Update()
    {
        ContestObjective();
        //WalkRandomly();
    }

    void GenerateSphereGoals()
    {
        GameObject objective = GameObject.FindWithTag("Objective");
        Debug.Log(objective);
        Vector3 ObjPos = objective.transform.position;
        Debug.Log(ObjPos);
        GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x - 3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
    }
}
