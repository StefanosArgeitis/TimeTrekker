using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Cut3 : MonoBehaviour
{
    public TextMeshProUGUI convo5;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("textEnd", 2f);
    }


    private void textEnd(){
        convo5.enabled = true;
        Invoke("textEnd_d", 11f);
    }

    private void textEnd_d(){
        convo5.enabled = false;
        Invoke("restartGame", 1.5f);
    }

    private void restartGame(){
        SceneManager.LoadScene("Mennnu 1");
    }
}
