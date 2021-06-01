using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    protected List<Goal> GoalList = new List<Goal>();
    protected List<Action> ActionList = new List<Action>();
    public List<Role> RoleList = new List<Role>();
    protected Goal GoalBeingPursued;
    protected Action ActionToExecute;
    private float MaxHP;
    private float CurrentHP;
    public bool isDead = false;
    protected float respawnTimer;
    protected Transform capturedFlag;
    private Vector3 targetLookAtPosition;
    public enum AgentType
    {
        Cube,
        Sphere,
        Cone,
        Tetrahedron
    }
    private AgentType agentType;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetMaxHP(100.0f);
        SetCurrentHPtoMax();
        GenerateBasicAgentGoals();
        GetActionsFromGoals();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDead)
        {
            respawnTimer -= Time.deltaTime;
            RespawnAgent(respawnTimer);
        }
        else
        {
            WalkRandomly();
        }
    }
    protected virtual void FixedUpdate()
    {
        //do nothing
    }
    //--------------------- Gets and Sets -----------------------//
    public void SetAgentType(AgentType at) { agentType = at; }
    public AgentType GetAgentType() { return agentType; }
    public List<Role> GetRoleList() { return RoleList; }
    public List<Action> GetActionList(){ return ActionList; }
    public bool GetIsDead() { return isDead; }
    public void SetIsDead(bool value) { isDead = value; }
    public void SetMaxHP(float newMaxHPvalue) { MaxHP = newMaxHPvalue; /*HealthBar.SetMaxHealth((int)newMaxHPvalue);*/ }
    public void SetCurrentHPtoMax() { CurrentHP = MaxHP; }
    public void TakeDamage(float dmg)
    {
        //Debug.Log("current Hp: " + CurrentHP);
        CurrentHP -= dmg;
        /*HealthBar.SetHealth((int)CurrentHP);*/
        if(CurrentHP <= 0)
        {
            KillAgent();
        }
    }
    public bool HasCapturedFlag()
    {
        if (capturedFlag == null)
        {
            return false;
        }
        return true;
    }
    public void SetCapturedFlag(Transform flag)
    {
        capturedFlag = flag;
    }
    public void DropFlag()
    {
        capturedFlag = null;
    }
    public void GetActionsFromGoals()
    {
        //Debug.Log("GETTING ACTIONS...");
        //TODO: add check for repeated actions
        foreach(Goal g in GoalList)
        {
            //Debug.Log(g.GetName());
            ActionList.AddRange(g.GetActionsFromGoal());
        }
    }
    public void GetActionsFormSpecificGoal(Goal g)
    {
        if(GoalList.Contains(g))
        {
            ActionList.AddRange(g.GetActionsFromGoal());
        }
    }
    public void GenerateGoalsBasedOnRole(Role role)
    {
        //TODO
    }
    protected virtual void GenerateBasicAgentGoals()
    {
        GoalList.Add(new MoveToTargetCoordsGoal(transform, new Vector3(Random.Range(-28.0f,28.0f), 1.0f, Random.Range(-5.0f, 5.0f))));
    }
    public Vector3 GetAgentCurrentPosition()
    {
        return transform.position;
    }

    //might need to check if every case is covered
    public void RemoveActionsAssociatedToGoal(Goal goalBeingRemoved)
    {   
        //remove actions exclusive to this goal, leaving shared actions behind
        foreach(Action a in goalBeingRemoved.GetActionsFromGoal())
        {
            bool canBeRemoved = true;
            foreach (Goal g in GoalList)
            {
                if(g != goalBeingRemoved)
                {
                    foreach (Action b in g.GetActionsFromGoal())
                    {
                        if (a.GetName() == b.GetName())
                        {
                            canBeRemoved = false;
                            break; //no need to check further, since we now know we cant remove the action because it is shared with other goals
                        }
                    }
                    //check if we can also stop this loop
                    if (!canBeRemoved) { break; }
                }
            }
            if (canBeRemoved)
            {
                ActionList.Remove(a);
                //Debug.Log("ACTION REMOVED: " + a);
            }
        }                
    }
    public void RemoveAchivedGoal(Goal toRemove)
    {
        GoalList.Remove(toRemove);
    }

    public Action GetRandomActionToAchiveSpecifiedGoal(Goal g)
    {
        int numberOfActions = g.GetActionsFromGoal().Count; //-1 to get list index
        //Debug.Log(numberOfActions);
        Action RandomActionFromThisGoal = g.GetActionsFromGoal()[Random.Range(0, numberOfActions - 1)];
        //Debug.Log(RandomActionFromThisGoal.GetName());
        return RandomActionFromThisGoal;
    }

    public void PerformSimulActions()
    {
        Action LookAtAction = ActionList.Find((action) => action.GetName() == "LookAtCloserEnemyAction");
        if (LookAtAction != null) { LookAtAction.Perform(); }
    }

    public void KillAgent()
    {
        if (HasCapturedFlag())
        {
            Action CapFlag = ActionList.Find((action) => action.GetName() == "CaptureFlagAction");
            CapFlag.DropFlag();
        }
        isDead = true;
        this.transform.position = new Vector3(0.0f, -50.0f, 0.0f);
        //Clear Roles, Goals and Actions
        ActionList = new List<Action>();
        GoalList = new List<Goal>();
        RoleList = new List<Role>();
        respawnTimer = 0.05f * Time.realtimeSinceStartup + 2.0f; //consider other equation
        RespawnAgent(respawnTimer);
    }
    public void RespawnAgent(float respawnTimer)
    {
        if (respawnTimer <= 0)
        {
            isDead = false;
            if (transform.gameObject.CompareTag("BlueTeam"))
            {
                transform.position = new Vector3(-12.87f + Random.Range(-2.5f, 2.5f), 1.0f, 14.77f + Random.Range(-2.5f, 2.5f)); //Hack: copy pasted value from AgentManager, consider doing this there/acess the variable
                SetMaxHP(100.0f);
                SetCurrentHPtoMax();
                GenerateBasicAgentGoals();
                GetActionsFromGoals();
            }
            else if (transform.gameObject.CompareTag("RedTeam"))
            {
                transform.position = new Vector3(12.87f + Random.Range(-2.5f, 2.5f), 1.0f, 14.77f + Random.Range(-2.5f, 2.5f)); //Hack: copy pasted value from AgentManager, consider doing this there/acess the variable
                SetMaxHP(100.0f);
                SetCurrentHPtoMax();
                GenerateBasicAgentGoals(); //problem here
                GetActionsFromGoals();
            }
            else if (transform.gameObject.CompareTag("GreenTeam"))
            {
                transform.position = new Vector3(0.0f + Random.Range(-2.5f, 2.5f), 1.0f, -7.0f + Random.Range(-2.5f, 2.5f)); //Hack: copy pasted value from AgentManager, consider doing this there/acess the variable
                SetMaxHP(100.0f);
                SetCurrentHPtoMax();
                GenerateBasicAgentGoals(); //problem here
                GetActionsFromGoals();
            }
        }
    }
    //-------------------------- Debug Methods -----------------------//

    void OnDrawGizmos()
    {
        Color color;
        color = Color.green;
        DrawHelperAtCenter(this.transform.forward, color, 2f);
    }
    private void DrawHelperAtCenter(Vector3 direction, Color color, float scale)
    {
        Gizmos.color = color;
        Vector3 destination = transform.position + direction * scale;
        Gizmos.DrawLine(transform.position, destination);
    }
    public void DebugLogActions()
    {   
        string DebugLogActionsString = "";
        foreach(Action a in ActionList)
        {
            DebugLogActionsString += " | " + a.GetName();
        }
        Debug.Log(DebugLogActionsString);
    }
    public void DebugLogGoals()
    {
        string DebugLogGoalsString = "";
        foreach (Goal a in GoalList)
        {
            DebugLogGoalsString += " | " + a.GetName();
        }
        Debug.Log(DebugLogGoalsString);
    }

    //------------------------ AI/Behavior methods -----------------------------//
    public void WalkRandomly()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "MoveToPositionGoal");
        //PerformSimulActions(); //look at doesn work with random Action (gets stuck just looking)
        ActionToExecute = GetRandomActionToAchiveSpecifiedGoal(GoalBeingPursued);
        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            GoalList.Add(new MoveToTargetCoordsGoal(transform, new Vector3(Random.Range(-12.0f, 12.0f), 1.0f, Random.Range(-7.0f, 22.0f))));
        }
    }

    public void ContestObjective()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "ContestObjectiveGoal");
        PerformSimulActions();
        ActionToExecute = ActionList.Find((action) => action.GetName() == "MoveToPositionAction");
        ActionToExecute.UpdateDirection();
        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            GameObject objective = GameObject.FindWithTag("Objective");
            Vector3 ObjPos = objective.transform.position;
            GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x -3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
            GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "ContestObjectiveGoal"));
        }
    }
    public void CaptureFlag()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "CaptureFlagGoal");
        ActionToExecute = ActionList.Find((action) => action.GetName() == "CaptureFlagAction");
        //DebugLogActions();
        //Debug.Log(GoalBeingPursued + " | " + ActionToExecute);
        ActionToExecute.UpdateDirection();
        //if (ActionToExecute.GetClosestFlagPosition() != new Vector3(1000f, 1000f, 1000f))
        
        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            //GameObject objective = GameObject.FindWithTag("Objective"); //remove later and add look at resource action
            //Vector3 ObjPos = objective.transform.position;
            GoalList.Add(new CaptureFlagGoal(transform));
            GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "CaptureFlagGoal"));
            //Debug.Log("Flag Captured");
        }

        /*else
        {
            Debug.Log("Waiting for Flags to spawn");
        }*/
    }
}
