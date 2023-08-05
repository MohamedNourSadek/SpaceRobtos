using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileUIView : View
{
    public UITabsManager tabs;
    
    public Button Reset;
    public Image CharacterImage;
    public Image RankImage;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI RankName;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI PowerText;
    public TextMeshProUGUI Leadership_Level;
    public TextMeshProUGUI scorePoints;
    public Slider LevelSlider;
    public Slider EnergySlider;
    public Button LeadershipButton;
    public Button RankButton;

    [Header("Skills UI")]
    public List<Button> skillUpgradeButtons;
    public TextMeshProUGUI accuracyLevel;
    public TextMeshProUGUI accuracyPercent;
    public TextMeshProUGUI interferenceLevel;
    public TextMeshProUGUI interferencePercent;
    public TextMeshProUGUI criticalHitLevel;
    public TextMeshProUGUI criticalHitPercent;
    public TextMeshProUGUI tenacityLevel;
    public TextMeshProUGUI tenacityPercent;
    public TextMeshProUGUI fireOutbreakLevel;
    public TextMeshProUGUI fireOutbreakPercent;

    #region Public Variables

    #endregion

    #region Private Variables
    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        tabs.Initialize();

        foreach(Button button in skillUpgradeButtons)
            button.onClick.AddListener(OnUpgradeSkillButton);

        Reset.onClick.AddListener(OnResetSkillButton);
        LeadershipButton.onClick.AddListener(OnLeaderShipButton);
        RankButton.onClick.AddListener(OnRankPressed);
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
        RefereshUI();

        if (GetComponentInChildren<SocialUIView>())
            GetComponentInChildren<SocialUIView>().RefereshUI();
    }
    public void RefereshUI()
    {
        CharacterData characterData = GameManager.DataManager.characterData;

        PlayerName.text = characterData.playerName;
        scorePoints.text = "Points avaiable = " + characterData.scorePoints.ToString();
        LevelText.text = "Lv" + (characterData.xp / CharacterData.MAX_XP).ToString();

        LevelSlider.maxValue = CharacterData.MAX_XP;
        EnergySlider.maxValue = CharacterData.MAX_ENERGY;

        LevelSlider.value = characterData.xp - ((characterData.xp / CharacterData.MAX_XP) * CharacterData.MAX_XP);
        EnergySlider.value = characterData.energyAmount;

        CharacterImage.sprite = GameManager.instance.GetCharacterImage(characterData.characterImage);
        RankImage.sprite = GameManager.instance.GetRankImage(characterData.GetRank());

        PowerText.text = characterData.powerNumber.ToString();
        Leadership_Level.text = "Lv" + characterData.leadershipAmount.ToString();
        RankName.text = GameManager.instance.GetRankName(characterData.GetRank());
        accuracyLevel.text = "Lvl " + characterData.accuracy.ToString();
        interferenceLevel.text = "Lvl " + characterData.interference.ToString();
        criticalHitLevel.text = "Lvl " + characterData.criticalHit.ToString();
        tenacityLevel.text = "Lvl " + characterData.tenacity.ToString();
        fireOutbreakLevel.text = "Lvl " + characterData.fireOutbreak.ToString();

        accuracyPercent.text = "Increase " + characterData.accuracy.ToString() + "% hit.";
        interferencePercent.text = "Increase " + characterData.interference.ToString() + "% dodge.";
        criticalHitPercent.text = "Increase " + characterData.criticalHit.ToString() + "% critical hit.";
        tenacityPercent.text = "Increase " + characterData.tenacity.ToString() + "% tenacity.";
        fireOutbreakPercent.text = "Increase " + characterData.fireOutbreak.ToString() + "% gun damage.";

        foreach(var button in skillUpgradeButtons)
            button.interactable = Manager.DataManager.characterData.scorePoints > 0;
    }

    #endregion


    #region Private Functions
    #endregion

    #region CallBacks and Coroutines
    private void OnUpgradeSkillButton()
    {
        if(GameManager.DataManager.characterData.scorePoints > 0)
        {
            int selectedButtonIndex = 0;

            for (int i = 0; i < skillUpgradeButtons.Count; i++)
                if (EventSystem.current.currentSelectedGameObject == skillUpgradeButtons[i].gameObject)
                    selectedButtonIndex = i;

            if (selectedButtonIndex == 0)
                Manager.DataManager.characterData.accuracy++;
            else if (selectedButtonIndex == 1)
                Manager.DataManager.characterData.interference++;
            else if (selectedButtonIndex == 2)
                Manager.DataManager.characterData.criticalHit++;
            else if (selectedButtonIndex == 3)
                Manager.DataManager.characterData.tenacity++;
            else if (selectedButtonIndex == 4)
                Manager.DataManager.characterData.fireOutbreak++;

            Manager.DataManager.characterData.scorePoints--;

            RefereshUI();
            Manager.PlayFabManager.SendPlayerData();
        }
    }
    private void OnResetSkillButton()
    {
        int overAllSkill = 0;

        overAllSkill += Manager.DataManager.characterData.accuracy;
        overAllSkill += Manager.DataManager.characterData.interference;
        overAllSkill += Manager.DataManager.characterData.criticalHit;
        overAllSkill += Manager.DataManager.characterData.tenacity;
        overAllSkill += Manager.DataManager.characterData.fireOutbreak;

        Manager.DataManager.characterData.scorePoints += overAllSkill;
        Manager.DataManager.characterData.accuracy = 0;
        Manager.DataManager.characterData.interference = 0;
        Manager.DataManager.characterData.criticalHit = 0;
        Manager.DataManager.characterData.tenacity = 0;
        Manager.DataManager.characterData.fireOutbreak = 0;

        Manager.PlayFabManager.SendPlayerData();

        RefereshUI();
    }
    private void OnLeaderShipButton()
    {
        View.GetViewGlobally<LeaderShipUIView>().ShowView();
    }
    private void OnRankPressed()
    {
        View.GetViewGlobally<RankUIView>().ShowView();
    }

    #endregion

}
