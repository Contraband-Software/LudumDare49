using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class menuecammove : MonoBehaviour
{
    float Offset;
    public float maxOffset;
    float xRot;
    float yRot;
    float dxRot;
    float dyRot;
    Vector3 sp;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        sp = transform.position;
        button.onClick.AddListener(click);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        float mouseX = Input.GetAxis("Mouse X")  * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")  * Time.deltaTime;
        dxRot -= mouseY;
        dyRot += mouseX;
        xRot += (dxRot - xRot);
        yRot += (dyRot - yRot);
        transform.localRotation = UnityEngine.Quaternion.Euler(xRot, yRot+180, 0);
        Offset += Time.deltaTime*(maxOffset-Offset)*Time.deltaTime*6.0f;
        Offset = Mathf.Min(Offset, maxOffset);
        transform.position = sp - new Vector3(0, 0, Offset);

    }
    public void click()
    {
        SceneManager.LoadScene("game");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
