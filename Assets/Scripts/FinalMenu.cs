using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMenu : MonoBehaviour
{
    
    [SerializeField] private string loadLevel;

    void Start()
    {
        Invoke("MainMenuReset", 2f);
    }

    void MainMenuReset(){
        SceneManager.LoadScene(loadLevel);
    }
}
