using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Task                                       //base task class, shit every type will have
{
    protected string Name;                              //may also need { get; set; } if there are problems, but there dont seem to be
    protected string Description;                       //may also need { get; set; } if there are problems, but there dont seem to be

    //constructors
    public Task()
    {
        Name = "Unnamed Task";
        Description = "";
    }
    public Task(string name, string description)
    {
        this.Name = name;
        this.Description = description;
    }

    //methods
    public string ShowInfo()
    {
        return $"{this.Name}: {this.Description}";
    }
}


public class MakeCoffee : Task
{
    public taskManager taskManager;
    private float timeLeft;
    private GameObject coffeeCup;
    private GameObject deliveryTray;
    private float completionDistance = 2f;
    float delayLeft = 2f;
    private TextMeshProUGUI timeLeftText;
    private bool taskFailed = false;
    private bool audioPlayed = false;

    public MakeCoffee(taskManager taskManager, GameObject coffeeCup, GameObject deliveryTray, TextMeshProUGUI timeLeftText)
    {
        this.timeLeft = 25f;
        this.delayLeft = 2f;
        this.taskManager = taskManager;
        this.coffeeCup = coffeeCup;
        this.deliveryTray = deliveryTray;
        this.completionDistance = 2f;
        this.timeLeftText = timeLeftText;
        this.taskFailed = false;
        this.audioPlayed = false;
    }

    public void ActiveTask(Vector3 playerPosition)
    {
        taskManager.coffee_taskIcon.SetActive(true);

        if (!audioPlayed)
        {
            taskManager.voiceLineManager.PlayVoiceLine("MakeCoffee");
            audioPlayed = true;
        }

        if (WinCondition(playerPosition))
        {
            Debug.Log("coffee has been delivered");
            taskManager.bossTASKLIST.Remove("MakeCoffee");
            taskManager.coffee_taskIcon.SetActive(false);
        }
        Countdown();
    }

    public bool WinCondition(Vector3 playerPosition)
    {
        float interactionDistance = 3f;
        Vector3 trayPos = this.deliveryTray.transform.position;

        float XZDistance = Mathf.Sqrt(((trayPos.x - playerPosition.x) * (trayPos.x - playerPosition.x)) + ((trayPos.y - playerPosition.y) * (trayPos.y - playerPosition.y)));

        if(XZDistance <= interactionDistance && taskManager.holding == "CoffeeCup" && Input.GetKeyDown(KeyCode.E))
        {
            taskManager.holding = "";
            GameObject.Destroy(taskManager.objSelector.hand.transform.GetChild(0).gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Countdown()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0;

            if (!taskFailed)
            {
                taskManager.tasksFailed++;
                Debug.Log("YOU FAILED TO DELIVER MY COFFEE!");
                taskFailed = true;
            }

            if(delayLeft > 0)
            {
                delayLeft -= Time.deltaTime;
                timeLeftText.text = "TASK FAILED!";
            }
            else
            {
                taskManager.bossTASKLIST.Remove("MakeCoffee");
                taskManager.coffee_taskIcon.SetActive(false);
            }

            
        }

        if (!taskFailed)
        {
            int roundedTime = (int)Mathf.Floor(timeLeft);
            int mins = roundedTime / 60;
            int seconds = roundedTime % 60;

            if (seconds > 9)
            {
                this.taskManager.coffee_timeLeft.text = "0" + mins.ToString() + ":" + seconds.ToString();
            }
            else
            {
                this.taskManager.coffee_timeLeft.text = "0" + mins.ToString() + ":0" + seconds.ToString();
            }
        }
    }
}

public class ToiletPaper
{
    private GameObject deliveryToilet;

    public taskManager taskManager;
    private float timeLeft;
    private float delayLeft = 2f;
    private float completionDistance = 2f;
    private bool taskFailed = false;
    private bool audioPlayed = false;
    private TextMeshProUGUI timeLeftText;

