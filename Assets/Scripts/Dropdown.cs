using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dropdown : MonoBehaviour
{
    public bool c_Day;
    public bool c_Month;
    public bool submitted;
    //public TextMeshProUGUI output;
    public TextMeshProUGUI correct;
    public TextMeshProUGUI wrong;
    public TMPro.TMP_Dropdown dropdown_d;
    public TMPro.TMP_Dropdown dropdown_m;
    [SerializeField] private string loadLevel;

    private void Start() {
        //Debug.Log("NICEssssss");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void inputDataDay(){

        if (dropdown_d.value == 17){
            c_Day = true;
            //Debug.Log("NICE");
            //output.text = "YESSSSSS";
        } else{
            c_Day = false;
            //output.text = "NOOOOOOOOOO";
        }
        
        

    }


    public void inputDataMonth(){
        if (dropdown_m.value == 9){
            c_Month = true;
            //output.text = "YESSSSSS";
        } else {
            c_Month = false;
        }
    }

    public void p_submitAnswer(){
        if(!submitted){
            submitAnswer();
        }
    }

    public void submitAnswer(){
        submitted = true;

        if (c_Day && c_Month){
            correct.enabled = true;
            Invoke("correctAnswer", 2f);
        }else{
            wrong.enabled = true;
            Invoke("wrongAnswer", 2f);
        }
    }

    public void wrongAnswer(){
        wrong.enabled = false;
        submitted = false;
    }

    public void correctAnswer(){
        SceneManager.LoadScene(loadLevel);
    }
}
