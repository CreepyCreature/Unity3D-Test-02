using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    
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
            SceneManager.LoadSceneAsync(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlayerResources.Reset();
            SceneManager.LoadSceneAsync(2);
        }        
	}
}
