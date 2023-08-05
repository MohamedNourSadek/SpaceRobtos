using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsUIView : View
{
    #region Public Variables
    public UITabsManager tabsManager;
    public Button DevelopementButton;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        tabsManager.Initialize();

        DevelopementButton.onClick.AddListener(View.GetViewGlobally<DevelopmentUIView>().ShowView);
    }
    #endregion

    #region Public Functions

    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines

    #endregion

}
