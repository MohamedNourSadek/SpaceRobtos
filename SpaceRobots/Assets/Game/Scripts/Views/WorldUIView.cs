using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldUIView : View
{
    public static Vector2Int maxValue = new Vector2Int(30, 30);

    #region Public Variables
    public List<List<WorldUIPosition>> WorldUIPositions = new List<List<WorldUIPosition>>();
    public GameObject positionsParent;
    public GameObject WorldRowObject;
    public GameObject WorldPositionInstance;
    public Vector2 spacing = new Vector2(10,10);
    public Button search;
    public Button bookmark;
    public Button findMe; 
    public Button marching;
    public TMPro.TMP_InputField x; 
    public TMPro.TMP_InputField y;
    public ScrollRect scrollRect;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        search.onClick.AddListener(OnSearchPressed);
        bookmark.onClick.AddListener(OnBookMarkPressed);
        findMe.onClick.AddListener(OnFindMePressed);
        x.onValueChanged.AddListener(OnXYChanged);
        y.onValueChanged.AddListener(OnXYChanged);
        scrollRect.onValueChanged.AddListener(OnMapChanged);
        marching.onClick.AddListener(OnMarchingPressed);
        CreatePositions();
        RefreshUI();
    }
    #endregion

    #region Public Functions
    public void RefreshUI()
    {
        foreach (var positionRow in WorldUIPositions)
            foreach(var position in positionRow)
                position.Initialize(null, "");

        foreach (var player in GameManager.DataManager.otherPlayersInCurrentMap)
        {
            if(player.Value.worldMapPosition.magnitude != 0)
                WorldUIPositions[player.Value.worldMapPosition.x-1][player.Value.worldMapPosition.y-1].Initialize(player.Value, player.Key);
        }
    }
    public void SwitchToPosition(Vector2Int position)
    {
        x.text = position.x.ToString();
        y.text = position.y.ToString();
         
        OnSearchPressed();
    }
    #endregion

    #region Private Functions
    private void CreatePositions()
    {
        for(int j = 0; j < 30; j++) 
        {
            List<WorldUIPosition> worldUIPositions = new List<WorldUIPosition>();
            GameObject row = Instantiate(WorldRowObject, positionsParent.transform);

            for (int i = 0; i < 30; i++)
            {
                WorldUIPosition worldPosition = Instantiate(WorldPositionInstance, row.transform).GetComponent<WorldUIPosition>();
                
                worldUIPositions.Add(worldPosition);

                worldPosition.myButton.onClick.AddListener(OnPlayerPressed);
                worldPosition.cooardinates.text =
                    (j + 1).ToString() + "," +
                    (i + 1).ToString();
            }
            
            WorldUIPositions.Add(worldUIPositions);
        }
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnSearchPressed()
    {
        int currentX = int.Parse(x.text);
        int currentY = int.Parse(y.text);

        Vector2 factor;
        factor.x = ((currentX-1)*1f) / (maxValue.x-1);
        factor.y = ((currentY-1)*1f) / (maxValue.y-1);

        scrollRect.verticalNormalizedPosition = 1f-factor.y;
        scrollRect.horizontalNormalizedPosition = factor.x;
    }
    private void OnMapChanged(Vector2 arg0)
    {
    }
    private void OnXYChanged(string text)
    {
        int currentX = int.Parse(x.text);
        int currentY = int.Parse(y.text);

        x.text = Mathf.Clamp(currentX, 1, maxValue.x).ToString();
        y.text = Mathf.Clamp(currentY, 1, maxValue.y).ToString();
    }
    private void OnBookMarkPressed()
    {
        View.GetViewGlobally<BookMarkUIView>().ShowView();
    }
    private void OnFindMePressed()
    {
        SwitchToPosition(GameManager.DataManager.characterData.worldMapPosition);
    }
    private void OnPlayerPressed()
    {
        WorldUIPosition playerUIPosition = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<WorldUIPosition>();

        View.GetViewGlobally<PlayerUIView>().Initialize(playerUIPosition.characterData, playerUIPosition.PlayerID);
        View.GetViewGlobally<PlayerUIView>().ShowView();
    }
    private void OnMarchingPressed()
    {
        View.GetViewGlobally<MarchingView>().ShowView();
    }
    #endregion

}
