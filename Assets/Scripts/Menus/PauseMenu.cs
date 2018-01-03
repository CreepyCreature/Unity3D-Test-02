using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [System.ComponentModel.DefaultValue(false)]
    public static bool gameIsPaused { get; private set; }
    public GameObject pauseMenuUI;

    public GameObject defaultMenu;
    public GameObject optionsMenu;
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
	}

    public void Resume ()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }

    void Pause ()
    {
        Time.timeScale = 0f;

        // Make sure the Default Menu is always the starting menu
        // when the player pauses the game
        defaultMenu.SetActive(true);
        optionsMenu.SetActive(false);

        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }

    public void LoadMenu ()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;

        // Dear future me, it's not the best idea to reset the game
        // state from the Pause Menu. Please think of something better,
        // regards, past (present?) me.
        PlayerResources.Reset();

        SceneManager.LoadScene("MainMenu");
    }

    public void Quit ()
    {
        Debug.Log("PauseMenu::Quit");
        Application.Quit();
    }
}
