using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int win_condition_;

    public TMPro.TMP_Text coin_count_text_;
    public TMPro.TMP_Text win_text_;

    // Use this for initialization
    void Start ()
    {
        win_text_.text = "";
        SetCoinCountText();
        PlayerResources.OnChange += UpdateCoins;
    }

    public void UpdateCoins ()
    {
        SetCoinCountText();
        if (PlayerResources.coins_ >= win_condition_ + 2)
        {
            win_text_.text = "COINPOCALYPSE!";
        }
        else if (PlayerResources.coins_ >= win_condition_ + 1)
        {
            win_text_.text = "COINTALITY!";
        }
        else if (PlayerResources.coins_ >= win_condition_)
        {
            win_text_.text = "You Win!";
        }
    }

    private void SetCoinCountText ()
    {
        coin_count_text_.text = "Collected Coins: " + PlayerResources.coins_.ToString();
    }
}