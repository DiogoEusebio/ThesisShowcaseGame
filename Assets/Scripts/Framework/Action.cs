using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected float movementSpeed = 5.0f;
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

    //------ virtual/abstract methods to avoid casting to subclass ---- //
    public virtual bool GetIsOnCoolDown() { return false; }
    public virtual void UpdateDirection() { /*do nothing*/ }
    public virtual void UpdateCooldown() { /*do nothing*/ }
    public virtual Vector3 GetClosestResorcePosition() { Vector3 err = new Vector3(1000f, 1000f, 1000f); return err; }
    public virtual Vector3 GetClosestFlagPosition() { Vector3 err = new Vector3(1000f, 1000f, 1000f); return err; }
    public virtual void SetCapturedFlag(Transform Flag) { /*do nothing*/ }
    public virtual void DropFlag() { /*do nothing*/ }
    public virtual void SetExecuted(bool val) { /*do nothing*/ }
    public virtual Transform GetTargetAgent() { /*do nothing here*/ Debug.Log("implement concrete method before calling");  return null; }
}
