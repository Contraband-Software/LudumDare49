using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointController : MonoBehaviour
{
    public GameObject playerObject;
    public string quest;

    GameController MainGameController;
    MeshRenderer ownMesh;

    // Start is called before the first frame update
    void Start()
    {
        MainGameController = playerObject.GetComponent<GameController>();
        ownMesh = gameObject.GetComponent<MeshRenderer>();
        if (quest != "")
        {
            ownMesh.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //find the vector pointing from our position to the target
        Vector3 direction = (playerObject.transform.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

        if (quest != "")
        {
            switch (quest)
            {
                case "Coffee":
                    if (MainGameController.makeCoffeeWaypoint != true)
                    {
                        ownMesh.enabled = false;
                    }
                    else
                    {
                        ownMesh.enabled = true;
                    }
                    break;
                case "Toilet":
                    if (MainGameController.toiletPaperWaypoint != true)
                    {
                        ownMesh.enabled = false;
                    }
                    else
                    {
                        ownMesh.enabled = true;
                    }
                    break;
                case "Pump":
                    if (MainGameController.pumpResetWaypoint != true)
                    {
                        ownMesh.enabled = false;
                    }
                    else
                    {
                        ownMesh.enabled = true;
                    }
                    break;
                case "Rod":
                    if (MainGameController.rodReplacementWaypoint != true)
                    {
                        ownMesh.enabled = false;
                    }
                    else
                    {
                        ownMesh.enabled = true;
                    }
                    break;
            }
        }
    }
}
