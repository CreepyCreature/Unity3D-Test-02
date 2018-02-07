using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame ()
    {
        int lastScene = GameData.GetLastScene();
        lastScene = lastScene > 0 ? lastScene : SceneManager.GetActiveScene().buildIndex + 1;
        GameManager.Instance.LoadScene(lastScene);
    }

    public void NewGame ()
    {
        GameManager.Instance.ResetGameState();
        PlayGame();
    }

    public void QuitGame ()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
