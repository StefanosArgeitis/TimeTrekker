using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Cut2 : MonoBehaviour
{
    public TextMeshProUGUI convo3;
    public TextMeshProUGUI convo4;
    [SerializeField] private string loadLevel;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("fText", 2f);
    }

    private void fText(){
        convo3.enabled = true;
        Invoke("fText_d", 24f);
    }

    private void fText_d(){
        convo3.enabled = false;
        Invoke("sText", 0.5f);
    }
    
    private void sText(){
        convo4.enabled = true;
        Invoke("sText_d", 30f);
    }

    private void sText_d(){
        convo4.enabled = false;
        Invoke("next", 2f);
    }

    private void next(){
        SceneManager.LoadScene(loadLevel);
    }
}
