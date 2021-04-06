using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour
{
    private bool RedTeamContesting = false;
    private bool BlueTeamContesting = false;
    void Start()
    {
        transform.position = new Vector3(0.0f, 1.5f, 0.0f);
    }

    void OnTriggerStay(Collider other)
    {   
    
        if(other.gameObject.tag == "RedTeam")
        {
            RedTeamContesting = true;
            if (!BlueTeamContesting)
            {
                transform.position += new Vector3(-1.0f, 0.0f, 0.0f) * Time.deltaTime;
            }
        }
        if (other.gameObject.tag == "BlueTeam")
        {
            BlueTeamContesting = true;
            if (!RedTeamContesting)
            {
                transform.position += new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RedTeam")
        {
            RedTeamContesting = false;
        }
        if (other.gameObject.tag == "BlueTeam")
        {
            BlueTeamContesting = false;
        }
    }
    private void Update()
    {
        if (transform.position.x <= -17.5f || transform.position.x >= 17.5)
        {
            transform.position = new Vector3(0.0f, 1.5f, 0.0f);
        }
    }
}
