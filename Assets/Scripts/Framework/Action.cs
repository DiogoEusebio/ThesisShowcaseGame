using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public enum State
    {
        NotBeingExecuted,
        BeingExecuted,
        Executed
    }
    public State ActionState = State.NotBeingExecuted;

    protected string name;
    public string GetName()
    {
        return name;
    }
    public abstract State Perform();

    //------ virtual methods to avoid casting to subclass ---- //
    public virtual bool GetIsOnCoolDown() { return false; }
    public virtual void UpdateDirection() { /*do nothing*/ }
    public virtual void UpdateCooldown() { /*do nothing*/ }
}
