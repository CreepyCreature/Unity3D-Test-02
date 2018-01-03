using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerResources
{
	public static int coins_ { get; private set; }

    public delegate void CoinCollected();
    public static event CoinCollected OnChange;

    //public PlayerResources () {; }

    public static void CollectCoin ()
    {
        coins_++;
        if (OnChange != null) OnChange();
    }

    public static void Reset ()
    {
        coins_ = 0;
        if (OnChange != null) OnChange();
    }
}
