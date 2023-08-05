using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageUIItem : MonoBehaviour
{
    #region Public Variables
    public Image PlayerImage;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerRank;
    public TextMeshProUGUI Message;
    public Button playerButton;
    #endregion

    #region Private Variables
    CharacterData myData;
    string mySenderID;
    #endregion
    
    #region Unity Delgates
    private void Awake()
    {
        playerButton.onClick.AddListener(OpenPlayerProfile);
    }

    #endregion

    #region Public Functions
    public void InitializeMessage(string senderID, CharacterData data, Sprite playerImage, string playerName, string playerMessage, string playerRank)
    {
        PlayerImage.sprite = playerImage;
        PlayerName.text = playerName;
        PlayerRank.text = "VIP:" + playerRank;
        Message.text = playerMessage;
        myData = data;
        mySenderID = senderID;
    }
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines
    private void OpenPlayerProfile()
    {
        View.GetViewGlobally<OtherProfileView>().characterData = myData;
        View.GetViewGlobally<OtherProfileView>().playerID = mySenderID;
        View.GetViewGlobally<OtherProfileView>().ShowView();
    }
    #endregion

}
