using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMarkUIView : View
{
    #region Public Variables
    public UITabsManager tabsManager;
    public Button clickableBackground;
    public GameObject bookMarkItemPrefab;
    public GameObject favParent;
    public GameObject friendsParent;
    public GameObject enemiesParent;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        
        tabsManager.Initialize();
        clickableBackground.onClick.AddListener(HideView);
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
        RefreshUI();
    }
    public void RefreshUI()
    {
        SetLists();
    }
    #endregion

    #region Private Functions
    private void SetLists()
    {
        SetList(friendsParent, Manager.DataManager.characterData.friendBookMarks, BookMarkType.Friend);
        SetList(favParent, Manager.DataManager.characterData.favBookMarks, BookMarkType.Favourite);
        SetList(enemiesParent, Manager.DataManager.characterData.enemyBookMarks, BookMarkType.Enemy);
    }
    private void SetList(GameObject parent, List<BookMarkData> dataList, BookMarkType type)
    {
        var previousItems = parent.GetComponentsInChildren<BookMarkItem>();
        foreach (var item in previousItems)
            Destroy(item.gameObject);

        foreach (var item in dataList)
        {
            GameObject newItem = Instantiate(bookMarkItemPrefab, parent.transform);
            BookMarkItem newBookMark = newItem.GetComponent<BookMarkItem>();

            newBookMark.GoToPosition = item.position;
            newBookMark.bookmarkType = type;
            newBookMark.playerName.text = item.playerName;
            newBookMark.coord.text = "[" + item.position.x + "," + item.position.y + "]";
            newBookMark.GoToButton.onClick.AddListener(HideView);
        }
    }
    #endregion

    #region CallBacks and Coroutines

    #endregion

}
