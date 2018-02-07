using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour {

    public static InventoryPanel Instance { get; private set;
    }
    public RectTransform root;
    public InventoryEntry inventoryItemPrefab;

    private List<InventoryEntry> entryInstances = new List<InventoryEntry>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);        
    }

    // Use this for initialization
    void Start ()
    {
        Populate();
	}

    public void Populate ()
    {
        foreach (var instance in entryInstances)
        {
            Destroy(instance.gameObject);
        }
        entryInstances.Clear();

        //foreach (var item in GameData.Instance.inventoryItems)
        foreach (var item in GameManager.Instance.GetInventoryList())
        {
            Debug.Log(item.id + " " + item.name + ": " + item.description);
            var entryInstance = CreateItemInstance(item);
            entryInstances.Add(entryInstance);
        }
    }
	
	private InventoryEntry CreateItemInstance (PickupItemInfo item)
    {
        var entry = Instantiate(inventoryItemPrefab, root.transform);
        entry.itemName.text = item.name;
        entry.itemDescription.text = item.description;
        return entry;
    }
}
