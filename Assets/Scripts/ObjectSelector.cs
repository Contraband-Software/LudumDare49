using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [Header("Important References")]
    public taskManager taskManager;
    public playerController pCon;
    public GameObject hand;

    [Header("Materials")]
    [SerializeField] private Material highlightMaterial;    //the material to be used as the indicator of selection
    [SerializeField] private Material transparentMaterial;

    [Header("Object Related")]
    [SerializeField] private GameObject selectedObject;                      //stores the gameobject that was most recently hit
    public List<Material> objectMaterial = new List<Material>();                       //stores the initial material of the gameobject most recently hit

    [Header("Slider Related")]
    public Slider slider;

    //optimization
    private int prevHitid = 0;                                                              //stores the unique ID of the last object the raycast hit
    Transform selection_prev;                                                               //stores the previous hits selection
    List<MeshRenderer> selectionRenderer_prev = new List<MeshRenderer>();                  //stores the previous hits selection renderer

    // Update is called once per frame
    void Update()
    {
        SelectInteractable();
        PickUpObject();
        InteractWithObject();
        InteractWithSlider();
    }

    //checks to see if looking at an interactable, and if so, will highlight it
    private void SelectInteractable()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));                           //sets the ray to be fired from the centre of the screen
        RaycastHit hit;                                                        
        if (Physics.Raycast(ray, out hit) && hit.distance <= 3f)                                        //fires the ray, if it hits, will go into the loop. If not, will deselect the selection if there is one
        {
            
            //INITIALISATION
            Transform selection;                                                                        //stores the selection and renderer of this current raycast attempt
            List<MeshRenderer> selectionRenderer = new List<MeshRenderer>();
            int thisHitid = hit.transform.gameObject.GetInstanceID();                                   //fetches the unique ID of the gameobject it hit
            
            //CHECK IF SAME OBJECT HIT
            if (prevHitid != thisHitid)                                                                 //if it hit a different gameObject than before, will get the objects
            {                                                                                           //transform and MeshRenderer comp
                selection = hit.transform;

                if(selection.transform.childCount > 0)
                {
                    for(int child = 0; child <= selection.transform.childCount -1; child++)                          //get all childrens mesh components
                    {
                        selectionRenderer.Add(selection.transform.GetChild(child).GetComponent<MeshRenderer>());
                    }
                }
                else
                {
                    selectionRenderer.Add(selection.transform.GetComponent<MeshRenderer>());
                }

                selection_prev = selection;
                selectionRenderer_prev = selectionRenderer;
            }
            else
            {                                                                                           //if the hit object is the same as last time, the program will use the previously set details
                selection = selection_prev;                                                             //this is done to make sure that GetComponent is not called every frame.
                selectionRenderer = selectionRenderer_prev;
            }

            if ((selection.gameObject.tag == "Interactable" || selection.gameObject.tag == "PickUpAble") && thisHitid != prevHitid)                //if the hit object is a different interactable from the previous,
            {                                                                                       //it will deselect the previous object and set this new one as the new current selection
                DeselectPreviousSelection(thisHitid);

                for(int mesh = 0; mesh <= selectionRenderer.Count -1; mesh++)
                {
                    selectedObject = selection.gameObject;
                    if (selection.transform.childCount > 0)
                    {
                        MeshRenderer thisRend = selectionRenderer[mesh];

                        if(thisRend != null)
                        {
                            Material[] mats = thisRend.materials;

                            Material objectMat = mats[mats.Length - 1];                                               //set the current initial object material to the material of the hit object
                            mats[mats.Length - 1] = highlightMaterial;                                                //changes the selected objects material to the highlighting material
                            thisRend.materials = mats;
                            objectMaterial.Add(objectMat);
                        }
                        
                    }
                    else
                    {
                        MeshRenderer thisRend = selectedObject.GetComponent<MeshRenderer>();

                        if(thisRend != null)
                        {
                            Material[] mats = thisRend.materials;

                            Material objectMat = mats[mats.Length - 1];                                               //set the current initial object material to the material of the hit object
                            mats[mats.Length - 1] = highlightMaterial;
                            thisRend.materials = mats;                                                                //changes the selected objects material to the highlighting material
                            objectMaterial.Add(objectMat);
                        }
                        
                    }
                    
                }
            }
            else
            {
                DeselectPreviousSelection(thisHitid);
            }
            prevHitid = thisHitid;                                                                      //once all logic has been done, it can set the previous object instance ID to this one, to be used
        }                                                                                               //in the next raycast.
        else
        {
            prevHitid = 0;                                                                              //deselects the previous selection if the raycast misses.
            DeselectPreviousSelection(1);                                                               //values are set to 0 and 1 so that the deselect condition is met (acts as override)
        }
    }

    //deselects the previously highlighted interactable
    private void DeselectPreviousSelection(int thisHitid)
    {
        
        if (selectedObject != null && (prevHitid != thisHitid))                                         //checks whether there is a previous object to deselect, also doesnt deselect if the object is the same 
        {                                                                                               // as the object from the last raycasthit (reduces GetComponent calls)
            if (selectedObject.transform.childCount > 0)
            {
                for(int child = 0; child <= selectedObject.transform.childCount - 1; child++)
                {

                    MeshRenderer thisRend = selectedObject.transform.GetChild(child).GetComponent<MeshRenderer>();
                    if(thisRend != null)
                    {
                        Material[] mats = thisRend.materials;
                        mats[mats.Length - 1] = objectMaterial[child];
                        thisRend.materials = mats;
                    }
                }
            }
            else
            {
                //selectedObject.GetComponent<MeshRenderer>().material = objectMaterial[0];

                MeshRenderer thisRend = selectedObject.GetComponent<MeshRenderer>();

                if(thisRend != null)
                {
                    Material[] mats = thisRend.materials;
                    mats[mats.Length - 1] = objectMaterial[0];
                    thisRend.materials = mats;
                }
            }

            selectedObject = null;
            objectMaterial = new List<Material>();
        }                                                                                               
    }                                                                                                   
                                                                                                        
                                                                                                        
    private void DeselectOverride(GameObject clonedObject)
    {
        if (clonedObject.transform.childCount > 0)
        {
            for (int child = 0; child <= clonedObject.transform.childCount - 1; child++)
            {

                MeshRenderer thisRend = clonedObject.transform.GetChild(child).GetComponent<MeshRenderer>();

                if(thisRend != null)
                {
                    Material[] mats = thisRend.materials;
                    mats[mats.Length - 1] = transparentMaterial;
                    thisRend.materials = mats;
                }
            }
        }
        else
        {
            //clonedObject.GetComponent<MeshRenderer>().material = objectMaterial[0];

            MeshRenderer thisRend = clonedObject.GetComponent<MeshRenderer>();

            if(thisRend != null)
            {
                Material[] mats = thisRend.materials;
                mats[mats.Length - 1] = transparentMaterial;
                thisRend.materials = mats;
            }
        }

    }

    private void PickUpObject()
    {
        if(taskManager.holding == "" && Input.GetKeyDown(KeyCode.E))
        {
            if(selectedObject != null && selectedObject.tag == "PickUpAble")
            {
                
                InteractableObj intObj = selectedObject.GetComponent<InteractableObj>();
                

                if(intObj.objectName == "Rod")
                {
                    print("picking up the red rod");
                    selectedObject.GetComponent<MeshRenderer>().material = taskManager.glowRed;
                    selectedObject.tag = "Untagged";
                    GameObject clonedSelected = Instantiate(taskManager.rodObject, hand.transform);
                    selectedObject.SetActive(false);
                    DeselectOverride(clonedSelected);
                    clonedSelected.GetComponent<MeshRenderer>().material = taskManager.glowRed;
                    clonedSelected.transform.localPosition = Vector3.zero;
                }
                else if(intObj.objectName == "RodGreen")
                {
                    GameObject clonedSelected = Instantiate(selectedObject, hand.transform);
                    DeselectOverride(clonedSelected);
                    clonedSelected.transform.localPosition = Vector3.zero;
                }
                else
                {
                    if(taskManager.holding == "")
                    {
                        print("picking up like normal");
                        GameObject clonedSelected = Instantiate(selectedObject, hand.transform);
                        DeselectOverride(clonedSelected);
                        clonedSelected.transform.localPosition = Vector3.zero;

                        Rigidbody rb = clonedSelected.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true;
                            rb.detectCollisions = false;
                        }

                        GameObject.Destroy(selectedObject);
                    }
                    

                }
                taskManager.holding = intObj.objectName;
            }
        }
    }
    
    private void InteractWithObject()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedObject != null && selectedObject.tag == "Interactable" && selectedObject.name != "Slider")
            {
                print(selectedObject.name);
                InteractableObj intObj = selectedObject.GetComponent<InteractableObj>();
                //Debug.Log(intObj);
                intObj.ObjectAction();
            }
            if(selectedObject != null && selectedObject.tag == "Interactable" && selectedObject.name == "TrashBin" && taskManager.holding != "Rod" && taskManager.holding != "RodGreen" && selectedObject.name != "Slider")
            {
                taskManager.holding = "";
                GameObject.Destroy(hand.gameObject.transform.GetChild(0).gameObject);
            }
        }
    }

    private void InteractWithSlider()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && selectedObject != null && selectedObject.name == "Slider" && taskManager.holding == "")
        {
            print(taskManager.holding);
            DeselectOverride(selectedObject);
            slider.OnInteract();
        }
    }
}

