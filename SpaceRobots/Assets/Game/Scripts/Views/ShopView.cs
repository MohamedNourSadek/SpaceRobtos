using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopView : View
{
    #region Public Variables
    public List<ShopItem> items;
    public Button ConfirmButton;
    #endregion

    #region Private Variables
    ShopItem selectedButton;
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();

        foreach (ShopItem item in items)
            item.myButton.onClick.AddListener(OnStoreItemPressed);

        ConfirmButton.onClick.AddListener(OnConfirmPressed);
    }
    #endregion

    #region Public Functions
    public void BuyItems()
    {
        StartCoroutine(BuyItem());
    }
    #endregion

    #region Private Functions
    IEnumerator BuyItem()
    {
        View.GetViewGlobally<LoadingView>().ShowView();
        yield return new WaitForSecondsRealtime(2f);

        if(selectedButton.itemType == ShopItemType.GEM)
        {
            Manager.DataManager.characterData.gemAmount += selectedButton.amount;
            Manager.DataManager.characterData.AddVIPPoints(selectedButton.price * 20);
        }
        else if(selectedButton.itemType == ShopItemType.Chip) 
        {
            Manager.DataManager.characterData.chips += selectedButton.amount;
        }

        if(selectedButton.currencyType == CurrencyType.GEM)
        {
            Manager.DataManager.characterData.gemAmount -= selectedButton.price;
        }


        View.GetViewGlobally<LoadingView>().HideView();
        OnBuySuccess();
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnStoreItemPressed()
    {
        foreach (ShopItem item in items)
        {
            if (EventSystem.current.currentSelectedGameObject == item.gameObject)
            {
                item.selectedImage.gameObject.SetActive(false);
                selectedButton = item;

                if (selectedButton.currencyType == CurrencyType.GEM)
                {
                    if (selectedButton.price <= GameManager.DataManager.characterData.gemAmount)
                        ConfirmButton.interactable = true;
                    else
                        ConfirmButton.interactable = false;
                }
                else
                    ConfirmButton.interactable = true;
            }
            else 
            {
                item.selectedImage.gameObject.SetActive(true);
            }
        }
    }
    private void OnConfirmPressed()
    {
        BuyItems();
    }
    private void OnBuySuccess()
    {
        View.GetViewGlobally<TopBarView>().RefreshPlayerInformation(Manager.DataManager.characterData);
        View.GetViewGlobally<NormalShopUIView>().RefereshUI();
        
        GameManager.PlayFabManager.SendPlayerData();
    }
    #endregion

}
