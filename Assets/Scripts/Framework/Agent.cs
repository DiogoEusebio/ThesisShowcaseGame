using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    protected List<Goal> GoalList = new List<Goal>();
    protected List<Action> ActionList = new List<Action>();
    protected List<Role> RoleList = new List<Role>();
    protected Goal GoalBeingPursued;
    protected Action ActionToExecute;
    public enum AgentType
    {
        Cube,
        Sphere,
        Cone
    }
    private AgentType agentType;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        GenereateBasicAgentGoals();
        GetActionsFromGoals();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        WalkRandomly();
    }

    public void SetAgentType(AgentType at) { agentType = at; }
    public AgentType GetAgentType() { return agentType; }
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
    public void GenerateGoalsBasedOnRole(Role role)
    {
        //TODO
    }
    public void GenereateBasicAgentGoals()
    {
        GoalList.Add(new MoveToTargetCoordsGoal(transform, new Vector3(Random.Range(-28.0f,28.0f), 1.0f, Random.Range(-5.0f, 5.0f))));
        if(agentType == AgentType.Cone)
        {
            GoalList.Add(new ShootEnemyGoal(transform));
        }
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
            GoalList.Add(new MoveToTargetCoordsGoal(transform, new Vector3(Random.Range(-28.0f,28.0f), 1.0f, Random.Range(-5.0f, 5.0f))));
        }
    }

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

    public void ContestObjective()
    {
        //Debug.Log("!!!");
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "ContestObjectiveGoal");
        //Debug.Log("???");
        //Debug.Log(GoalBeingPursued);
        PerformSimulActions();
        ActionToExecute = GetRandomActionToAchiveSpecifiedGoal(GoalBeingPursued);
        //Debug.Log(ActionToExecute);
        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            GameObject objective = GameObject.FindWithTag("Objective");
            Vector3 ObjPos = objective.transform.position;
            GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x -3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
            GetActionsFromGoals();
        }
    }
}
