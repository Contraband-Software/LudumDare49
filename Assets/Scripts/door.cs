using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool isOpen = false;
    float rot = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            rot += 30.0f * Time.deltaTime;
        }
        transform.localRotation = Quaternion.Euler(-90, Mathf.Min(rot, 90f), 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isOpen = true;
        }
    }
}
