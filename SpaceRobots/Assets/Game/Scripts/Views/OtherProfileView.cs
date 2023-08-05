using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OtherProfileView : View
{
    #region Public Variables
    public Image playerImage;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerPower;
    public TextMeshProUGUI level;
    public Button outsideClickable;
    public CharacterData characterData;
    public string playerID;

    public Button AddFriend;
    public Button BlockPlayer;
    public Button MessagePlayer;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        outsideClickable.onClick.AddListener(HideView);
        MessagePlayer.onClick.AddListener(MessageOtherPlayer);
        AddFriend.onClick.AddListener(OnAddFriendPressed);
        BlockPlayer.onClick.AddListener(OnBlockFriendPressed);
    }



    private void MessageOtherPlayer()
    {
        View.GetViewGlobally<SendMessageView>().sendToID = playerID;
        View.GetViewGlobally<SendMessageView>().ShowView();
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        playerImage.sprite = Manager.GameManager.GetCharacterImage(characterData.characterImage);
        playerName.text = characterData.playerName;
        playerPower.text = "Power : " + characterData.powerNumber.ToString();
        level.text = "Level " + characterData.GetCurrentlevel().ToString();
        
        if(playerID == Manager.PlayFabManager.playfabID)
        {
            AddFriend.interactable = false;
            BlockPlayer.interactable = false;   
        }
        else
        {
            AddFriend.interactable = !IsAlreadyAdded();
            BlockPlayer.interactable = !IsAlreadyBlocked();
        }
        
        base.ShowView();
    }
    #endregion

    #region Private Functions
    private bool IsAlreadyAdded()
    {
        bool exists = false;

        foreach(var friend in Manager.DataManager.friendsList)
        {
            if(friend.playerID == playerID)
            {
                exists = true;  
            }
        }

         return exists;
    }

    private bool IsAlreadyBlocked()
    {
        bool exists = false;

        foreach (var blocked in Manager.DataManager.blockedList)
        {
            if (blocked.playerID == playerID)
            {
                exists = true;
            }
        }

        return exists;
    }
    #endregion

    #region CallBacks and Coroutines

    private void OnAddFriendPressed()
    {
        if (IsAlreadyAdded() == false)
        {
            Manager.DataManager.friendsList.Add(
                new playerInfo() {
                    friendName = characterData.playerName,
                    playerID = playerID,
                    playerImage = characterData.characterImage
                });

            Manager.PlayFabManager.SendPlayerFriendsData();
            AddFriend.interactable = false;
        }
    }

    private void OnBlockFriendPressed()
    {
        if (IsAlreadyBlocked() == false)
        {
            Manager.DataManager.blockedList.Add(
                new playerInfo()
                {
                    friendName = characterData.playerName,
                    playerID = playerID,
                    playerImage = characterData.characterImage
                });

            Manager.PlayFabManager.SendPlayerBlockedData();
            BlockPlayer.interactable = false;
        }
    }
    #endregion

}
