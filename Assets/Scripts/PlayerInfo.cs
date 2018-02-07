using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int win_condition_;

    public GameObject playerInfoUI;
    public TMPro.TMP_Text coin_count_text_;
    public TMPro.TMP_Text win_text_;
    public GameObject coin_collected_text_;

    // Use this for initialization
    void Start ()
    {
        win_text_.text = "";
        SetCoinCountText();
        PlayerResources.OnCoinChange += UpdateCoins;
        PlayerResources.OnCoinCollected += OnCoinCollected;

        UpdateCoins();
    }

    public void UpdateCoins ()
    {
        SetCoinCountText();
        if (PlayerResources.Coins >= win_condition_ + 2)
        {
            win_text_.text = "COINPOCALYPSE!";
        }
        else if (PlayerResources.Coins >= win_condition_ + 1)
        {
            win_text_.text = "COINTALITY!";
        }
        else if (PlayerResources.Coins >= win_condition_)
        {
            win_text_.text = "You Win!";
        }
    }

    public void OnCoinCollected ()
    {
        GameObject playerInfoUI = GameObject.FindGameObjectWithTag("PlayerInfoUI");
        GameObject toast = Instantiate(coin_collected_text_, playerInfoUI.transform);
        Destroy(toast, 0.66f);
    }

    private void SetCoinCountText ()
    {
        coin_count_text_.text = "Collected Coins: " + PlayerResources.Coins.ToString();
    }
}