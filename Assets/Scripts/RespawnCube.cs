using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCube : MonoBehaviour
{
    public Respawn rp;

    private void OnTriggerEnter(Collider other) {

        if (other.tag.Equals("Player")){
            rp.p_respawn();
        }
    }

}
