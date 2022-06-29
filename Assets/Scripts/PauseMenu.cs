using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject p_menu_UI;
    [SerializeField] private bool paused;
    public KeyCode pauseMenu = KeyCode.Escape;   
    void Update()
    {
        if (Input.GetKeyDown(pauseMenu)){
            paused = !paused;
        }

        if (paused){
            activateMenu();
        } else {
            deactivateMenu();
        }
    }

    private void activateMenu(){
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        p_menu_UI.SetActive(true); 

    }

    public void deactivateMenu(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        p_menu_UI.SetActive(false); 
        paused = false;

    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("Mennnu");
    }
}
