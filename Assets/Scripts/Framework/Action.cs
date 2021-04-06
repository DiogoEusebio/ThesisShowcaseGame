using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public enum State
    {
        notBeingExecuted,
        BeingExecuted,
        Executed
    }
    public State ActionState = State.notBeingExecuted;

    protected string name;
    public string GetName()
    {
        return name;
    }
    public abstract State Perform();
}
