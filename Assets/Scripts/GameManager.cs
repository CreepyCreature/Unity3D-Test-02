using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    private Vector3 savedPosition;
    private Dictionary<int, Vector3> checkpointDictionary = new Dictionary<int, Vector3>();
    private List<string> destroyedCoins = new List<string>();

    private List<PickupItemInfo> pickUpItems = new List<PickupItemInfo>();
    
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {  Destroy(gameObject); return;  }
        
        Initialize();
    }

    private void Initialize ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameData.Load();
        checkpointDictionary = GameData.GetCheckpoints();

        int sceneIndex = ActiveSceneIndex();
        PlayerResources.Coins = GameData.GetCoinCount(sceneIndex);
        destroyedCoins = GameData.GetDestroyedCoins(sceneIndex);
        pickUpItems = GameData.GetInventoryItems();
        //destroyedItems = GameData.GetDestroyedItems(sceneIndex);

        PlayerResources.OnItemCollected += OnItemCollected;
    }

	void Start ()
    {
        DestroyCollectedCoins();
    }
	
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

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameData.Load();
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ResetGameState();
        }
	}

    public void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene \"" + scene.name + "\" loaded!");
        DestroyCollectedCoins();
    }

    public void LoadScene (int sceneIndex)
    {
        //SceneManager.LoadSceneAsync(sceneIndex);
        pickUpItems.Clear();
        destroyedCoins = GameData.GetDestroyedCoins(sceneIndex);
        PlayerResources.Coins = GameData.GetCoinCount(sceneIndex);
        pickUpItems = GameData.GetInventoryItems();

        Debug.LogWarning("In LoadScene");
        foreach (var i in pickUpItems)
        {
            Debug.Log(i.name);
        }

        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync (int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            Debug.Log(progress);
            yield return null;
        }
    }

    private void DestroyCollectedCoins ()
    {
        foreach (var coin in GameObject.FindGameObjectsWithTag("PickUpCoin"))
        {
            // Debug.Log("Analyzing " + coin.name);

            if (destroyedCoins.Exists(x => x == coin.name))
            {                
                // Debug.LogWarning("Destroying " + coin.name);
                Destroy(coin.gameObject);
            }
            else
            {
                // Debug.Log("Adding callback to " + coin.name);
                coin.GetComponent<PickUpCoin>().OnCoinDestroyed += OnCoinDestroyed;
            }
        }

        foreach (var item in GameObject.FindGameObjectsWithTag("PickUp"))
        {
            Debug.Log("Analyzing " + item.name);

            if (destroyedCoins.Exists(x => x == item.name))
            {
                Debug.LogWarning("Destroying " + item.name);
                Destroy(item.gameObject);
            }
            else
            {
                Debug.Log("Adding callback to " + item.name);
                item.GetComponent<PickupItem>().OnPickUpItemDestroyed += OnItemDestroyed;
            }
        }
        foreach (var it in destroyedCoins)
        {
            Debug.Log(it);
        }
    }

    // Coin info is saved when a Checkpoint is activated, see SaveCheckpoint(..)
    public void SaveCoinData ()
    {
        int sceneIndex = ActiveSceneIndex();
        GameData.UpdateCoinCount(sceneIndex, PlayerResources.Coins);

        foreach (var coinName in destroyedCoins)
        {
            GameData.UpdateDestroyedCoins(sceneIndex, coinName);
        }
    }

    public void OnCoinDestroyed (string gameObjectName)
    {
        destroyedCoins.Add(gameObjectName);
        // Debug.Log("GameManager::Adding coin \"" + gameObjectName + "\" to destroyed list!");
    }

    public void OnItemDestroyed (string gameObjectName)
    {
        destroyedCoins.Add(gameObjectName);
    }

    public void SaveCheckpoint (Vector3 newPosition)
    {
        int sceneIndex = ActiveSceneIndex();
        checkpointDictionary[sceneIndex] = newPosition;
        savedPosition = newPosition;

        // Coin info is saved when a Checkpoint is activated
        SaveCoinData();
        SaveInventoryData();

        GameData.SetCheckpoint(sceneIndex, newPosition);
        GameData.Save();

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

    public void OnItemCollected (PickupItemInfo item)
    {
        // GameData.SaveInventoryItem(item);
        Debug.Log("Collected " + item.name);
        pickUpItems.Add(item);
        InventoryPanel.Instance.Populate();
    }

    private void SaveInventoryData ()
    {
        foreach (var inventoryItem in pickUpItems)
        {
            Debug.Log("Calling SaveInventoryData with " + inventoryItem.name);
            GameData.SaveInventoryItem(inventoryItem);
        }
    }

    public List<PickupItemInfo> GetInventoryList ()
    {
        return pickUpItems;
    }

    public void ResetGameState ()
    {
        checkpointDictionary.Clear();
        destroyedCoins.Clear();
        PlayerResources.Reset();
        GameData.Clear();
    }

    // Gets the active scene's build index; saving the world
    // one keystroke at a time.
    private int ActiveSceneIndex ()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
