using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float camSpeed = 0.05f;
    private float camHeight = 28.0f;
    private float camXaxis = 0.0f;
    private float camZaxis = 25.0f;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.LookAt(new Vector3(0.0f, -1.0f, -0.5f));
        transform.position = new Vector3(camXaxis, camHeight, camZaxis);
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
            camZaxis -= 1 * camSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            camZaxis += 1 * camSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            camXaxis += 1 * camSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            camXaxis -= 1 * camSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            camHeight++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            camHeight--;
        }
        transform.position = new Vector3(camXaxis, camHeight, camZaxis);
    }
}
