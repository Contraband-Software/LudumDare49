using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endgame : MonoBehaviour
{
    public string firestring;
    public taskManager taskm;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(taskm.tasksFailed > 1)
        {
            SceneManager.LoadScene("fired");
        }
    }

    public void fired(int firetype)
    {
        if (firetype == 0)
        {
            firestring = "you failed to keep the reactor at a high enough power.";
        }
        SceneManager.LoadScene("fired");
    }

}
