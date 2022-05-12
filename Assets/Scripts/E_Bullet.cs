using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Bullet : MonoBehaviour
{
    public float DecayTime;

    private void Update() {
        Destroy (gameObject, DecayTime);
    }
    


}
