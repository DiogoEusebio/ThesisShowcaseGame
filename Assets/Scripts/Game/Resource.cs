using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool collected = false;
    void Start()
    {
    }

    void Update()
    {

    }
    public bool IsCollected()
    {
        return collected;
    }

    public void ConsumedBy(Transform Agent)
    {
        //Do something
        Debug.Log(Agent + " Consumed a resource");
        Agent.GetComponent<Agent>().ConsumeResourceToHeal();

        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Agent Agent))
        {
            if (Agent.GetAgentType() == Agent.AgentType.Tetrahedron)
            {
                if (!IsCollected())
                {
                    collected = true;
                    ResourceManager RM = transform.parent.GetComponent<ResourceManager>();
                    RM.UpdateCooldownFlag(this.transform.position);
                    transform.parent = other.transform;
                    //consider taking into consideration case when there is a collision but agent is not trying to collect item
                    transform.parent.GetComponent<Agent>().GetActionList().Find((action) => action.GetName() == "CollectResourceAction").SetExecuted(true);
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    //Debug.Log("resource collision " + other);
                }
            }
        }
    }
}
