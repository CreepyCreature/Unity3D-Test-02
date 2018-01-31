using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;

using System.Linq;

//[System.Serializable]
public struct CheckpointProxy
{
    public int Key { get; set; }
    public Vector3 Value { get; set; }
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

    [XmlArray("Checkpoints")]
    [XmlArrayItem(ElementName = "Checkpoint")]
    public List<CheckpointProxy> checkpointProxy = new List<CheckpointProxy>();

    [XmlIgnore]
    private static Dictionary<int, Vector3> checkpoints = new Dictionary<int, Vector3>();
    
    //[XmlArray("Checkpoints")]
    //[XmlArrayItem(ElementName = "Checkpoint")]
    //private static List<CheckpointProxy> CheckpointProxy { get; set; }
    //[XmlIgnore]
    //private static Dictionary<int, Vector3> CheckpointDictionary
    //{
    //    get { return CheckpointProxy.ToDictionary(x => x.Key, x => x.Value); }
    //    set { CheckpointProxy = value.Select(x => new global::CheckpointProxy() { Key = x.Key, Value = x.Value }).ToList(); }
    //}

    public static void Save ()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream fileStream = new FileStream(gameDataFile, FileMode.Create);
        serializer.Serialize(fileStream, Instance);
        fileStream.Close();

        Debug.Log("GameData::Game Data saved in " + gameDataFile);
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

        foreach (var c in instance.checkpointProxy)
        {
            Debug.Log("Loaded " + c.Key + " => " + c.Value);
        }
    }

    public static void SetCheckpoint (int sceneIndex, Vector3 position)
    {
        checkpoints[sceneIndex] = position;

        if (Instance.checkpointProxy.Exists(x => x.Key == sceneIndex))
        {
            Instance.checkpointProxy.RemoveAll(x => x.Key == sceneIndex);
        }

        CheckpointProxy newCheckpoint = new CheckpointProxy();
        newCheckpoint.Key = sceneIndex;
        newCheckpoint.Value = position;
        Instance.checkpointProxy.Add(newCheckpoint);
    }

    public static Dictionary<int, Vector3> GetCheckpoints ()
    {
        return checkpoints;
    }

    private static void DeserializeCheckpoints ()
    {
        var checkpointList = Instance.checkpointProxy;
        foreach (var c in checkpointList)
        {
            checkpoints[c.Key] = c.Value;
        }
    }

    private static void SerializeCheckpointDictionary()
    {
        //foreach (var key in checkpoints.Keys)
        //{
        //    Debug.Log("adding");
        //    Instance.checkpointProxy.Add(new CheckpointProxy(key, checkpoints[key]));
        //}

        //foreach (var e in Instance.checkpointProxy)
        //{
        //    Debug.Log(e.Key + " => " + e.Value);
        //}
    }
}
