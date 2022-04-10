using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuaseGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        thisCanvas.enabled = false;
    }
    public Canvas thisCanvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            thisCanvas.enabled = true;
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1f;
        thisCanvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
}
