using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class reactorController : MonoBehaviour
{
    public Slider slider;
    public GameObject rods;
    public float reactorPower = 100;
    Vector3 rodStart;
    Vector3 targetPosition;
    bool isMeltdownTimer = false;
    bool isFireingTimer = false;
    public float MeltdownTimer = 0;
    public float FireingTimer = 0;
    public TextMeshProUGUI powerOut;
    public GameObject lowText;
    public GameObject highText;
    public endgame end;
    public AudioSource lowp;
    public AudioSource highp;
    public AudioSource playeraudio;
    bool canPlaylowp = true;
    bool canPlayhighp = true;
    // Start is called before the first frame update
    void Start()
    {
        rodStart = rods.transform.position;
        targetPosition = rodStart;
        lowText.SetActive(false);
        highText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playeraudio.isPlaying);
        powerOut.SetText(Mathf.Round(reactorPower).ToString());
        //Debug.Log(rodStart - new Vector3(0, slider.sliderValue * 10f, 0));
        targetPosition = rodStart - new Vector3(0, slider.sliderValue*3.7f, 0);

        rods.transform.position += (targetPosition - rods.transform.position)*Time.deltaTime;
        reactorPower = reactorPower+reactorPower*(slider.sliderValue -0.39244f)*0.15f*Time.deltaTime;
        if(slider.sliderValue < 0.5)
        {
            reactorPower -= 0.1f * Time.deltaTime;
        }
        if (slider.sliderValue > 0.5)
        {
            reactorPower += 0.1f * Time.deltaTime;
        }
        reactorPower = Mathf.Max(reactorPower, 0f);
        if (MeltdownTimer > 30)
        {
            meltdown();
        }
        if (FireingTimer > 30)
        {
            Fireing();
        }
        if (reactorPower<70)
        {
            FireingTimer += Time.deltaTime;
            lowText.SetActive(true);
            if (!lowp.isPlaying && canPlaylowp && !playeraudio.isPlaying)
            {
                lowp.Play();
                canPlaylowp = false;
            }
        }
        if (reactorPower > 130)
        {
            MeltdownTimer += Time.deltaTime;
            highText.SetActive(true);
            if (!highp.isPlaying && canPlayhighp && !playeraudio.isPlaying)
            {
                highp.Play();
                canPlayhighp = false;
            }
        }
        if(reactorPower>70 && reactorPower < 130)
        {
            MeltdownTimer = 0;
            FireingTimer = 0;
            lowText.SetActive(false);
            highText.SetActive(false);
            canPlaylowp = true;
            canPlayhighp = true;
        }
    }

    void Fireing()
    {
        end.fired(0);
    }
    void meltdown()
    {
        SceneManager.LoadScene("Meltdown");
    }
}

