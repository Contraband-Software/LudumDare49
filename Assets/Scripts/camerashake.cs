using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerashake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)) * 0.001f-new Vector3(0.05f, 0.05f, 0.05f);
        //Debug.Log(new Vector3(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)) * 0.001f);
    }
}
