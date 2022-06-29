using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string loadLevel;

    private void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(loadLevel);
    }

    public void QuitGame()
    {
        Application.Quit();    
    }

}
