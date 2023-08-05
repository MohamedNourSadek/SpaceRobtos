using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ResourceStructureView : BuildingButton
{
    public float TimeAdded = 30;

    [Header("Not Panel References")]
    public GameObject BuiltPanel;
    public Button IronFactoryBuildButton;
    public Button GoldFactoryBuildButton;
    public Button OilFactoryBuildButton;
    public Button TitaniumFactoryBuildButton;
    public Button UraniumFactoryBuildButton;
    
    [Header("Built Panel References")]
    public GameObject NotBuiltPanel;
    public Image BuildingSprite;     
    public GameObject BuildingNumber;
    public Button DestroyButton;
    public TextMeshProUGUI resourceName;
    public TextMeshProUGUI producingEvery;

    #region Public Variables

    #endregion

    #region Private Variables
    protected BuildingType toBeBuilt;
    protected bool ShowOnBuild = true;
    private bool isBuilt;
    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        DestroyButton.onClick.AddListener(OnDestroyFactory);
        
        levelHandler.OnLevelUpdate += OnLevelUpgrade;
        IronFactoryBuildButton.onClick.AddListener(OnIronFactoryBuildRequest);
        GoldFactoryBuildButton.onClick.AddListener(OnGoldFactoryBuildRequest);
        OilFactoryBuildButton.onClick.AddListener(OnOilFactoryBuildRequest);
        TitaniumFactoryBuildButton.onClick.AddListener(OnTitaniumFactoryBuildRequest);
        UraniumFactoryBuildButton.onClick.AddListener(OnUraniumFactoryBuildRequest);
    }

    #endregion

    #region Public Functions
    public override void ShowView()
    {
        RefreshMode();
        base.ShowView();
    }
    protected override void LoadAndApplyData()
    {
        base.LoadAndApplyData();

        var levelStored = GameManager.DataManager.characterData.buildingsLevels.ContainsKey(MyID);
        
        if (levelStored)
        {
            isBuilt = true;
            
            levelHandler.currentLevel = GameManager.DataManager.characterData.buildingsLevels[MyID].BuildingLevel;
            levelHandler.buildingType = GameManager.DataManager.characterData.buildingsLevels[MyID].BuildingType;
            OnLevelUpgrade();

            StartCoroutine(AddResourceOverTime());
        }

        RefreshMode();
    }
    #endregion

    #region Private Functions
    private void RefreshMode()
    {
        if (isBuilt)
        {
            BuiltPanel.SetActive(true);
            NotBuiltPanel.SetActive(false);

            BuildingSprite.gameObject.SetActive(true);
            BuildingNumber.gameObject.SetActive(true);
            levelHandler.levelNumberUi[1].gameObject.SetActive(true); //level header
           
            resourceName.text = levelHandler.buildingType + " Factory";
            BuildingSprite.sprite = GameManager.DataManager.GetSprite(levelHandler.buildingType.ToString() + " Factory", BuildingSprite);
        }
        else
        {
            BuiltPanel.SetActive(false);
            NotBuiltPanel.SetActive(true);

            BuildingNumber.gameObject.SetActive(false) ;
            BuildingSprite.gameObject.SetActive(false);
            levelHandler.levelNumberUi[1].gameObject.SetActive(false); //level header

            resourceName.text = "Resource Structure";
        }

        var myUpgrades = GetUpgradeInfo(levelHandler.buildingType);
        levelHandler.SetLevelsData(myUpgrades);
    }
    private List<List<requirement>> GetUpgradeInfo(BuildingType type)
    {
        var upgradeListRequirements = new List<List<requirement>>();

        for (int i = 0; i < 30; i++)
        {
            upgradeListRequirements.Add(
                new List<requirement>()
                {
                    new BuildingRequirement()
                    {
                        requirementType = RequirementType.Building,
                        buildingType = type,
                        requirementAmount = i+1,
                    },
                    new ResourceRequirement()
                    {
                        requirementType = RequirementType.Resource,
                        resourceType = ResourceType.Iron,
                        requirementAmount = 6 + i
                    },
                    new ResourceRequirement()
                    {
                        requirementType = RequirementType.Resource,
                        resourceType = ResourceType.Oil,
                        requirementAmount = 4 + i
                    },
                    new ResourceRequirement()
                    {
                        requirementType = RequirementType.Resource,
                        resourceType = ResourceType.Titanium,
                        requirementAmount = 3 + i
                    }
                }
            );
        }

        return upgradeListRequirements;
    }
    private IEnumerator AddResourceOverTime()
    {
        while (isBuilt)
        {
            yield return new WaitForSeconds((float)GetTimeToProduce());

            if (isBuilt)
            {
                float percent =  1 + ((GameManager.DataManager.characterData.GetVIPLevel() + 4)/100f);
                int increarement = (int)(percent * 50);

                if (levelHandler.buildingType == BuildingType.Iron)
                    GameManager.DataManager.characterData.ironAmount += increarement;
                else if (levelHandler.buildingType == BuildingType.Gold)
                    GameManager.DataManager.characterData.goldAmount += increarement;
                else if (levelHandler.buildingType == BuildingType.Oil)
                    GameManager.DataManager.characterData.oilAmount += increarement;
                else if (levelHandler.buildingType == BuildingType.Titanium)
                    GameManager.DataManager.characterData.titaniumAmount += increarement;
                else if (levelHandler.buildingType == BuildingType.Uranium)
                    GameManager.DataManager.characterData.uraniumAmount += increarement;

                Debug.Log(increarement +  " of " + levelHandler.buildingType.ToString() + " is added ");

                View.GetViewGlobally<FloatingMessage>().ShowMessage("+ " + levelHandler.buildingType + " Added");

                View.GetViewGlobally<TopBarView>().RefreshPlayerInformation(GameManager.DataManager.characterData);

                GameManager.PlayFabManager.SendPlayerData();
            }

        }
    }
    private double GetTimeToProduce()
    {
        return Math.Round((TimeAdded / levelHandler.currentLevel), 2);
    }
    #endregion


    #region CallBacks and Coroutines
    private void OnIronFactoryBuildRequest()
    {
        toBeBuilt = BuildingType.Iron;

        List<requirement> requirements = new List<requirement>()
        {
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Iron, requirementAmount = 2 },
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Oil, requirementAmount = 4 }
        };

        View.GetViewGlobally<CreationPopUpView>().ShowView(requirements, "Iron Factory", "Building Iron Factory", OnBuildingBuilt, false);
    }
    private void OnGoldFactoryBuildRequest()
    {
        toBeBuilt = BuildingType.Gold;

        List<requirement> requirements = new List<requirement>()
        {
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Iron, requirementAmount = 2 },
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Oil, requirementAmount = 4 }
        };

        View.GetViewGlobally<CreationPopUpView>().ShowView(requirements, "Gold Factory", "Building Gold Factory", OnBuildingBuilt, false);
    }
    private void OnOilFactoryBuildRequest()
    {
        toBeBuilt = BuildingType.Oil;

        List<requirement> requirements = new List<requirement>()
        {
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Iron, requirementAmount = 2 },
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Oil, requirementAmount = 4 }
        };

        View.GetViewGlobally<CreationPopUpView>().ShowView(requirements, "Oil Factory", "Building Oil Factory", OnBuildingBuilt, false);
    }
    private void OnTitaniumFactoryBuildRequest()
    {
        toBeBuilt = BuildingType.Titanium;

        List<requirement> requirements = new List<requirement>()
        {
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Iron, requirementAmount = 2 },
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Oil, requirementAmount = 4 }
        };

        View.GetViewGlobally<CreationPopUpView>().ShowView(requirements, "Titanium Factory", "Building Titanium Factory", OnBuildingBuilt, false);
    }
    private void OnUraniumFactoryBuildRequest()
    {
        toBeBuilt = BuildingType.Uranium;

        List<requirement> requirements = new List<requirement>()
        {
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Iron, requirementAmount = 2 },
            new ResourceRequirement() {requirementType = RequirementType.Resource, resourceType = ResourceType.Oil, requirementAmount = 4 }
        };

        View.GetViewGlobally<CreationPopUpView>().ShowView(requirements, "Uranium Factory", "Building Uranium Factory", OnBuildingBuilt, false);
    }

    protected void OnBuildingBuilt(int i = 0) // you can only build 1 everytime
    {
        isBuilt = true;

        BuildingData data = new BuildingData()
        {
            BuildingLevel = 1,
            BuildingType = toBeBuilt
        };

        levelHandler.currentLevel = data.BuildingLevel;
        levelHandler.buildingType = data.BuildingType;

        if(GameManager.DataManager.characterData.buildingsLevels.ContainsKey(MyID) == false)
            GameManager.DataManager.characterData.buildingsLevels.Add(MyID, data);

        OnLevelUpgrade();
        StartCoroutine(AddResourceOverTime());

        GameManager.PlayFabManager.SendPlayerData();
        if(ShowOnBuild)
            ShowView();
    }
    private void OnLevelUpgrade()
    {
        StopAllCoroutines();
        producingEvery.text = "Producing every " + (GetTimeToProduce()).ToString() + " S";
        StartCoroutine(AddResourceOverTime());
    }
    private void OnDestroyFactory()
    {
        isBuilt = false;
        levelHandler.buildingType = BuildingType.ResourcesPlatform;
        levelHandler.currentLevel = 1;

        RefreshMode();

        if (GameManager.DataManager.characterData.buildingsLevels.ContainsKey(MyID))
        {
            GameManager.DataManager.characterData.buildingsLevels.Remove(MyID);
            GameManager.PlayFabManager.SendPlayerData();
        }

        ShowView();
        StopCoroutine(AddResourceOverTime());
    }
    #endregion

}
