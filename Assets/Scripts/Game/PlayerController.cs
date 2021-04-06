using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float Speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(32.0f, 1.0f, 0.0f);
        transform.gameObject.tag = "RedTeam";
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0.0f, 0.0f, -1.0f * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0.0f, 0.0f, 1.0f * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(1.0f * Speed * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(-1.0f * Speed * Time.deltaTime, 0.0f, 0.0f);
        }
    }
}

