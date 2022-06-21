using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string loadLevel;

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("hoihohoi");
        SceneManager.LoadScene(loadLevel);

    }
}
