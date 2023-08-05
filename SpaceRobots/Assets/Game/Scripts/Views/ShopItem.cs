using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour 
{
    public int amount;
    public int price;
    public Image selectedImage;
    public Button myButton;
    public CurrencyType currencyType;
    public ShopItemType itemType;

    private void Awake()
    {
        myButton = GetComponent<Button>();  
    }
}

public enum ShopItemType
{
    GEM, Chip
}

public enum CurrencyType
{
    USD, GEM
}
