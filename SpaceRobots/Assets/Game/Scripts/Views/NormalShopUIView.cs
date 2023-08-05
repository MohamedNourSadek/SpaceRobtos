using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalShopUIView : ShopView
{
    #region Public Variables
    public UITabsManager tabs;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        tabs.Initialize();
        tabs.OnSelectedChanged += RefereshUI;
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
    }
    public void RefereshUI()
    {
        foreach (ShopItem item in items)
        {
            item.selectedImage.gameObject.SetActive(true);
        }

        ConfirmButton.interactable = false;
    }
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines

    #endregion

}