    public ToiletPaper(taskManager taskManager, GameObject deliveryToilet, TextMeshProUGUI timeLeftText)
    {
        this.taskManager = taskManager;
        this.deliveryToilet = deliveryToilet;
        this.timeLeft = 20f;
        this.delayLeft = 2f;
        this.completionDistance = 2f;
        this.taskFailed = false;
        this.audioPlayed = false;
        this.timeLeftText = timeLeftText;
    }

    public void ActiveTask(Vector3 playerPosition)
    {
        taskManager.tp_taskIcon.SetActive(true);

        if (!audioPlayed)
        {
            taskManager.voiceLineManager.PlayVoiceLine("ToiletPaper");
            audioPlayed = true;
        }

        if (WinCondition(playerPosition))
        {
            Debug.Log("Toilet paper has been replaced");
            taskManager.bossTASKLIST.Remove("ToiletPaper");
            taskManager.tp_taskIcon.SetActive(false);
        }
        Countdown();
    }

    public bool WinCondition(Vector3 playerPosition)
    {
        float interactionDistance = 3f;
        Vector3 toiletPos = this.deliveryToilet.transform.position;

        float XZDistance = Mathf.Sqrt(((toiletPos.x - playerPosition.x) * (toiletPos.x - playerPosition.x)) + ((toiletPos.y - playerPosition.y) * (toiletPos.y - playerPosition.y)));

        if (XZDistance <= interactionDistance && taskManager.holding == "ToiletPaper" && Input.GetKeyDown(KeyCode.E))
        {
            taskManager.holding = "";
            GameObject.Destroy(taskManager.objSelector.hand.transform.GetChild(0).gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Countdown()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0;

            if (!taskFailed)
            {
                taskManager.tasksFailed++;
                Debug.Log("YOU FAILED TO REPLACE THE TOILET PAPER!");
                taskFailed = true;
            }

            if (delayLeft > 0)
            {
                delayLeft -= Time.deltaTime;
                timeLeftText.text = "TASK FAILED!";
            }
            else
            {
                taskManager.bossTASKLIST.Remove("ToiletPaper");
                taskManager.tp_taskIcon.SetActive(false);
            }


        }

        if (!taskFailed)
        {
            int roundedTime = (int)Mathf.Floor(timeLeft);
            int mins = roundedTime / 60;
            int seconds = roundedTime % 60;

            if (seconds > 9)
            {
                this.taskManager.tp_timeLeft.text = "0" + mins.ToString() + ":" + seconds.ToString();
            }
            else
            {
                this.taskManager.tp_timeLeft.text = "0" + mins.ToString() + ":0" + seconds.ToString();
            }
        }
    }
}


public class ResetBreaker : Task
{
    public taskManager taskManager;
    private GameObject breaker;
    private InteractableObj breakerObj;
    private bool breakerFuckedUp = false;

    public ResetBreaker(taskManager taskManager, GameObject breaker)
    {
        this.taskManager = taskManager;
        this.breaker = breaker;
        this.breakerObj = breaker.GetComponent<InteractableObj>();
    }

    public void ActiveTask()
    {
        if (!breakerFuckedUp)
        {
            Debug.Log("Power is out!");
            breakerFuckedUp = true;
            breakerObj.breakerTurnedOn = false;
        }

        if (WinCondition())
        {
            Debug.Log("Power back on!");
            taskManager.reactorTASKLIST.Remove("ResetBreaker");
        }
    }

