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

        InitAgentLog();
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
        //GoalList.Add(new CollectResourcesGoal(transform));
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
            if (newDist < maxDist && !AgentObj.GetComponent<Agent>().GetIsDead() && AgentObj.GetComponent<TetrahedronAgent>() == null)
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
        Vector4 ColorVec = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        if(BlueTeamAllegianceScore == RedTeamAllegianceScore && BlueTeamAllegianceScore == GreenTeamAllegianceScore && BlueTeamAllegianceScore != 0)
        {
            BlueTeamAllegianceScore = 0;
            RedTeamAllegianceScore = 0;
            GreenTeamAllegianceScore = 0;
            myMat.color = ColorVec;
            //remove all relations (cut from player's roles)
        }
        if(BlueTeamAllegianceScore - RedTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            BlueTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.z = 1;
        }
        if(BlueTeamAllegianceScore - GreenTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            BlueTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.z = 1;
        }
        if(RedTeamAllegianceScore - BlueTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            RedTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.x = 1;
        }
        if(RedTeamAllegianceScore - GreenTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            RedTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.x = 1;
        }
        if(GreenTeamAllegianceScore - BlueTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            GreenTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.y = 1;
        }
        if(GreenTeamAllegianceScore - RedTeamAllegianceScore >= AllegianceScoreTreshold)
        {
            GreenTeamAllegianceScore = AllegianceScoreTreshold;
            ColorVec.y = 1;
        }
        myMat.color = ColorVec;
        UpdateRelations();
    }
    private void UpdateRelations()
    {

        //TODO: UPDATEASPROVIDABLE
        Material myMat = transform.GetComponent<Renderer>().material;
        if (myMat.color.Equals(Color.black))
        {
            //no allies no enemies
            UpdateAsTeammateToTeam(false, false, false);
            UpdateAsCompetitorToTeam(false, false, false);
            LogAgentActionResult("Lost Relations");
        }
        else if (myMat.color.Equals(Color.blue))
        {
            //ally with Blue, enemy with rest
            UpdateAsTeammateToTeam(false, true, false);
            UpdateAsCompetitorToTeam(true, false, true);
            LogAgentActionResult("Allied with Blue Team, Enemy with Red and Green Teams");
        }
        else if (myMat.color.Equals(Color.red))
        {
            //ally with red enemy with rest
            UpdateAsTeammateToTeam(true, false, false);
            UpdateAsCompetitorToTeam(false, true, true);
            LogAgentActionResult("Allied with Red Team, Enemy with Blue and Green Teams");
        }
        else if (myMat.color.Equals(Color.green))
        {
            //ally with green enemy with rest
            UpdateAsTeammateToTeam(false, false, true);
            UpdateAsCompetitorToTeam(true, true, false);
            LogAgentActionResult("Allied with Green Team, Enemy with Red and Blue Teams");
        }
        else if (myMat.color.Equals(Color.magenta))
        {
            //ally with red and blue, enemy with green
            UpdateAsTeammateToTeam(true, true, false);
            UpdateAsCompetitorToTeam(false, false, true);
            LogAgentActionResult("Allied with Red and Blue Teams, Enemy with Green Team");
        }
        else if (myMat.color.Equals(Color.yellow))
        {
            //ally with green and red, enemy with blue
            UpdateAsTeammateToTeam(true, false, true);
            UpdateAsCompetitorToTeam(false, true, false);
            LogAgentActionResult("Allied with Red and Green Teams, Enemy with Blue Team");
        }
        else if (myMat.color.Equals(Color.cyan))
        {
            //ally with green and blue, enemy with red
            UpdateAsTeammateToTeam(false, true, true);
            UpdateAsCompetitorToTeam(true, false, false);
            LogAgentActionResult("Allied with Blue and Green Teams, Enemy with Red Team");
        }
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
            //WalkRandomly(); just stop
        }
    }
    private void UpdateAsTeammateToTeam(bool RedTeamBool, bool BlueTeamBool, bool GreenTeamBool)
    {
        //if true add as teammate, if false remove
        for(int i = 0; i < 3; i++) //hardcoded for 3 teams, IDC
        {
            GameObject[] TargetTeam = null; //ignorable assignment so compiler accepts this
            bool activeBool = false;        //ignorable assignment so compiler accepts this
            if (i == 0) { TargetTeam = GameObject.FindGameObjectsWithTag("RedTeam"); activeBool = RedTeamBool; }
            if (i == 1) { TargetTeam = GameObject.FindGameObjectsWithTag("BlueTeam"); activeBool = BlueTeamBool; }
            if (i == 2) { TargetTeam = GameObject.FindGameObjectsWithTag("GreenTeam"); activeBool = GreenTeamBool;  }
            foreach(GameObject Teammate in TargetTeam)
            {
                if (Teammate != this.transform.gameObject)
                {
                    if (activeBool)
                    {

                        Teammate.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "TeammateRole").AddTeammate(this.transform);
                        Teammate.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "TeammateRole").DebugLogTeammates();

                    }
                    else
                    {
                        Teammate.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "TeammateRole").RemoveTeammate(this.transform);
                    }
                }
            }
        }
    }
    private void UpdateAsCompetitorToTeam(bool RedTeamBool, bool BlueTeamBool, bool GreenTeamBool)
    {
        //if true add as teammate, if false remove
        for (int i = 0; i < 3; i++) //hardcoded for 3 teams, IDC
        {
            GameObject[] TargetTeam = null; //ignorable assignment so compiler accepts this
            bool activeBool = false;        //ignorable assignment so compiler accepts this
            if (i == 0) { TargetTeam = GameObject.FindGameObjectsWithTag("RedTeam"); activeBool = RedTeamBool; }
            if (i == 1) { TargetTeam = GameObject.FindGameObjectsWithTag("BlueTeam"); activeBool = BlueTeamBool; }
            if (i == 2) { TargetTeam = GameObject.FindGameObjectsWithTag("GreenTeam"); activeBool = GreenTeamBool; }
            foreach (GameObject Competitor in TargetTeam)
            {
                if (Competitor != this.transform.gameObject)
                {
                    //Competitor.GetComponent<Agent>().DebugLogRoles();
                    if (activeBool)
                    {
                        Competitor.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "CompetitorRole").AddCompetitor(this.transform);
                        Competitor.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "CompetitorRole").DebugLogCompetitors();

                    }
                    else
                    {
                        Competitor.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "CompetitorRole").RemoveCompetitor(this.transform);
                        //Competitor.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "CompetitorRole").DebugLogCompetitors();
                    }
                }
            }
        }
    }


}
