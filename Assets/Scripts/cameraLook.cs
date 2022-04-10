using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class cameraLook : MonoBehaviour
{
    public float sensitivity;
    public GameObject player;
    float xRotation = 0f;
    public float cameraBob;
    public float bobSpeed;
    public float bobMagnitude;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.locked;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")*sensitivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")*sensitivity*Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = UnityEngine.Quaternion.Euler(xRotation,0,0);
        transform.position = new Vector3(player.transform.position.x, (float)Math.Sin(cameraBob * bobSpeed) * bobMagnitude+player.transform.position.y+0.7f, player.transform.position.z);
        player.transform.Rotate(UnityEngine.Vector3.up * mouseX);
    }
}
