using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float Speed = 30.0f;
    private Rigidbody Rb;
    private Vector3 Direction;
    private Transform parent;
    // Start is called before the first frame update

    void Start()
    {
        Rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direction * Speed * Time.deltaTime;
        if(transform.position.x > 100 || transform.position.x < -100 || transform.position.z > 100 || transform.position.z < -100)
        {
            Destroy(this.gameObject);
        }
    }
    public void setDirection(Vector3 direction)
    {
        Direction = direction;
    }
    public void setParent(Transform agent)
    {
        parent = agent;
    }

    void OnDrawGizmos()
    {
        Color color;
        color = Color.green;
        DrawHelperAtCenter(this.transform.up, color, 2f);
    }
    private void DrawHelperAtCenter(Vector3 direction, Color color, float lenght)
    {
        Gizmos.color = color;
        Vector3 destination = transform.position + direction * lenght;
        Gizmos.DrawLine(transform.position, destination);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("bullet collision "+ other);
        if(other.TryGetComponent(out Agent otherAgent))
        {
            if(otherAgent.GetAgentType() != Agent.AgentType.Cube)
            {
                if (parent.gameObject.tag == "RedTeam" && other.gameObject.tag == "BlueTeam" || parent.gameObject.tag == "BlueTeam" && other.gameObject.tag == "RedTeam")
                {
                    otherAgent.TakeDamage(50.0f);
                    Destroy(this.gameObject);
                }
            }
        }  
    }
}
