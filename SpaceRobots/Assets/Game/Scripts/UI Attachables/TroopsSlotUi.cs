using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopsSlotUi : MonoBehaviour
{
    public GameObject emptyUi;
    public GameObject UsedUI;
    public Image TroopsIcon;
    public TextMeshProUGUI TroopsName;
    public TextMeshProUGUI Amount;
    public Button Remove;

    RobotsNames myRobot;

    private void Start()
    {
        Remove.onClick.AddListener(RemoveTroop);
    }

    public void RemoveTroop()
    {
        Manager.DataManager.characterData.robotsInTroops.Remove(myRobot);
        View.GetViewGlobally<ExpeditionUIManager>().RefereshTroops();
        Manager.PlayFabManager.SendPlayerData();
    }
    public void SetTroops(RobotsNames name, float  amount)
    {
        Amount.text = amount.ToString();
        myRobot = name;
        emptyUi.SetActive(false);
        UsedUI.SetActive(true);

    }

    public void SetState(bool state)
    {
        UsedUI.SetActive(state);
        emptyUi.SetActive(!state);
    }
}
