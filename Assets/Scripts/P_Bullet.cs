using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision) {

        if(collision.transform.tag == "Enemy"){

            Destroy(collision.gameObject);
            
        }

        Destroy(gameObject);
    }
}
