using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject p_menu_UI;
    []
    public KeyCode pauseMenu = KeyCode.Escape;   
    void Update()
    {
        if (Input.GetKeyDown(pauseMenu)){
            
        }
    }

    private void activateMenu(){
       p_menu_UI.SetActive(true); 
    }

    private void deactivateMenu(){
        p_menu_UI.SetActive(false); 
    }
}
