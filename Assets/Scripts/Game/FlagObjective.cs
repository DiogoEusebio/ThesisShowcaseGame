using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagObjective : MonoBehaviour
{
    private bool captured = false;
    private Transform CaptureAgent = null;
    void Start()
    {
        Debug.Log("FLAG EXISTS");
    }

    // Update is called once per frame
    void Update()
    {
        if (captured)
        {
            //update flag pos
        }
    }

    public bool IsCaptured() { return captured; }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Flag Collision");
        if (other.TryGetComponent(out Agent Agent))
        {
            if (Agent.GetAgentType() == Agent.AgentType.Tetrahedron)
            {
                if (!IsCaptured())
                {
                    //assign agent to captureAgent
                    captured = true;
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    Debug.Log("Flag collision " + other);
                }
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("STAY");
    }
}
