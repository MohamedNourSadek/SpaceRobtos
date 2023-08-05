using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailUIItem : MonoBehaviour
{
    public TextMeshProUGUI EmailName;
    public Button ShowButton;
    public Button CloseButton;
    public MarchingData reportData;

    private CharacterData myData;
    private string myEmailContent;
    private string senderID;

    public void InitializeAsReport(MarchingData enemyData, CharacterData myData)
    {
        EmailName.text = enemyData.marchingType.ToString() + " Result";
        reportData = enemyData;
        this.myData = myData;
        ShowButton.onClick.AddListener(OnShowPressed);
        CloseButton.onClick.AddListener(OnClosePressed);
    }

    public void InitializeAsEmail(string messsage, CharacterData senderData,string playerID)
    {
        myEmailContent = messsage;
        myData = senderData;
        senderID = playerID;

        EmailName.text = "Email from " + senderData.playerName;
        CloseButton.onClick.AddListener(OnClosePressed);
        ShowButton.onClick.AddListener(OnShowEmailPressed);
    }

    private void OnShowEmailPressed()
    {
        View.GetViewGlobally<EmailResultView>().emailText.text = myEmailContent;
        View.GetViewGlobally<EmailResultView>().playerName.text = myData.playerName;
        View.GetViewGlobally<EmailResultView>().playerCharacter.sprite = Manager.GameManager.GetCharacterImage(myData.characterImage);
        View.GetViewGlobally<EmailResultView>().senderData = myData;
        View.GetViewGlobally<EmailResultView>().senderID = senderID;

        View.GetViewGlobally<EmailResultView>().ShowView();
    }

    private void OnClosePressed()
    {
        Destroy(this.gameObject);
    }

    private void OnShowPressed()
    {
        if(reportData.marchingType == MarchingType.Scout)
        {
            View.GetViewGlobally<ScoutResultUI>().ShowView(reportData.playerData);
        }
        else if(reportData.marchingType == MarchingType.Attack)
        {
            View.GetViewGlobally<AttackResultUI>().ShowView(reportData, myData);
        }
    }
}
