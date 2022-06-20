using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Respawn : MonoBehaviour
{

    private bool respawning;
    private Vector3 respawnPoint;
    public Transform player;
    public Image blackScreen;
    private bool fadeIn;
    private bool fadeOut;

    public float respawntime;
    public float fadeSpeed;
    public float fadetime;
    //public Text text;
    //public TMP_Text text;
    public TextMeshProUGUI text;
    private bool checkpointTxt;
    private void Start() {

        respawnPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update(){
        
        if(fadeIn){

            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 1f){
                fadeIn = false;
            }
        }
        
        if(fadeOut){

            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f){
                fadeOut = false;
            }
        }

    }

    public void p_respawn(){

        if (!respawning){
            StartCoroutine("RespawnI");
        }
    }

    public IEnumerator RespawnI(){

        respawning = true;

        yield return new WaitForSeconds(respawntime);

        fadeIn = true;

        yield return new WaitForSeconds(fadetime);

        fadeOut = true;
        respawning = false;
        player.transform.position = respawnPoint;


    }

    public void newSpawnPoint(){
        respawnPoint = player.transform.position;
    }
}
