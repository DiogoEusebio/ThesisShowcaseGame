using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour
{
    private Vector3 BlueTeamPoint;
    private Vector3 RedTeamPoint;
    private Vector3 GreenTeamPoint;
    private bool RedTeamContesting = false;
    private bool BlueTeamContesting = false;
    private bool GreenTeamContesting = false;
    private int RedTeamScore, BlueTeamScore, GreenTeamScore = 0;
    void Start()
    {
        BlueTeamPoint = new Vector3(-12.87f, 1.0f, 14.77f);
        RedTeamPoint = new Vector3(12.87f, 1.0f, 14.77f);
        GreenTeamPoint = new Vector3(0.0f, 1.0f, -7.0f);
        transform.position = new Vector3(0.0f, 1.0f, 7.0f);
    }

    void OnTriggerStay(Collider other)
    {
        Vector3 direction;
        if(other.gameObject.tag == "RedTeam")
        {
            RedTeamContesting = true;
            if (!BlueTeamContesting && !GreenTeamContesting)
            {
                direction = Vector3.Normalize(RedTeamPoint - this.transform.position);
                transform.position += direction * Time.deltaTime;
            }
        }
        if (other.gameObject.tag == "BlueTeam")
        {
            BlueTeamContesting = true;
            if (!RedTeamContesting && !GreenTeamContesting)
            {
                direction = Vector3.Normalize(BlueTeamPoint - this.transform.position);
                transform.position += direction * Time.deltaTime;
            }
        }
        if (other.gameObject.tag == "GreenTeam")
        {
            GreenTeamContesting = true;
            if (!RedTeamContesting && !BlueTeamContesting)
            {
                direction = Vector3.Normalize(GreenTeamPoint - this.transform.position);
                transform.position += direction * Time.deltaTime;
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
        if (other.gameObject.tag == "GreenTeam")
        {
            GreenTeamContesting = false;
        }
    }
    private void Update()
    {
        if (Vector3.Distance(this.transform.position, RedTeamPoint) < 0.3)
        {
            RedTeamScore++;
            Debug.Log("Red Team Scores! Score:" + RedTeamScore);
            transform.position = new Vector3(0.0f, 1.0f, 7.0f);
        }
        if (Vector3.Distance(this.transform.position, BlueTeamPoint) < 0.3)
        {
            BlueTeamScore++;
            Debug.Log("Blue Team Scores! Score:" + BlueTeamScore);
            transform.position = new Vector3(0.0f, 1.0f, 7.0f);
        }
        if (Vector3.Distance(this.transform.position, GreenTeamPoint) < 0.3)
        {
            GreenTeamScore++;
            Debug.Log("Green Team Scores! Score:" + GreenTeamScore);
            transform.position = new Vector3(0.0f, 1.0f, 7.0f);
        }
        }
}
