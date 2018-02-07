using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PickupItemInfo
{
    public int id;
    public string name;
    public string description;

    public PickupItemInfo(int itemID, string itemName, string itemDescription)
    {
        id = itemID;
        name = itemName;
        description = itemDescription;
    }
}

public static class PlayerResources
{
	public static int Coins { get; set; }
    public static List<PickupItemInfo> inventoryItems = new List<PickupItemInfo>();

    public delegate void CoinCountChanged();
    public static event CoinCountChanged OnCoinChange;
    public static event CoinCountChanged OnCoinCollected;

    public delegate void CollectedItem(PickupItemInfo item);
    public static event CollectedItem OnItemCollected;

    //public PlayerResources () {; }

    public static void CollectCoin ()
    {
        Coins++;
        if (OnCoinChange != null) OnCoinChange();
        if (OnCoinCollected != null) OnCoinCollected();
    }

    public static void CollectItem (int itemID, string itemName, string itemDescription)
    {
        PickupItemInfo item = new PickupItemInfo(itemID, itemName, itemDescription);
        inventoryItems.Add(item);
        if (OnItemCollected != null) OnItemCollected(item);
    }

    public static void Reset ()
    {
        Coins = 0;
        if (OnCoinChange != null) OnCoinChange();

        inventoryItems.Clear();
    }
}
