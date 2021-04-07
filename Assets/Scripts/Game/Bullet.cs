using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float Speed = 30.0f;
    private Rigidbody Rb;
    private Vector3 Direction;
    // Start is called before the first frame update

    void Start()
    {
        Rb = this.GetComponent<Rigidbody>();
        transform.localScale = new Vector3(0.002f, 0.01f, 0.002f); //hack becuase the cone mesh (parent) was imported with wrong dimensions
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
        Debug.Log("bullet collision");
        if(other.GetComponent<Agent>().GetAgentType() != Agent.AgentType.Cube)
        {
            Debug.Log(other.GetComponent<Agent>().GetAgentType());
            if (transform.parent.tag == "RedTeam" && other.gameObject.tag == "BlueTeam" || transform.parent.tag == "BlueTeam" && other.gameObject.tag == "RedTeam")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
