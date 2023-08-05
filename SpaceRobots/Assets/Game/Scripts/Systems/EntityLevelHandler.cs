using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EntityLevelHandler 
{
    #region Public Variables
    public RequirementsUIManager requirementsUIManager;
    public List<TextMeshProUGUI> levelNumberUi;
    public Button UpgradeButton;
    public Button InstantButton;
    public GameObject NormalView;
    public GameObject MaxLevelView;
    public BuildingType buildingType;
    public int currentLevel = 0;
    public List<List<requirement>> levelRequirements;
    public Action OnLevelUpdate;
    public Slider timeSlider;
    public TextMeshProUGUI timeLeft;
    #endregion

    #region Private Variables
    bool currentlyBuilding = false;
    ILevelHolder holder;
    #endregion

    #region Public Functions
    public void Intialize(int currentLevel, ILevelHolder holder)
    {
        this.currentLevel = currentLevel;
        this.holder = holder;
        UpgradeButton.onClick.AddListener(OnBuildPressed);
        InstantButton.onClick.AddListener(OnInstantPressed);

        InstantButton.interactable = false;
    }
    public void SetLevelsData(List<List<requirement>> requirements)
    {
        IntializeData(requirements);
        RefreshUi();
    }
    public IsRequirementsMet CanUpgrade()
    {
        if (currentLevel < levelRequirements.Count)
        {
            return Manager.GameManager.DoIHaveResources(levelRequirements[currentLevel - 1]);
        }
        else
        {
            return new IsRequirementsMet() { isMet = false, isRequirementsMet = new List<IsRequirementMet>() };
        }
    }
    public void RefreshUi()
    {
        if(currentlyBuilding == false)
        {
            IsRequirementsMet requirements = CanUpgrade();
            UpgradeButton.interactable = requirements.isMet;

            foreach (var levelUi in levelNumberUi)
                levelUi.text = currentLevel.ToString();

            if (requirements.isRequirementsMet.Count > 0)
            {
                NormalView.SetActive(true);
                MaxLevelView.SetActive(false);

                requirementsUIManager.OrganizeRequirementsUi(requirements);
            }
            else
            {
                NormalView.SetActive(false);
                MaxLevelView.SetActive(true);
            }
        }
    }

    #endregion

    #region Private Functions
    private void IntializeData(List<List<requirement>> requirements)
    {
        levelRequirements = requirements;
    }
    private void SetBuildingLevel(int i)
    {
        holder.SetLevel(i);
    }
    private IEnumerator Update()
    {
        currentlyBuilding = true;
        UpgradeButton.interactable = false;
        timeSlider.maxValue = 20f;
        timeSlider.value = 20;
        
        while(timeSlider.value > 0)
        {
            timeLeft.text = ((int)(timeSlider.value+1)).ToString() + " s";
            timeSlider.value -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        
            if(Manager.DataManager.characterData.GetVIPLevel() + 6 > timeSlider.value)
                InstantButton.interactable = true;
        }

        timeLeft.text = "";
        currentLevel++;
        SetBuildingLevel(currentLevel);
        Manager.PlayFabManager.OnDataSent += OnDataSent;
        Manager.PlayFabManager.SendPlayerData();
        OnLevelUpdate?.Invoke();
        
        InstantButton.interactable = false;
        currentlyBuilding = false;

        RefreshUi();
    }
    #endregion 

    #region CallBacks and Coroutines
    private void OnBuildPressed()
    {
        IsRequirementsMet requirements = CanUpgrade();

        if (requirements.isMet)
        {
            Manager.GameManager.SpendResources(requirements);
            GameManager.instance.StartCoroutine(Update());
        }
    }
    private void OnInstantPressed()
    {
        timeSlider.value = 0;
        InstantButton.interactable = false;
    }
    private void OnDataSent()
    {
        Manager.PlayFabManager.OnDataSent -= OnDataSent;
        View.GetViewGlobally<LoadingView>().HideView();
    }
    #endregion
}
