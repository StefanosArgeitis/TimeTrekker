using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Checpoints : MonoBehaviour
{
    public Respawn rp;
    public Transform player;
    public GameObject checkpointparticle;
    private bool checkpointBool = false;
    AudioSource music;
    public TextMeshProUGUI textCheckpoint;
    // Start is called before the first frame update
    void Start()
    {
        rp = FindObjectOfType<Respawn>();
        music = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!checkpointBool){

        Instantiate(checkpointparticle, player.transform.position, Quaternion.identity);
        //Debug.Log("NICE");
        rp.newSpawnPoint();
        music.Play();
        checkpointBool = true;
        textCheckpoint.enabled = true;
        Invoke("disableText", 2f);
        }
    }

    private void disableText(){
        textCheckpoint.enabled = false;
    }
}
