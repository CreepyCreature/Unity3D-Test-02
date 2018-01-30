using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    private Vector3 savedPosition;
    private Dictionary<int, Vector3> checkpointDictionary = new Dictionary<int, Vector3>();
    
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {  Destroy(gameObject); return;  }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlayerResources.Reset();
            LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlayerResources.Reset();
            LoadScene(2);
        }        
	}

    public void LoadScene (int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void SavePlayerPosition (Vector3 newPosition)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        checkpointDictionary[sceneIndex] = newPosition;
        savedPosition = newPosition;
        Debug.Log("GameManager::Saving Player position " + checkpointDictionary[sceneIndex] + " at Scene Index "+ sceneIndex);
    }

    public Vector3 GetPlayerPosition (int sceneIndex = -1)
    {
        if (sceneIndex < 0)
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        if (HasPlayerPosition(sceneIndex))
        {
            return checkpointDictionary[sceneIndex];
        }

        return new Vector3(0, 0, 0);
    }

    public bool HasPlayerPosition (int sceneIndex)
    {
        if (checkpointDictionary.ContainsKey(sceneIndex))
            return true;
        else
            return false;
    }
}