    public bool WinCondition()
    {
        if(breakerFuckedUp && breakerObj.breakerTurnedOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class PumpReset
{
    public taskManager taskManager;
    private float timeLeft;
    private bool audioPlayed = false;

    private bool pumpSetOff = false;
    private InteractableObj pumpLeverInteractable;

    public PumpReset(taskManager taskManager)
    {
        this.taskManager = taskManager;
        this.audioPlayed = false;
        this.pumpSetOff = false;
    }

    public void ActiveTask()
    {
        taskManager.pump_taskIcon.SetActive(true);

        if (!pumpSetOff)
        {
            pumpLeverInteractable = taskManager.pumpLever.GetComponent<InteractableObj>();
            pumpLeverInteractable.PumpLeverSetOff();

            pumpSetOff = true;
        }

        
        if (WinCondition())
        {
            Debug.Log("Pump up and running!");
            taskManager.reactorTASKLIST.Remove("PumpReset");
            taskManager.pump_taskIcon.SetActive(false);
        }
       
        
    }

    public bool WinCondition()
    {
        if (pumpLeverInteractable.pumpSwitchedOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class RodReplacement
{
    public taskManager taskManager;
    public bool brokenRodSelected = false;
    public GameObject randomRod = null;
    public Vector3 rodPosition;

    public RodReplacement(taskManager taskManager)
    {
        this.taskManager = taskManager;
        brokenRodSelected = false;
        randomRod = null;
        rodPosition = Vector3.zero;

        foreach(GameObject rod in taskManager.allRods)
        {
            rod.GetComponent<MeshRenderer>().material = taskManager.glowGreen;
        }
    }

    public void ActiveTask(Vector3 playerPosition)
    {
        taskManager.rod_taskIcon.SetActive(true);

        if (!brokenRodSelected)
        {
            SelectRandomRod();
            brokenRodSelected = true;
        }

        if (WinCondition(playerPosition))
        {
            Debug.Log("rod replaced, poggers");
            taskManager.reactorTASKLIST.Remove("RodReplacement");

            randomRod.SetActive(true);
            randomRod.GetComponent<MeshRenderer>().material = taskManager.glowGreen;
            taskManager.rod_taskIcon.SetActive(false);
        }
    }

    public void SelectRandomRod()
    {
        int rodIndex = Random.Range(0, 8);
        randomRod = taskManager.allRods[rodIndex];
        rodPosition = randomRod.gameObject.transform.position;
        Debug.Log(rodPosition);

        randomRod.tag = "PickUpAble";
        randomRod.GetComponent<MeshRenderer>().material = taskManager.glowRed;
    }

    public bool WinCondition(Vector3 playerPosition)
    {
        float interactionDistance = 1.9f;
        Vector3 rodPos = rodPosition;

        float XZDistance = Mathf.Sqrt(((rodPos.x - playerPosition.x) * (rodPos.x - playerPosition.x)) + ((rodPos.y - playerPosition.y) * (rodPos.y - playerPosition.y)));

        if (XZDistance <= interactionDistance && taskManager.holding == "RodGreen" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(XZDistance);
            taskManager.holding = "";
            GameObject.Destroy(taskManager.objSelector.hand.transform.GetChild(0).gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}


public class taskManager : MonoBehaviour
{
    public int tasksFailed = 0;
    [Header("Task Cycling")]
    public float taskSpawnDelay = 20f;
    private bool bossTasksStarted = false;
    private bool reactorTasksStarted = false;
    private bool resetBossTasks = false;
    private bool resetReactorTasks = false;

    private List<string> reactorTasks = new List<string>();
    private List<string> bossTasks = new List<string>();

    public List<string> reactorTASKLIST = new List<string>();
    public List<string> bossTASKLIST = new List<string>();

    public List<string> allowedReactorTasks = new List<string>();
    public List<string> allowedBossTasks = new List<string>();

    public taskManager selfReference;
    public ObjectSelector objSelector;
    public VoiceLine_Manager voiceLineManager;
    public GameController gameController;

    public string holding; 

    [Header("Coffee Task")]
    public GameObject coffeeCup;
    public GameObject deliveryTray;
    public GameObject coffee_taskIcon;
    public TextMeshProUGUI coffee_timeLeft;

    [Header("Breaker Tast")]
    public GameObject breaker;

    [Header("Toilet Paper Task")]
    public GameObject deliveryToilet;
    public GameObject tp_taskIcon;
    public TextMeshProUGUI tp_timeLeft;

    [Header("Pump Task")]
    public GameObject pumpLever;
    public GameObject pump_taskIcon;

    [Header("Rod Replacement")]
    public List<GameObject> allRods = new List<GameObject>();
    public GameObject rodObject;
    public GameObject rod_taskIcon;
    public Material glowGreen;
    public Material glowRed;
    public GameObject controlRodBin;

    
   

    private MakeCoffee makeCoffee;
    private ResetBreaker resetBreaker;
    private ToiletPaper toiletPaper;
    private PumpReset pumpReset;
    private RodReplacement rodReplacement;
   
    // Start is called before the first frame update
    void Start()
    {
        bossTasksStarted = false;
        reactorTasksStarted = false;

        coffee_taskIcon.SetActive(false);
        tp_taskIcon.SetActive(false);
        pump_taskIcon.SetActive(false);
        rod_taskIcon.SetActive(false);

        selfReference = gameObject.GetComponent<taskManager>();

        makeCoffee = new MakeCoffee(selfReference, coffeeCup, deliveryTray, coffee_timeLeft);
        resetBreaker = new ResetBreaker(selfReference, breaker);
        toiletPaper = new ToiletPaper(selfReference, deliveryToilet, tp_timeLeft);
        pumpReset = new PumpReset(selfReference);
        rodReplacement = new RodReplacement(selfReference);

        reactorTasks.Add("RodReplacement");
        reactorTasks.Add("PumpReset");

        bossTasks.Add("MakeCoffee");
        bossTasks.Add("ToiletPaper");

        allowedBossTasks = new List<string>(bossTasks);
        allowedReactorTasks = new List<string>(reactorTasks);
    }

    // Update is called once per frame
    void Update()
    {
        activeTasks();
        //ResetTasksOverride();
        CheckIfCanBinControlRod(gameObject.transform.position);

        CycleTasks();
        CheckIfNoMoreTasks();
    }

    public void CycleTasks()
    {
        if(Time.time > 15f) //do boss tasks
        {

            if (!bossTasksStarted)
            {
                StartCoroutine(spawnBossTask());
                bossTasksStarted = true;
            }

        }
        if(Time.time > 45f)// do reactor tasks
        {
            if (!reactorTasksStarted)
            {
                StartCoroutine(spawnReactorTask());
                reactorTasksStarted = true;
            }
        }
    }

    public void AddNewTask(string task)
    {
        if(task == "MakeCoffee")
        {
            makeCoffee = new MakeCoffee(selfReference, coffeeCup, deliveryTray, coffee_timeLeft);

        }
        if(task == "ToiletPaper")
        {
            toiletPaper = new ToiletPaper(selfReference, deliveryToilet, tp_timeLeft);
        }
        if(task == "PumpReset")
        {
            pumpReset = new PumpReset(selfReference);
        }
        if(task == "RodReplacement")
        {
            rodReplacement = new RodReplacement(selfReference);
        }
    }

    public void ResetAllowedBossTasks()
    {
        allowedBossTasks = new List<string>();
        foreach(string task in bossTasks)
        {
            allowedBossTasks.Add(task);
        }
    }
    public void ResetAllowedReactorTasks()
    {
        allowedReactorTasks = new List<string>();
        foreach (string task in reactorTasks)
        {
            allowedReactorTasks.Add(task);
        }
    }

    public void CheckIfNoMoreTasks()
    {
        if(allowedBossTasks.Count == 0 && bossTASKLIST.Count == 0)
        {
            if(resetBossTasks == false)
            {
                ResetAllowedBossTasks();
                resetBossTasks = true;
                
            }
            
            
        }
        if(allowedReactorTasks.Count == 0 && reactorTASKLIST.Count == 0)
        {
            if (resetReactorTasks == false)
            {
                ResetAllowedReactorTasks();
                resetReactorTasks = true;
                
            }
        }
    }

    public IEnumerator spawnBossTask()
    {
        while (true)
        {
            if(allowedBossTasks.Count != 0)
            {
                int ranTask = Random.Range(0, allowedBossTasks.Count);
                string chosenTask = allowedBossTasks[ranTask];

                AddNewTask(chosenTask);
                bossTASKLIST.Add(chosenTask);
                allowedBossTasks.Remove(allowedBossTasks[ranTask]);

                resetBossTasks = false;
            }

            yield return new WaitForSeconds(35f);
        }
        
    }

    public IEnumerator spawnReactorTask()
    {
        while (true)
        {
            if (allowedReactorTasks.Count != 0)
            {
                int ranTask = Random.Range(0, allowedReactorTasks.Count);
                string chosenTask = allowedReactorTasks[ranTask];

                AddNewTask(chosenTask);
                reactorTASKLIST.Add(chosenTask);
                allowedReactorTasks.Remove(allowedReactorTasks[ranTask]);

                resetReactorTasks = false;
            }
           

            yield return new WaitForSeconds(35f);
        }
    }


    public void ResetTasksOverride()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            makeCoffee = new MakeCoffee(selfReference, coffeeCup, deliveryTray, coffee_timeLeft);
            resetBreaker = new ResetBreaker(selfReference, breaker);
            toiletPaper = new ToiletPaper(selfReference, deliveryToilet, tp_timeLeft);
            pumpReset = new PumpReset(selfReference);
            rodReplacement = new RodReplacement(selfReference);

            reactorTASKLIST = new List<string>();
            bossTASKLIST = new List<string>();

            reactorTASKLIST.Add("RodReplacement");
            reactorTASKLIST.Add("PumpReset");

            bossTASKLIST.Add("MakeCoffee");
            bossTASKLIST.Add("ToiletPaper");
        }
    }

    public void activeTasks()
    {
        if (bossTASKLIST.Contains("MakeCoffee"))
        {
            makeCoffee.ActiveTask(gameObject.transform.position);
            gameController.makeCoffeeWaypoint = true;
        }
        else
        {
            gameController.makeCoffeeWaypoint = false;
        }
        if (reactorTASKLIST.Contains("ResetBreaker"))
        {
            resetBreaker.ActiveTask();
        }
        if (bossTASKLIST.Contains("ToiletPaper"))
        {
            toiletPaper.ActiveTask(gameObject.transform.position);
            gameController.toiletPaperWaypoint = true;
        }
        else
        {
            gameController.toiletPaperWaypoint = false;
        }
        if (reactorTASKLIST.Contains("PumpReset"))
        {
            pumpReset.ActiveTask();
            gameController.pumpTurnedOff = true;
            gameController.pumpResetWaypoint = true;
        }
        else
        {
            gameController.pumpTurnedOff = false;
            gameController.pumpResetWaypoint = false;
        }

        if (reactorTASKLIST.Contains("RodReplacement"))
        {
            rodReplacement.ActiveTask(gameObject.transform.position);
            gameController.faultyRod = true;
            gameController.rodReplacementWaypoint = true;
        }
        else
        {
            gameController.faultyRod = false;
            gameController.rodReplacementWaypoint = false;
        }
    }

    public void CheckIfCanBinControlRod(Vector3 playerPosition)
    {
        if(holding == "Rod" || holding == "RodGreen")
        {
            float interactionDistance = 6f;
            Vector3 rodBinPos = this.controlRodBin.transform.position;

            float XZDistance = Mathf.Sqrt(((rodBinPos.x - playerPosition.x) * (rodBinPos.x - playerPosition.x)) + ((rodBinPos.y - playerPosition.y) * (rodBinPos.y - playerPosition.y)));

            if (XZDistance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
            {
                holding = "";
                GameObject.Destroy(objSelector.hand.transform.GetChild(0).gameObject);
            }
        }
        
    }
}

