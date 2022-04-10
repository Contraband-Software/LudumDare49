using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("AFFECTING GENERATOR")]
    public bool pumpTurnedOff = false; //IF TRUE, INCREASES POWER RATE
    public bool faultyRod = false; //IF TRUE, DECREASES POWER RATE

    [Header("WAYPOINTS")]
    public bool makeCoffeeWaypoint = false;
    public bool toiletPaperWaypoint = false;
    public bool pumpResetWaypoint = false;
    public bool rodReplacementWaypoint = false;



    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        makeCoffeeWaypoint = false;
        toiletPaperWaypoint = false;
        pumpResetWaypoint = false;
        rodReplacementWaypoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
