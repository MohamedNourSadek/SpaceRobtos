using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoutUIView : View
{
    #region Public Variables
    public Button ClickableBackground;
    public CharacterData character;
    public string playerID;
    public Button Scout;
    public Button Cancel;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI HelpText;
    #endregion

    #region Private Variables
    public static int cost = 1000;
    public static int scoutWaitTime = 30;
    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        ClickableBackground.onClick.AddListener(HideView);
        Scout.onClick.AddListener(OnScoutPressed);
        Cancel.onClick.AddListener(HideView);
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        RefreshUI();
        base.ShowView();
    }
    #endregion

    #region Private Functions
    private void RefreshUI()
    {
        CostText.text = cost.ToString() + " Gold";
        Scout.interactable = (cost <= Manager.DataManager.characterData.goldAmount);
        
        if(Scout.interactable)
        {
            HelpText.text = "";
        }
        else
        {
            HelpText.text = "You need " + (cost - Manager.DataManager.characterData.goldAmount).ToString() + " more to scout.";
        }
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnScoutPressed()
    {
        HideView();

        bool exists = Manager.DataManager.marchingQueue.Find(d => d.playerId == playerID) != null;

        if(exists == false)
        {
            MarchingData activityData = new MarchingData()
            {
                marchingType = MarchingType.Scout,
                playerData = character,
                playerId = playerID
            }; 

            Manager.DataManager.characterData.goldAmount -= cost;
            Manager.DataManager.marchingQueue.Add(activityData);
            View.GetViewGlobally<MarchingView>().AddItem(activityData);
        }
    }

    #endregion

}

[System.Serializable]
public class MarchingData : IEquatable<MarchingData>
{
    public MarchingType marchingType;
    public CharacterData playerData;
    public string playerId;

    public bool Equals(MarchingData other)
    {
        if(playerId == other.playerId)
            return true;
        else 
            return false;
    }
}

public enum MarchingType
{
    Scout, Attack
}
