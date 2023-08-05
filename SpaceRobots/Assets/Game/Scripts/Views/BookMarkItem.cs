using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookMarkItem : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI coord;
    public Button GoToButton;
    public Button Remove;
    public Vector2Int GoToPosition= new Vector2Int(1,1);
    public BookMarkType bookmarkType = BookMarkType.Friend;

    private void Awake()
    {
        GoToButton.onClick.AddListener(OnGoToPressed);
        Remove.onClick.AddListener(OnRemove);
    }

    private void OnGoToPressed()
    {
        View.GetViewGlobally<WorldUIView>().SwitchToPosition(GoToPosition);
    }
    private void OnRemove()
    {
        if(bookmarkType == BookMarkType.Friend)
            RemoveFromList(Manager.DataManager.characterData.friendBookMarks);
        else if(bookmarkType == BookMarkType.Favourite)
            RemoveFromList(Manager.DataManager.characterData.favBookMarks);
        else if (bookmarkType == BookMarkType.Enemy)
            RemoveFromList(Manager.DataManager.characterData.enemyBookMarks);
        
        View.GetViewGlobally<BookMarkUIView>().RefreshUI();
        Manager.PlayFabManager.SendPlayerData();
    }

    private void RemoveFromList(List<BookMarkData> data)
    {
        BookMarkData bookMarkData = new BookMarkData() { playerName = playerName.text, position = GoToPosition };

        var item = data.Find(b => b.Equals(bookMarkData));

        if (item != null)
            data.Remove(item);
    }
}
