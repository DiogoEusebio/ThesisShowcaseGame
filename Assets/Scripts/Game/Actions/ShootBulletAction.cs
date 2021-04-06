using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletAction : Action
{   
    public GameObject bulletPrefab;
    private Transform agentTransform;
    public ShootBulletAction(Transform AgentTransform)
    {
        agentTransform = AgentTransform;
        bulletPrefab = (GameObject)Resources.Load("Prefabs/Bullet", typeof(GameObject));
    }
    public override State Perform()
    {
        GameObject b = GameObject.Instantiate(bulletPrefab, agentTransform.position, Quaternion.LookRotation(agentTransform.forward) * new Quaternion(0.5f, 0.5f, 0.5f, 0.5f), agentTransform);
        b.GetComponent<Bullet>().setDirection(agentTransform.forward);
        ActionState = State.Executed;
        return ActionState;
        
    }
}
