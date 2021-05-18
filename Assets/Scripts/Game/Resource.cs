using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello!");
    }

    // Update is called once per frame
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
        if (!IsCollected())
        {
            collected = true;
            transform.parent = other.transform;
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Debug.Log("resource collision " + other);
        }
    }
}
