using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrahedronAgent : Agent
{
    private bool delivering = false;
    private int BlueTeamAllegianceScore = 0;
    private int RedTeamAllegianceScore = 0;
    private int GreenTeamAllegianceScore = 0;
    public int AllegianceScoreTreshold;
    protected override void Start()
    {
        SetMaxHP(50.0f);
        SetCurrentHPtoMax();
        GenerateBasicAgentGoals();
        GetActionsFromGoals();
        UpdateAllegiance();
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
            GatherAndDeliverResources();
            //CaptureFlag();
        }
    }
    protected override void GenerateBasicAgentGoals()
    {
        //GameObject objective = GameObject.FindWithTag("Objective");
        //Debug.Log(objective);
        //Vector3 ObjPos = objective.transform.position;
        //Debug.Log(ObjPos);
        //GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x - 3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
        GoalList.Add(new CollectResourcesGoal(transform));
        GoalList.Add(new MoveToTargetCoordsGoal(transform, new Vector3(Random.Range(-12f, 12f), 1.0f, Random.Range(-7f, 22f))));
        //GoalList.Add(new CaptureFlagGoal(transform));
    }
    private Transform GetClosestAgentTransform()
    {
        List<GameObject> AllAgent = GetListOfPlayers(GameObject.FindGameObjectsWithTag("RedTeam"),
                                                      GameObject.FindGameObjectsWithTag("BlueTeam"),
                                                      GameObject.FindGameObjectsWithTag("GreenTeam"));
        if(AllAgent.Count == 0)
        {
            Debug.Log("NO AGENTS TO DELIVER RESOURCES TO");
            return null;
        }
        float maxDist = 1000f; //consider changing to maxfloat
        Transform ClossestAgent = null;
        foreach(GameObject AgentObj in AllAgent)
        {
            float newDist = Vector3.Distance(AgentObj.transform.position, transform.position);
            if (newDist < maxDist && !AgentObj.GetComponent<Agent>().GetIsDead())
            {
                maxDist = newDist;
                ClossestAgent = AgentObj.transform;
            }
        }
        return ClossestAgent;
    }
    List<GameObject> GetListOfPlayers(GameObject[] team1, GameObject[] team2, GameObject[] team3)
    {
        //here players means agents belonging to a team, consider finding a more sutiable name
        List<GameObject> Agents = new List<GameObject>();
        foreach (GameObject obj in team1)
        {
            Agents.Add(obj);
        }
        foreach (GameObject obj in team2)
        {
            Agents.Add(obj);
        }
        foreach (GameObject obj in team3)
        {
            Agents.Add(obj);
        }
        return Agents;
    }
    private void UpdateAllegiance()
    {
        Material myMat = transform.GetComponent<Renderer>().material;
        Vector4 ColorVec = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        if(BlueTeamAllegianceScore == RedTeamAllegianceScore && BlueTeamAllegianceScore == GreenTeamAllegianceScore && BlueTeamAllegianceScore != 0)
        {
            BlueTeamAllegianceScore = 0;
            RedTeamAllegianceScore = 0;
            GreenTeamAllegianceScore = 0;
            myMat.color = ColorVec;
            //remove all relations (cut roles from players)
        }
        if(BlueTeamAllegianceScore - RedTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            BlueTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.z = 1;
            //Set Red as enemy and blue as teammate
        }
        if(BlueTeamAllegianceScore - GreenTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            BlueTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.z = 1;
            //Set Green as enemy and blue as teammate
        }
        if(RedTeamAllegianceScore - BlueTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            Debug.Log("Red > Blue");
            RedTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.x = 1;
            //Set Blue as enemy and Red as teammate
        }
        if(RedTeamAllegianceScore - GreenTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            Debug.Log("Red > Green");
            RedTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.x = 1;
            //Set Green as enemy and Red as teammate
        }
        if(GreenTeamAllegianceScore -BlueTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            GreenTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.y = 1;
            //Set Blue as enemy and Green as teammate
        }
        if(GreenTeamAllegianceScore -RedTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            GreenTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.y = 1;
            //Set Red as enemy and Green as teammate
        }
        myMat.color = ColorVec;
    }
    private void GatherAndDeliverResources()
    {
        if (delivering)
        {
            DeliverResource();
        }
        else
        {
            GatherResources();
        }
    }
    private void DeliverResource()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "DeliverResourceGoal");
        ActionToExecute = ActionList.Find((action) => action.GetName() == "DeliverResourceAction");
        Action.State result = ActionToExecute.Perform();
        if (result == Action.State.Executed)
        {
            string deliveredTag = ActionToExecute.GetTargetAgent().tag;
            if(deliveredTag == "BlueTeam") { BlueTeamAllegianceScore++; }
            else if(deliveredTag == "RedTeam") { RedTeamAllegianceScore++; }
            else if(deliveredTag == "GreenTeam") { GreenTeamAllegianceScore++; }
            UpdateAllegiance();
        }
        if (result == Action.State.Executed || result == Action.State.NotBeingExecuted)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);

            if (GetComponentInChildren<Resource>() != null)
            {
                //aka if agent has more than one resource collected
                Transform ResourceToDeliver = GetComponentInChildren<Resource>().transform;
                GoalList.Add(new DeliverResourceGoal(transform, GetClosestAgentTransform(), ResourceToDeliver));
                GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "DeliverResourceGoal"));
                Debug.Log("delivering yet another resource");
            }
            else
            {
                GoalList.Add(new CollectResourcesGoal(transform));
                GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "CollectResourcesGoal"));
                delivering = false;
            }
        }
    }
    private void GatherResources()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "CollectResourcesGoal");
        ActionToExecute = ActionList.Find((action) => action.GetName() == "CollectResourceAction");
        ActionToExecute.UpdateDirection();
        if(ActionToExecute.GetClosestResorcePosition() != new Vector3(1000f, 1000f, 1000f))
        {
            if (ActionToExecute.Perform() == Action.State.Executed)
            {
                GoalBeingPursued.SetGoalState(Goal.State.Achieved);
                RemoveAchivedGoal(GoalBeingPursued);
                RemoveActionsAssociatedToGoal(GoalBeingPursued);
                Transform ResourceToDeliver = GetComponentInChildren<Resource>().transform;
                GoalList.Add(new DeliverResourceGoal(transform, ResourceToDeliver, GetClosestAgentTransform()));
                GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "DeliverResourceGoal"));
                //Debug.Log("Collected Resource, now delivering");
                delivering = true;
            }
        }
        else
        {
            //WalkRandomly();
        }
    }
}
