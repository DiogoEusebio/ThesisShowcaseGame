using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal 
{   
    public enum State
    {
        notBeingFollowed,
        BeingFollowed,
        Achieved,
        notAchieved
    }
    protected State GoalState = State.notBeingFollowed;
    protected string name;
    protected List<Action> ActionList = new List<Action>();
    //List of Actions that can be used to achive this goal, which must be created/stored/filled in the Goal constructor

    public List<Action> GetActionsFromGoal()
    {
        return ActionList;
    }
    public State GetGoalState()
    {
        return GoalState;
    }
    public void SetGoalState(State s)
    {
        GoalState = s;
    }
    public string GetName()
    {
        return name;
    }
}
