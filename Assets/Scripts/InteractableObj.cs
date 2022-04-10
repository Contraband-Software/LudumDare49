using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InteractableObj : MonoBehaviour
{
    [Header("General Info")]
    public string objectName;
    public bool interacted = false;
    public GameController gameController;

    [Header("Coffee Maker Stuff")]
    public GameObject coffeeCup;
    public GameObject coffeCupSpawn;
    public bool coffeeMade = false;
    public float timeToMake = 10f;
    public float timeLeft;
    public Image progressBar_Coffee;

    [Header("Breaker Stuff")]
    public bool breakerTurnedOn = true;
    public string state = "down";
    public GameObject lights;
    public GameObject switchObject;

    [Header("Pump Lever Stuff")]
    public GameObject pump;
    public GameObject pumpLever;
    public bool pumpSwitchedOn = true;
    public string pumpLeverState = "down";

    [Header("rock paper scisors stuff")]
    public rpcmanager rpc;

    public void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Game")
        {
            gameController = GameObject.Find("Player").GetComponent<GameController>();
        }
        
    }
    private void Update()
    {
        ObjectUpdate();
    }

    public void ObjectAction()
    {
        if(objectName == "CoffeeMaker")
        {
            if (!interacted)                                                        //ON BEGIN of the interaction
            {
                print("Time to make some cofee");
                interacted = true;
                coffeeMade = false;
                timeLeft = timeToMake;
                gameObject.tag = "Untagged";
            }
            if (interacted && coffeeMade)                                            //recall, to finish the interaction after a process
            {
                GameObject spawnedCoffeeCup = GameObject.Instantiate(coffeeCup, coffeCupSpawn.transform);
                spawnedCoffeeCup.transform.localPosition = Vector3.zero;
                spawnedCoffeeCup.tag = "PickUpAble";
            }
        }

        if(objectName == "BreakerBox")
        {
            if (!interacted)
            {
                print("switching breaker box");
                if(state == "down" && !interacted)
                {
                    //put breaking up

                    Material lightMat = lights.GetComponent<MeshRenderer>().material;
                    int index = lightMat.shader.GetPropertyNameId(2);
                    
                    lightMat.SetFloat(index, 240f);

                    switchObject.transform.localEulerAngles = new Vector3(-176f, 0f, 0f);
                    interacted = true;
                    breakerTurnedOn = false;
                    state = "up";
                }

                if(state == "up" && !interacted)
                {
                    //put breaker down

                    print("putting breaker down");

                    Material lightMat = lights.GetComponent<MeshRenderer>().material;
                    int index = lightMat.shader.GetPropertyNameId(2);

                    lightMat.SetFloat(index, 0f);

                    switchObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    interacted = true;
                    breakerTurnedOn = true;
                    state = "down";
                }
                interacted = false;
            }
        }
        if(objectName == "PumpLever")
        {
            if (!interacted)
            {
                if(pumpLeverState == "down" && !interacted)
                {
                    print("TURNING PUMP OFF");
                    Material[] mats = pump.GetComponent<MeshRenderer>().materials;
                    int index = mats[16].shader.GetPropertyNameId(2);
                    mats[16].SetFloat(index, 240f);

                    pumpLever.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    interacted = true;
                    pumpSwitchedOn = false;
                    pumpLeverState = "up";
                }
                if (pumpLeverState == "up" && !interacted)
                {
                    print("TURNING PUMP ONNNN");
                    Material[] mats = pump.GetComponent<MeshRenderer>().materials;
                    int index = mats[16].shader.GetPropertyNameId(2);
                    mats[16].SetFloat(index, 0f);

                    pumpLever.transform.localEulerAngles = new Vector3(-177f, 0f, 0f);
                    interacted = true;
                    pumpSwitchedOn = true;
                    pumpLeverState = "down";
                }
                interacted = false;
            }
        }

        if(objectName == "rpcb1")
        {
            rpc.b1Press();
        }
        if (objectName == "rpcb2")
        {
            rpc.b2Press();
        }
        if (objectName == "rpcb3")
        {
            rpc.b3Press();
        }
        if (objectName == "rpcb4")
        {
            rpc.b4Press();
        }
    }


    public void ObjectUpdate()                                                  //process 
    {
        if(objectName == "CoffeeMaker")
        {
            if (interacted)
            {

                //COUNTDOWN
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                }
                else
                {
                    timeLeft = 0;
                }

                //UPDATE PROGRESS BAR
                float a = timeToMake - timeLeft;
                float percentFinished = (a / timeToMake);
                progressBar_Coffee.fillAmount = percentFinished;

                //DECIDE WHETHER ITS FINISHED
                if (timeLeft == 0 && coffeeMade == false)
                {
                    coffeeMade = true;
                    ObjectAction();
                }

                //RESET
                if(coffeeMade && coffeCupSpawn.transform.childCount == 0)
                {
                    coffeeMade = false;
                    interacted = false;
                    gameObject.tag = "Interactable";
                    progressBar_Coffee.fillAmount = 0f;
                }
            }
        }
        if(objectName == "PumpLever")
        {
            if(gameController != null)
            {
                if (pumpSwitchedOn)
                {

                    gameController.pumpTurnedOff = false;
                }
                else
                {
                    gameController.pumpTurnedOff = true;
                }
            }
            
        }
    }


    public void PumpLeverSetOff()
    {
        Material[] mats = pump.GetComponent<MeshRenderer>().materials;
        int index = mats[16].shader.GetPropertyNameId(2);
        mats[16].SetFloat(index, 240f);

        pumpLever.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        interacted = false;
        pumpSwitchedOn = false;
        pumpLeverState = "up";
    }
}
