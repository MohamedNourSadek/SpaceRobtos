using Photon.Chat.Demo;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.TabViewManager;

public class SocialUIView : View
{
    #region Public Variables
    public GameObject friendsListParent;
    public GameObject blockedListParent;
    public GameObject friendPrefab;
    public UITabsManager tabsManager;
    #endregion

    #region Private Variables
    #endregion

    #region Unity Delgates

    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();

        RefereshUI();
        tabsManager.Initialize();
        tabsManager.OnSelectedChanged += RefereshUI;
    }
    public void RefereshUI()
    {
        var elements = GetComponentsInChildren<FriendUIItem>();

        foreach(var element in  elements)
        {
            Destroy(element.gameObject);
        }

        foreach(var friend in DataManager.DataManager.friendsList)
        {
            FriendUIItem friendItem = Instantiate(friendPrefab, friendsListParent.transform).GetComponent<FriendUIItem>();

            friendItem.friendName.text = friend.friendName;
            friendItem.friendImage.sprite = Manager.GameManager.GetCharacterImage(friend.playerImage);
            friendItem.playerInfo = friend;
            friendItem.undoText.text = "Unfriend";
            friendItem.isFriend = true;
        }

        foreach (var blocked in DataManager.DataManager.blockedList)
        {
            FriendUIItem blockedItem = Instantiate(friendPrefab, blockedListParent.transform).GetComponent<FriendUIItem>();

            blockedItem.friendName.text = blocked.friendName;
            blockedItem.friendImage.sprite = Manager.GameManager.GetCharacterImage(blocked.playerImage);
            blockedItem.playerInfo = blocked;
            blockedItem.undoText.text = "Unblock";
            blockedItem.isFriend = false;
        }
    }
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines

    #endregion

}

[System.Serializable]
public class playerInfo
{
    public string playerID;
    public string friendName;
    public int playerImage;
}
