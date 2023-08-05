using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroUIManager : View
{
    #region Public Variables
    public CharacterSelect characterSelect;
    public Button CreateProfileButton;
    public TextMeshProUGUI ValidatorText;
    public TMP_InputField InputName;
    #endregion

    #region Unity callbacks
    protected override void Start()
    {
        base.Start();
        CreateProfileButton.onClick.AddListener(OnCreateProfilePressed);
        InputName.onValueChanged.AddListener(OnNameInputValueChanged);
    }
    #endregion

    #region callbacks and Coroutine
    private void OnCreateProfilePressed()
    {
        int selectedCharacter = characterSelect.GetSelectedCharacter();

        Manager.DataManager.characterData.characterImage = selectedCharacter;
        Manager.DataManager.characterData.playerName = InputName.text;
        
        View.GetViewGlobally<LoadingView>().ShowView();
        HideView();

        Manager.PlayFabManager.OnDataSent += Manager.GameManager.InitializePlayerFirstTimeData;
        Manager.PlayFabManager.SendPlayerData();
    }
    private void OnNameInputValueChanged(string inputText)
    {
        if(inputText.Replace(" ","").Length >= 2)
        {
            CreateProfileButton.interactable = true;
            ValidatorText.text = "";
        }
        else
        {
            CreateProfileButton.interactable = false;
            ValidatorText.text = "Name must be 2 letters or more";
        }
    }
    #endregion
}
