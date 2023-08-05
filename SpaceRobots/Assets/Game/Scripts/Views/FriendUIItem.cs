using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FriendUIItem : MonoBehaviour
{
    public TextMeshProUGUI friendName;
    public Image friendImage;
    public TextMeshProUGUI undoText;
    public Button undoButton;

    public playerInfo playerInfo;
    public bool isFriend;
    private void Awake()
    {
        undoButton.onClick.AddListener(OnUndo);
    }

    private void OnUndo()
    {
        List<playerInfo> listToRemoveFrom = isFriend ? Manager.DataManager.friendsList : Manager.DataManager.blockedList;

        playerInfo player = listToRemoveFrom.Find(element => element.playerID == playerInfo.playerID);
        
        if (player != null)
            listToRemoveFrom.Remove(player);

        if(isFriend)
            Manager.PlayFabManager.SendPlayerFriendsData();
        else
            Manager.PlayFabManager.SendPlayerBlockedData();

        Destroy(this.gameObject);
    }
}
