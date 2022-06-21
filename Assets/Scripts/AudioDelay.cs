using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDelay : MonoBehaviour
{
    AudioSource music;
    void Start()
    {
        music = GetComponent<AudioSource>();
        Invoke("playAudio", 3f);
    }

    void playAudio(){
        music.Play();
    }
}
