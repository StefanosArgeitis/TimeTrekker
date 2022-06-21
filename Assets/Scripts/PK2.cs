using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PK2 : MonoBehaviour
{
    AudioSource music;
    void Start()
    {
        music = GetComponent<AudioSource>();
        Invoke("playAudio", 167f);
    }

    void playAudio(){
        music.Play();
    }
}
