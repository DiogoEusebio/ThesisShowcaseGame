using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float camSpeed = 0.05f;
    private float camHeight = 25.0f;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0.0f, 25.0f, 0.0f);
        transform.LookAt(new Vector3(0.0f, -1.0f, -1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //playerCamera();
        freecamera();
    }
    void playerCamera()
    {
        transform.position = new Vector3(Player.transform.position.x, camHeight, Player.transform.position.z);
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            camHeight++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            camHeight--;
        }
    }
    void freecamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0.0f, 0.0f, -1.0f * camSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0.0f, 0.0f, 1.0f * camSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(1.0f * camSpeed, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(-1.0f * camSpeed, 0.0f, 0.0f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            camHeight++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            camHeight--;
        }
    }
}
