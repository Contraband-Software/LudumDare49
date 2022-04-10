using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public CharacterController controller;
    public cameraLook cameraLook;
    public float speed;
    public GameObject holdObject;
    public float bobSpeed;
    public float bobMagnitude;
    public AudioSource crashsound;
    public AudioSource crashsound1;
    public AudioSource crashsound2;
    public AudioSource crashsound3;
    public AudioSource crashsound4;
    public AudioSource crashsound5;
    bool as1p = false;
    bool as2p = false;
    bool as3p = false;
    bool as4p = false;
    bool as5p = false;
    bool as6p = false;
    int asindex = 0;
    //public AudioSource footStepSource;
    float cameraBob;
    float lastBob;
    float vY = 0;
    float time;
    Collider col;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null)// && !body.isKinematic)
        {
            body.velocity += hit.controller.velocity;
        }
        if (hit.gameObject.tag == "collide" && UnityEngine.Random.Range(0,100)>70)
        {
            if (asindex == 0 && !as1p)
            {
                crashsound.Play();
                as1p = true;
            }
            if (asindex == 1 && !as2p)
            {
                crashsound1.Play();
                as2p = true;
            }
            if (asindex == 2 && !as3p)
            {
                crashsound2.Play();
                as3p = true;
            }
            if (asindex == 3 && !as4p)
            {
                crashsound3.Play();
                as4p = true;
            }
            if (asindex == 4 && !as5p)
            {
                crashsound4.Play();
                as5p = true;
            }
            if (asindex == 5 && !as6p)
            {
                crashsound5.Play();
                as6p = true;
            }
            asindex++;
            if (asindex == 5)
            {
                asindex = 0;
            }
        }
        //Debug.Log(hit.gameObject.tag);
    }

    void Start()
    {
        //footStepSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Mathf.Floor(time) % 1.5 == 0)
        {
            as1p = false;
            as2p = false;
            as3p = false;
            as4p = false;
            as5p = false;
            as6p = false;
        }
        vY += -9.81f * Time.deltaTime;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x != 0 || z != 0){
            /*
            if (!footStepSource.isPlaying)
            {
                footStepSource.Play();
            }
            */
            cameraLook.cameraBob += 1.0f*Time.deltaTime;
            cameraBob += 1.0f * Time.deltaTime * bobSpeed;
        }
        else
        {
            //footStepSource.Stop();
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move*speed*Time.deltaTime + transform.up * vY*Time.deltaTime);
        if (holdObject != null)
        {
            holdObject.transform.position -= new Vector3(0f, (float)Math.Sin(lastBob * bobSpeed) * bobMagnitude, 0f);
            holdObject.transform.position += new Vector3(0f, (float)Math.Sin(cameraBob * bobSpeed) * bobMagnitude, 0f);
        }
        lastBob = cameraBob;
    }

    private void OnCollisionEnter(Collision col)
    {
        vY = 0;
    }
}
