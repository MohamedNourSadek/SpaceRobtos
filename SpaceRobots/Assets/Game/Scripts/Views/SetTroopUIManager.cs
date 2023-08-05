using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTroopUIManager : View
{
    #region Public Variables
    public AddTroopUI AddTroopUI;
    public Slider TroopsAmount;
    public Button Confirm;
    #endregion

    #region Private Variables
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();

        TroopsAmount.onValueChanged.AddListener(RefereshItems);
        Confirm.onClick.AddListener(OnConfirmPressed);
    }

    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
        RefereshItems(0);
    }

    public void RefereshItems(float amount)
    {
        TroopsAmount.minValue = 1;
        TroopsAmount.maxValue = Manager.DataManager.characterData.robotsIHave[RobotsNames.F1Hunter];
        AddTroopUI.TroopNumber.text = TroopsAmount.value.ToString();
    }
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines
    private void OnConfirmPressed()
    {
        Manager.DataManager.characterData.robotsInTroops[RobotsNames.F1Hunter] = (int)TroopsAmount.value;
        HideView();
        View.GetViewGlobally<ExpeditionUIManager>().ShowView();
        GameManager.PlayFabManager.SendPlayerData();
    }
    #endregion

}
