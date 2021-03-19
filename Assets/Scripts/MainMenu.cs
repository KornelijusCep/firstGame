using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    PlayerData data;
    MainMenuData menu;

    public Button loadButton;

    private int level;
    public bool isGameCreated;

    void Start(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;   

        data = SaveSystem.LoadPlayer();
        menu = SaveSystem.LoadMainMenu();

        if(!menu.isGameCreated)
            loadButton.interactable = false;
        else
            loadButton.interactable = true;
            
    }

    public void PlayGame()
    {
        isGameCreated = true;
        SaveSystem.SaveMainMenu(this);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        
    }

    public void LoadGame()
    {
        level = data.level;
        SceneManager.LoadScene(level);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}


