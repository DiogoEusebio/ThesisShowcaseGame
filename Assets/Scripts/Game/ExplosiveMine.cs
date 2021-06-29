using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : MonoBehaviour
{
    private Rigidbody Rb;
    private float MineDamage = 50.0f;
    private Transform parent;
    // Start is called before the first frame update
    public void setParent(Transform agent)
    {
        parent = agent;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Mine collision "+ other);
        if (other.TryGetComponent(out Agent otherAgent))
        {
            //change to get enemies from parent enemy list from role

            /*parent.gameObject.tag == "RedTeam" && other.gameObject.tag == "BlueTeam" || parent.gameObject.tag == "RedTeam" && other.gameObject.tag == "GreenTeam" ||
            parent.gameObject.tag == "BlueTeam" && other.gameObject.tag == "RedTeam" || parent.gameObject.tag == "BlueTeam" && other.gameObject.tag == "GreenTeam" ||
            parent.gameObject.tag == "GreenTeam" && other.gameObject.tag == "RedTeam" || parent.gameObject.tag == "GreenTeam" && other.gameObject.tag == "BlueTeam"*/
            if (parent.GetComponent<Agent>().GetRoleList().Find((role) => role.GetName() == "CompetitorRole").GetTargetAgents().Contains(other.gameObject))
            {
                otherAgent.LogAgentActionResult("Stepped on mine");
                otherAgent.TakeDamage(MineDamage);
                Destroy(this.gameObject);
            }
        }
    }
}
