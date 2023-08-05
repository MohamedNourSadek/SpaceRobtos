using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : View
{
    #region Public Variables
    public TabsElement tabsButton;

    public View BaseView;
    public View ResourceView;
    public View WorldMap;

    public Button StageButton;
    public Button TroopsButton;
    public Button Shopview;
    public Button ProfileView;
    public Button EmailView;
    public Button ChatButton;

    public View WorldTopBar;
    public View TopView;
    public View LeftView;
    public View RightView;
    #endregion

    #region Private Variables
    const int defaultSelectedTab = 0;
    
    int currentTab = 0;
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();
        
        ShowView();
        GetView<TopBarView>().RefreshPlayerInformation(Manager.DataManager.characterData);

        SelectTab(defaultSelectedTab);
        tabsButton.GetComponentInChildren<Button>().onClick.AddListener(OnBaseResourcePressed);

        StageButton.onClick.AddListener(View.GetViewGlobally<CampaignUIManager>().ShowView);
        TroopsButton.onClick.AddListener(OnTroopsButtonPressed);
        Shopview.onClick.AddListener(View.GetViewGlobally<NormalShopUIView>().ShowView);
        ProfileView.onClick.AddListener(View.GetViewGlobally<ProfileUIView>().ShowView);
        EmailView.onClick.AddListener(View.GetViewGlobally<MailUIView>().ShowView);
        ChatButton.onClick.AddListener(View.GetViewGlobally<ChatUIView>().ShowView);
    }
    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    private void SelectTab(int toSelect)
    {
        currentTab = toSelect;
        tabsButton.SetTab(currentTab);

        if (currentTab == 0)
        {
            BaseView.ShowView();
            ResourceView.HideView();
            WorldMap.HideView();

            WorldTopBar.HideView();
            TopView.ShowView();
            LeftView.ShowView();
            RightView.ShowView();
        }
        else if (currentTab == 1)
        {
            ResourceView.ShowView();
            BaseView.HideView();
            WorldMap.HideView();

            WorldTopBar.HideView();
            TopView.ShowView();
            LeftView.ShowView();
            RightView.ShowView();
        }
        else
        {
            WorldMap.ShowView();
            ResourceView.HideView();
            BaseView.HideView();

            WorldTopBar.ShowView();
            TopView.HideView();
            LeftView.HideView();
            RightView.HideView();
        }

    }
    private int GetNextTab()
    {
        if (currentTab == 2)
            return 0;
        else
            return currentTab + 1;
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnBaseResourcePressed()
    {
        int nextTab =  GetNextTab();
        SelectTab(nextTab);
    }
    private void OnTroopsButtonPressed()
    {
        View.GetViewGlobally<ExpeditionUIManager>().expeditionType = ExpeditionType.Setting;
        View.GetViewGlobally<ExpeditionUIManager>().ShowView();
    }
    #endregion
}
