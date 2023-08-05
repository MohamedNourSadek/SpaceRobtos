using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftBarUIView : View
{
    #region Public Variables
    public Button EventsButton;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();
        EventsButton.onClick.AddListener(View.GetViewGlobally<EventsUIView>().ShowView);
    }
    #endregion

    #region Public Functions

    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines

    #endregion

}
