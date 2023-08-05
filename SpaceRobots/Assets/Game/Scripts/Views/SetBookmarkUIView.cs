using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetBookmarkUIView : View
{
    #region Public Variables
    public Button BookmarkButton;
    public Toggle FriendToggle;
    public Toggle FavouriteToggle;
    public Toggle EnemyToggle;
    public CharacterData CharacterData;
    #endregion

    #region Private Variable
    public BookMarkType selected;
    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        FriendToggle.onValueChanged.AddListener(OnTogglePressed);
        FavouriteToggle.onValueChanged.AddListener(OnTogglePressed);
        EnemyToggle.onValueChanged.AddListener(OnTogglePressed);
        BookmarkButton.onClick.AddListener(OnBookMarkPressed);
    }
    #endregion

    #region Callbacks
    private void OnTogglePressed(bool state)
    {
        var currentSelectedItem = EventSystem.current.currentSelectedGameObject.gameObject;

        BookmarkButton.interactable = true;

        if(currentSelectedItem == FriendToggle.gameObject )
        {
            selected = BookMarkType.Friend;

            FriendToggle.SetIsOnWithoutNotify(true);
            FavouriteToggle.SetIsOnWithoutNotify(false);
            EnemyToggle.SetIsOnWithoutNotify(false);
        }
        else if(currentSelectedItem == FavouriteToggle.gameObject )
        {
            selected = BookMarkType.Favourite;

            FavouriteToggle.SetIsOnWithoutNotify(true);
            FriendToggle.SetIsOnWithoutNotify(false);
            EnemyToggle.SetIsOnWithoutNotify(false);
        }
        else if (currentSelectedItem == EnemyToggle.gameObject)
        {
            selected = BookMarkType.Enemy;

            EnemyToggle.SetIsOnWithoutNotify(true);
            FavouriteToggle.SetIsOnWithoutNotify(false);
            FriendToggle.SetIsOnWithoutNotify(false);
        }
    }
    private void OnBookMarkPressed()
    {
        BookMarkData data = new BookMarkData()
        {
            playerName = CharacterData.playerName,
            position = CharacterData.worldMapPosition
        };

        CharacterData myData = Manager.DataManager.characterData;

        if (selected == BookMarkType.Friend)
        {
            if(CharacterData.friendBookMarks.Find(b => b.Equals(data)) == null)
                Manager.DataManager.characterData.friendBookMarks.Add(data);
        }
        else if(selected == BookMarkType.Enemy)
        {
            if (CharacterData.enemyBookMarks.Find(b => b.Equals(data)) == null)
                Manager.DataManager.characterData.enemyBookMarks.Add(data);
        }
        else if (selected == BookMarkType.Favourite)
        {
            if (CharacterData.favBookMarks.Find(b => b.Equals(data)) == null)
                Manager.DataManager.characterData.favBookMarks.Add(data);
        }


        HideView();

        Manager.PlayFabManager.SendPlayerData();
    }
    #endregion
}

public enum BookMarkType
{
    Favourite, Friend, Enemy
}
