using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string loadLevel;

    public void PlayGame()
    {
        SceneManager.LoadScene(loadLevel);
    }

    public void QuitGame()
    {
        Application.Quit();    
    }

}
