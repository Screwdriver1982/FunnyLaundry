using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountScript : MonoBehaviour
{
    

    public Text goldTxt;
    public Text shieldsTxt;
    public Text heartsTxt;
    public Text shieldPriceTxt;
    public Text heartsPriceTxt;

    public int shieldPrice;
    public int heartsPrice;

    int playerGold;
    int playerShields;
    int playerHearts;

    // Start is called before the first frame update
    void Start()
    {
        GetCurrency();
        CurrencyTxtUpdate();
        ShopTxtUpdate();



    }

    public void CurrencyTxtUpdate()
    {
        goldTxt.text = "" + playerGold;
        shieldsTxt.text = "" + playerShields;
        heartsTxt.text = "" + playerHearts;
    }

    public void GetCurrency()
    {
        playerGold = PlayerPrefs.GetInt("PlayerGold", 0);
        playerShields = PlayerPrefs.GetInt("PlayerShields", 0);
        playerHearts = PlayerPrefs.GetInt("PlayerHearts", 0);
    }

    // Update is called once per frame

    public void ShopTxtUpdate()
    {
        shieldPriceTxt.text = "" + shieldPrice;
        heartsPriceTxt.text = "" + heartsPrice;
    }

    void Update()
    {
        
    }

    public void CurrencyChanges(int difGold, int difShields, int difHearts)
    {
        if (difGold != 0)
        { 
            playerGold += difGold;
            PlayerPrefs.SetInt("PlayerGold", playerGold);    
        }
        
        if (difShields != 0)
        { 
            playerShields += difShields;
            PlayerPrefs.SetInt("PlayerShields", playerShields);
        }
        
        if (difHearts != 0)
        {
            playerHearts += difHearts;
            PlayerPrefs.SetInt("PlayerHearts", playerHearts);
           
           
        }

        CurrencyTxtUpdate();



    }

    public void BuyShields()
    {
        if (shieldPrice <= playerGold)
        { 
            CurrencyChanges(-shieldPrice, 1, 0);
        }
        
    }
    public void BuyHearts()
    {
        
        if (heartsPrice <= playerGold)
        {
            CurrencyChanges(-heartsPrice, 0, 1);
        }
    }


}
