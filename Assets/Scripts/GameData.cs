using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;

using System.Linq;

//[System.Serializable]
public struct CheckpointStruct
{
    public int SceneIndex { get; set; }
    public Vector3 Position { get; set; }
}

public struct CoinCountStruct
{
    public int SceneIndex { get; set; }
    public int CoinCount { get; set; }
}

public struct DestroyedCoinStruct
{
    public int SceneIndex { get; set; }
    public string CoinName { get; set; }
}

[XmlRoot("GameData")]
public class GameData
{
    private static /*readonly*/ GameData instance = new GameData();
    static GameData () { }
    private GameData () { }
    public static GameData Instance { get { return instance; } private set { instance = value; } }
        
    private static string XMLFileName = "/GameData.xml";
    public static string gameDataFile = Application.persistentDataPath + XMLFileName;

    [XmlArray("DestroyedCoins")]
    [XmlArrayItem(ElementName = "DestroyedCoin")]
    public List<DestroyedCoinStruct> destroyedCoins = new List<DestroyedCoinStruct>();

    [XmlArray("CoinCount")]
    [XmlArrayItem(ElementName = "Coins")]
    public List<CoinCountStruct> coinCountProxy = new List<CoinCountStruct>();

    [XmlArray("Checkpoints")]
    [XmlArrayItem(ElementName = "Checkpoint")]
    public List<CheckpointStruct> checkpointsProxy = new List<CheckpointStruct>();

    [XmlElement("LastSceneIndex")]
    public int lastSceneIndex = 0;

    [XmlArray("InventoryItems")]
    [XmlArrayItem(ElementName = "InventoryItem")]
    public List<PickupItemInfo> inventoryItems = new List<PickupItemInfo>();

    [XmlIgnore]
    private static Dictionary<int, Vector3> checkpoints = new Dictionary<int, Vector3>();

    public static void Save ()
    {
        foreach (var i in Instance.inventoryItems)
        {
            Debug.LogWarning(i.name);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream fileStream = new FileStream(gameDataFile, FileMode.Create);
        serializer.Serialize(fileStream, Instance);
        fileStream.Close();

        // Debug.Log("GameData::Game Data saved in " + gameDataFile);
    }

    public static void Load ()
    {
        if (!File.Exists(gameDataFile))
        {
            Debug.LogWarning("GameData::Save file " + gameDataFile + " not found!");
            return;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream fileStream = new FileStream(gameDataFile, FileMode.Open);

        try
        {
            Instance = serializer.Deserialize(fileStream) as GameData;
            DeserializeCheckpoints();
        }
        finally
        {
            fileStream.Close();
        }
    }

    public static void Clear ()
    {
        Instance.destroyedCoins.Clear();
        Instance.coinCountProxy.Clear();
        Instance.checkpointsProxy.Clear();
        checkpoints.Clear();
        Instance.inventoryItems.Clear();
        Save();
    }

    public static void UpdateDestroyedCoins (int sceneIndex, string coinName)
    {
        if (Instance.destroyedCoins.Exists(x => x.SceneIndex == sceneIndex && x.CoinName == coinName))
        {
            return;
        }

        DestroyedCoinStruct coinStruct = new DestroyedCoinStruct();
        coinStruct.SceneIndex = sceneIndex;
        coinStruct.CoinName = coinName;
        Instance.destroyedCoins.Add(coinStruct);
    }

    public static void UpdateDestroyedItems(int sceneIndex, string coinName)
    {
        if (Instance.destroyedCoins.Exists(x => x.SceneIndex == sceneIndex && x.CoinName == coinName))
        {
            return;
        }

        DestroyedCoinStruct coinStruct = new DestroyedCoinStruct();
        coinStruct.SceneIndex = sceneIndex;
        coinStruct.CoinName = coinName;
        Instance.destroyedCoins.Add(coinStruct);
    }

    public static List<string> GetDestroyedCoins (int sceneIndex)
    {
        List<string> coinNamesList = new List<string>();
        var coinStruct = Instance.destroyedCoins.FindAll(x => x.SceneIndex == sceneIndex);
        foreach (var coin in coinStruct)
        {
            coinNamesList.Add(coin.CoinName);
        }

        return coinNamesList;
    }

    public static void UpdateCoinCount (int sceneIndex, int coinCount)
    {
        if (Instance.coinCountProxy.Exists(x => x.SceneIndex == sceneIndex))
        {
            Instance.coinCountProxy.RemoveAll(x => x.SceneIndex == sceneIndex);
        }

        CoinCountStruct coinStruct = new CoinCountStruct();
        coinStruct.SceneIndex = sceneIndex;
        coinStruct.CoinCount = coinCount;
        Instance.coinCountProxy.Add(coinStruct);
    }

    public static int GetCoinCount (int sceneIndex)
    {
        if (Instance.coinCountProxy.Exists(x => x.SceneIndex == sceneIndex))
        {
            int count = Instance.coinCountProxy.Find(x => x.SceneIndex == sceneIndex).CoinCount;
            return count;
        }

        return 0;
    }

    public static void SetCheckpoint (int sceneIndex, Vector3 position)
    {
        checkpoints[sceneIndex] = position;

        if (Instance.checkpointsProxy.Exists(x => x.SceneIndex == sceneIndex))
        {
            Instance.checkpointsProxy.RemoveAll(x => x.SceneIndex == sceneIndex);
        }

        CheckpointStruct newCheckpoint = new CheckpointStruct();
        newCheckpoint.SceneIndex = sceneIndex;
        newCheckpoint.Position = position;
        Instance.checkpointsProxy.Add(newCheckpoint);

        Instance.lastSceneIndex = sceneIndex;
    }

    public static Dictionary<int, Vector3> GetCheckpoints ()
    {
        return checkpoints;
    }

    public static int GetLastScene ()
    {
        return Instance.lastSceneIndex;
    }

    private static void DeserializeCheckpoints ()
    {
        var checkpointList = Instance.checkpointsProxy;
        foreach (var c in checkpointList)
        {
            checkpoints[c.SceneIndex] = c.Position;
        }
    }

    public static void SaveInventoryItem (PickupItemInfo item)
    {
        if (Instance.inventoryItems.Exists(x => x.id == item.id))
        {
            Instance.inventoryItems.RemoveAll(x => x.id == item.id);
        }

        Instance.inventoryItems.Add(item);
        Debug.Log("Added " + item.name);

        foreach (var i in Instance.inventoryItems)
        {
            Debug.Log(i.name);
        }
    }

    public static List<PickupItemInfo> GetInventoryItems ()
    {
        return Instance.inventoryItems.ToList();
    }
}
