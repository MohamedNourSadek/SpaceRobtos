using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailResultView : View
{
    #region Public Variables
    public Button outsideClickable;
    public Button otherCloseButton;
    public TextMeshProUGUI emailText;
    public TextMeshProUGUI playerName;
    public Button playerCharacterButton;
    public Image playerCharacter;
    public CharacterData senderData;
    public string senderID;

    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        outsideClickable.onClick.AddListener(HideView);
        otherCloseButton.onClick.AddListener(HideView);
        playerCharacterButton.onClick.AddListener(OnPlayerPressed);
    }

    #endregion

    #region Public Functions
    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines
    private void OnPlayerPressed()
    {
        View.GetViewGlobally<OtherProfileView>().characterData = senderData;
        View.GetViewGlobally<OtherProfileView>().playerID = senderID;

        View.GetViewGlobally<OtherProfileView>().ShowView();
        HideView();
    }
    #endregion

}
