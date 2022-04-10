using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public AudioClip defeaultSound;
    public AudioClip volumeOneSound;
    public AudioSource player;
    public Collider point;
    int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        player.clip = defeaultSound;
        player.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "SoundVolume1")
        {
            state = 1;
            player.clip = volumeOneSound;
            player.Play();
        }
    }
    void OnTriggerExit(Collider col)
    {
        state = 0;
        player.clip = defeaultSound;
        player.Play();
    }
}
