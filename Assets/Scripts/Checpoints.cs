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
    public TextMeshProUGUI textCheckpoint;
    // Start is called before the first frame update
    void Start()
    {
        rp = FindObjectOfType<Respawn>();
    }

    private void OnTriggerEnter(Collider other) {

        Instantiate(checkpointparticle, player.transform.position, Quaternion.identity);
        Debug.Log("NICE");
        rp.newSpawnPoint();

        textCheckpoint.enabled = true;
        Invoke("disableText", 2f);
    }

    private void disableText(){
        textCheckpoint.enabled = false;
    }
}
