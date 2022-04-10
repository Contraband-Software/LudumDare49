using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{

    [Header("Slider Value")]
    public float sliderValue = 0f;
    public bool interacted = false;
    public bool lockPlayer = false;

    private float localMin = 0.45f;
    private float localMax = -0.97f;
    private float fullDistance;

    [Header("References")]
    public GameObject sittingPoint;
    public GameObject sliderNotch;
    public Vector3 prePosition;
    public GameObject player;

    public void Start()
    {
        fullDistance = Mathf.Abs(localMax - localMin);
    }

    private void Update()
    {
        LockPlayer();
        SetPlayerAsLocked();
        UpdateSliderValue();
    }

    public void SetPlayerAsLocked()
    {
        if (lockPlayer && Input.GetKeyUp(KeyCode.E))
        {
            interacted = true;
            gameObject.tag = "Untagged";
        }
    }

    public void LockPlayer()
    {
        if (lockPlayer)
        {
            print("lock player");
            player.transform.position = new Vector3(sittingPoint.transform.position.x, player.transform.position.y, sittingPoint.transform.position.z);
            ControlSlider();
        }
        if(lockPlayer && interacted && Input.GetKeyDown(KeyCode.E))
        {
            print("unlocking player");
            player.transform.position = prePosition;
            gameObject.tag = "Interactable";
            lockPlayer = false;
            interacted = false;
        }
    }

    public void ControlSlider()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            //print("scrolling up");
            if(sliderValue < 1f)
            {
                sliderValue += 0.03f;
            }
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            //print("scrollin down");
            if(sliderValue > 0f)
            {
                sliderValue -= 0.03f;
            }
        }


    }

    public void UpdateSliderValue()
    {
        sliderValue = Mathf.Clamp(sliderValue, 0f, 1f);
        float xPosition = localMin - (sliderValue * fullDistance);

        sliderNotch.transform.localPosition = new Vector3(xPosition, sliderNotch.transform.localPosition.y, sliderNotch.transform.localPosition.z);
    }

    public void OnInteract()
    {
        print("clicked on the slider");
        if (!interacted)
        {
            if (!lockPlayer)
            {
                prePosition = player.transform.position;
                lockPlayer = true;
            }
        }
        
    }
}
