using Photon.Pun;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIView : View
{
    #region Public Variables
    public GameObject MessageItemPrefab;
    public TMP_InputField messageInput;
    public Button SendButton;
    public GameObject messagesParent;
    public UITabsManager tabsManager;
    public Button BlockingList;
    public TextMeshProUGUI miniMessagePlayerName;
    public TextMeshProUGUI miniMessage;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        SendButton.interactable = false;
     
        SendButton.onClick.AddListener(OnSendPressed);
        BlockingList.onClick.AddListener(OnBlockingListPressed);
        messageInput.onValueChanged.AddListener(OnInputValueChanged);
        tabsManager.Initialize(0);
    }


    #endregion

    #region Public Functions
    public void AddMessage(string senderID, string message)
    {
        Manager.PlayFabManager.GetOtherPlayerData(senderID);

        Manager.PlayFabManager.OnOtherPlayerDataRecieved += (CharacterData playerData) => {

            Manager.PlayFabManager.OnOtherPlayerDataRecieved = null;

            MessageUIItem newMessage = Instantiate(MessageItemPrefab, messagesParent.transform).GetComponent<MessageUIItem>();


            newMessage.InitializeMessage(
                senderID,
                playerData,
                Manager.GameManager.GetCharacterImage(playerData.characterImage),
                playerData.playerName,
                message,
                playerData.GetVIPLevel().ToString());

            miniMessagePlayerName.text = playerData.playerName + " : "; 
            miniMessage.text = " " + message;
        };
    }
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines
    private void OnSendPressed()
    {
        Manager.PhotonManager.SendPublicMessage(messageInput.text);
        messageInput.text = "";
    }
    private void OnBlockingListPressed()
    {
        Debug.Log("On Blocking List Pressed");
    }
    private void OnInputValueChanged(string newValue)
    {
        if(newValue.IsNullOrWhitespace())
            SendButton.interactable = false;
        else 
            SendButton.interactable = true;
    }
    #endregion

}
