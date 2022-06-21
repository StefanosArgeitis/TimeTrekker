using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PK1 : MonoBehaviour
{
    AudioSource music;
    void Start()
    {
        music = GetComponent<AudioSource>();
        Invoke("playAudio", 56f);
    }

    void playAudio(){
        music.Play();
    }
}
