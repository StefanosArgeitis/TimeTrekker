using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checpoints : MonoBehaviour
{
    public Respawn rp;
    public Transform player;
    public GameObject checkpointparticle;
    // Start is called before the first frame update
    void Start()
    {
        rp = FindObjectOfType<Respawn>();
    }

    private void OnTriggerEnter(Collider other) {

        Instantiate(checkpointparticle, player.transform.position, Quaternion.identity);
        rp.newSpawnPoint();
    }
}
