using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    public TMP_Text coinsText;
    private int coins;

    void Awake()
    {
        if (coinsText == null)
        {
            GameObject go = GameObject.Find("CoinsQuantity");
            if (go != null)
                coinsText = go.GetComponent<TMP_Text>();
        }
        UpdateCoinBar();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log($"Coins changed: {amount} Total: {coins}");
        UpdateCoinBar();
    }

    private void UpdateCoinBar()
    {
        if (coinsText != null)
            coinsText.text = coins.ToString();
    }

    public int GetCoins()
    {
        return coins;
    }

    public void SetCoins(int amount)
    {
        coins = amount;
        UpdateCoinBar();
    }
}