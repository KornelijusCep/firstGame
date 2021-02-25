using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isGamePause = false;

    public GameObject pauseMenuUI;
    public GameObject GameInUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isGamePause){
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume()
    {
        GameInUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePause = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        GameInUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePause = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LaunchMenu(){
        Resume();
        SceneManager.LoadScene("MainMenu");
    }   

    public void QuitGame(){
        Application.Quit();
    }
}
