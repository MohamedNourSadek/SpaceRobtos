using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageUIView : View
{
    #region Public Variables
    public TextMeshProUGUI messageTitle;
    public TextMeshProUGUI messageDetails;
    public Button OkButton;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        OkButton.onClick.AddListener(OkButton.onClick.RemoveAllListeners);
    }
    #endregion

    #region Public Functions
    public void ShowView(string title, string detail, UnityAction OnOkPress)
    {
        ShowView();
        messageTitle.text = title;
        messageDetails.text = detail;
        OkButton.onClick.AddListener(OnOkPress);
    }
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines

    #endregion

}
