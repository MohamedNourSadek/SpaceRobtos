using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackResultUI : View
{
    #region Public Variables
    public Button BackgroundClickable;
    public Button closeButton2;
    public TextMeshProUGUI winLoseText;
    public TextMeshProUGUI troopsLossText;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        BackgroundClickable.onClick.AddListener(HideView);
        closeButton2.onClick.AddListener(HideView);
    }
    #endregion

    #region Public Functions
    public void ShowView(MarchingData enemyData, CharacterData myData)
    {
        RefreshUI(enemyData.playerData, myData);
        ShowView();
    }
    public void RefreshUI(CharacterData enemyData, CharacterData myData)
    {
        int troopsCompare = myData.robotsIHave[RobotsNames.F1Hunter] - enemyData.robotsIHave[(RobotsNames.F1Hunter)];

        if (troopsCompare > 0)
        {
            winLoseText.text = "Win!";
            troopsLossText.text = "You've Lost The Following Troops: \n - " + enemyData.robotsIHave[(RobotsNames.F1Hunter)] + " " + RobotsNames.F1Hunter.ToString();
        }
        else if (troopsCompare < 0)
        {
            winLoseText.text = "Lost!";
            troopsLossText.text = "You've Lost The Following Troops: \n - " + myData.robotsIHave[RobotsNames.F1Hunter] + " " + RobotsNames.F1Hunter.ToString();
        }
        else
        {
            winLoseText.text = "Draw!";
            troopsLossText.text = "You've Lost The Following Troops: \n - " + myData.robotsIHave[RobotsNames.F1Hunter] + " " + RobotsNames.F1Hunter.ToString();
        }
    }
    #endregion

    #region Private Functions
    #endregion

    #region CallBacks and Coroutines
    #endregion

}
