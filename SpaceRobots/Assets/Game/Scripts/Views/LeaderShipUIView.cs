using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderShipUIView : View
{
    #region Public Variables
    public Button IncreaseButton;
    public Button Increase10Times;
    public TextMeshProUGUI OwnedChips;
    public TextMeshProUGUI LeadershipAmount;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        IncreaseButton.onClick.AddListener(OnIncreasePressed);
        Increase10Times.onClick.AddListener(OnIncrease10Pressed);
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
        LeadershipAmount.text = "Leadership " + GameManager.DataManager.characterData.leadershipAmount.ToString();
        OwnedChips.text = "Owned: x " + GameManager.DataManager.characterData.chips.ToString();
        Increase10Times.interactable = GameManager.DataManager.characterData.chips >= 10;
        IncreaseButton.interactable = GameManager.DataManager.characterData.chips > 0;
    }
    #endregion

    #region Private Functions
    private bool GetRandomPossibility()
    {
        float randomNum = Random.Range(0f, 1f);

        return (randomNum <= 0.4f);
    }
    private int GetRandomReward()
    {
        float randomNum = Random.Range(0f, 1.01f);

        if (randomNum >= .99f)
            return 24;
        else if (randomNum >= 0.97f)
            return 22;
        else if (randomNum >= 0.95f)
            return 20;
        else if (randomNum >= 0.90f)
            return 18;
        else if (randomNum >= 0.85f)
            return 16;
        else if (randomNum >= 0.75f)
            return 14;
        else if (randomNum >= 0.55f)
            return 12;
        else if (randomNum >= 0.35f)
            return 10;
        else if (randomNum >= 0.15f)
            return 8;
        else if (randomNum >= 0.5f)
            return 6;
        else
            return 4;
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnIncreasePressed()
    {
        GameManager.DataManager.characterData.chips--;
        IncreaseButton.interactable = false;

        if (GetRandomPossibility())
        {
            GameManager.DataManager.characterData.leadershipAmount+=2;
            View.GetViewGlobally<MessageUIView>().ShowView("Leadership Gained", "You've got 2 leadership point", OnIncreaseOkPressed);
        }
        else
        {
            View.GetViewGlobally<MessageUIView>().ShowView("Leadership Gained", "You've got No leadership points", OnIncreaseOkPressed);
        }
        
        GameManager.PlayFabManager.SendPlayerData();
    }
    private void OnIncrease10Pressed()
    {
        GameManager.DataManager.characterData.chips -= 10;
        Increase10Times.interactable = false; 

        int randomReward = GetRandomReward();
            
        GameManager.DataManager.characterData.leadershipAmount+= randomReward;
        View.GetViewGlobally<MessageUIView>().ShowView("Leadership Gained", "You've got " + randomReward + " leadership point", OnIncreaseOkPressed);

        GameManager.PlayFabManager.SendPlayerData();
    }

    private void OnIncreaseOkPressed()
    {
        View.GetViewGlobally<MessageUIView>().HideView();
        RefreshUI();
    }
    #endregion

}
