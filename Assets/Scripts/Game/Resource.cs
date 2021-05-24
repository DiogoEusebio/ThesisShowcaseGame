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
    public void Consume()
    {
        //Do something

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
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    //Debug.Log("resource collision " + other);
                }
            }
        }
    }
}
