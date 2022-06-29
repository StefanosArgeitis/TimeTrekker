using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Cut1 : MonoBehaviour
{
    public TextMeshProUGUI convo1;
    public TextMeshProUGUI convo2;
    [SerializeField] private string loadLevel;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("fText", 2f);
    }

    private void fText(){
        convo1.enabled = true;
        Invoke("fText_d", 22f);
    }

    private void fText_d(){
        convo1.enabled = false;
        Invoke("sText", 0.5f);
    }
    
    private void sText(){
        convo2.enabled = true;
        Invoke("sText_d", 20f);
    }

    private void sText_d(){
        convo2.enabled = false;
        Invoke("next", 2f);
    }

    private void next(){
        SceneManager.LoadScene(loadLevel);
    }
}
