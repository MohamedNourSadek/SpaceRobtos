using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendMessageView : View
{
    #region Public Variables
    public Button outsideClickable;
    public Button cancel;
    public Button send;
    public TMP_InputField messageTextInput;
    public string sendToID;
    #endregion
    
    #region Private Variables
    
    #endregion
    
    #region Unity Delgates
    
    protected override void Awake()
    {
        base.Awake();
        outsideClickable.onClick.AddListener(HideView);
        cancel.onClick.AddListener(HideView);
        send.onClick.AddListener(OnSendPressed);
        send.onClick.AddListener(HideView);
    }

    #endregion

    #region Public Functions
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines
    private void OnSendPressed()
    {
        Manager.PhotonManager.SendPrivateMessage(sendToID, messageTextInput.text);
        messageTextInput.text = "";
    }
    #endregion

}
