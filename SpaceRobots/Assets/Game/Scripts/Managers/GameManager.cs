using JetBrains.Annotations;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Manager
{
    #region Public Variables
    public static GameManager instance;
    public int LockFrameRate = 60;
    #endregion

    #region Private Variables
    private int currentLogin;
    #endregion

    #region Unity Delgates
    public override void Awake()
    {
        base.Awake();
        Manager.GameManager = this;
        Application.targetFrameRate = LockFrameRate;
        instance = this;

        StartGameLoop();
    }
    #endregion

    #region Public Functions
    public IsRequirementsMet DoIHaveResources(List<requirement> requirements)
    {
        IsRequirementsMet result = new IsRequirementsMet();
        result.isMet = true;
        
        foreach (requirement requirement in requirements)
        {
            IsRequirementMet isRequirementMet = DoIHaveResource(requirement);   
            
            if(!isRequirementMet.isMet) 
                result.isMet = false;

            result.isRequirementsMet.Add(isRequirementMet);
        }

        return result;
    }
    public IsRequirementMet DoIHaveResource(requirement requirement)
    {
        IsRequirementMet result = new IsRequirementMet();
        result.isMet = true;
        result.amountRequired = requirement.requirementAmount;

        if (requirement.requirementType == RequirementType.Resource)
        {
            ResourceRequirement req = (ResourceRequirement) requirement;

            switch (req.resourceType)
            {
                case ResourceType.Gold:
                    {
                        if (DataManager.characterData.goldAmount < req.requirementAmount)
                            result.isMet = false;

                        result.amountIHave = DataManager.characterData.goldAmount;
                        result.name = req.resourceType;
                        break;
                    }
                case ResourceType.Iron:
                    {
                        if (DataManager.characterData.ironAmount < req.requirementAmount)
                            result.isMet = false;

                        result.amountIHave = DataManager.characterData.ironAmount;
                        result.name = req.resourceType;
                        break;
                    }
                case ResourceType.Oil:
                    {
                        if (DataManager.characterData.oilAmount < req.requirementAmount)
                            result.isMet = false;
                        
                        result.amountIHave = DataManager.characterData.oilAmount;
                        result.name = req.resourceType;

                        break;
                    }
                case ResourceType.Titanium:
                    {
                        if (DataManager.characterData.titaniumAmount < req.requirementAmount)
                            result.isMet = false;
                        
                        result.amountIHave = DataManager.characterData.titaniumAmount;
                        result.name = req.resourceType;

                        break;
                    }
                case ResourceType.Uranium:
                    {
                        if (DataManager.characterData.uraniumAmount < req.requirementAmount)
                            result.isMet = false;

                        result.amountIHave = DataManager.characterData.uraniumAmount;
                        result.name = req.resourceType;

                        break;
                    }
            }
        }
        else if (requirement.requirementType == RequirementType.Building)
        {
            BuildingRequirement req = (BuildingRequirement)requirement;
            result.name = req.buildingType;

            BuildingData data = FindHightLevelOfType(req.buildingType);

            if (data == null)
            {
                if (1 < req.requirementAmount)
                    result.isMet = false;

                result.amountIHave = 1;
            }
            else
            {
                if (data.BuildingLevel < req.requirementAmount)
                    result.isMet = false;

                result.amountIHave = data.BuildingLevel;
            }
        }

        return result;
    }
    public void SpendResources(IsRequirementsMet requirements)
    {
        if (requirements.isMet)
        {
            int powerAdded = 0;

            foreach (var requirement in requirements.isRequirementsMet)
            {
                if (requirement.name.ToString() == ResourceType.Gold.ToString())
                    DataManager.characterData.goldAmount -= requirement.amountRequired;
                else if (requirement.name.ToString() == ResourceType.Iron.ToString())
                    DataManager.characterData.ironAmount -= requirement.amountRequired;
                else if (requirement.name.ToString() == ResourceType.Oil.ToString())
                    DataManager.characterData.oilAmount -= requirement.amountRequired;
                else if (requirement.name.ToString() == ResourceType.Titanium.ToString())
                    DataManager.characterData.titaniumAmount -= requirement.amountRequired;
                else if (requirement.name.ToString() == ResourceType.Uranium.ToString())
                    DataManager.characterData.uraniumAmount -= requirement.amountRequired;

                powerAdded += requirement.amountRequired;
            }

            AddPower(powerAdded / 10);
            AddXP(powerAdded * 10);
            AddEnergy(powerAdded * 10);

            View.GetViewGlobally<TopBarView>().RefreshPlayerInformation(Manager.DataManager.characterData);
        }

    }
    public void AddPower(int amount)
    {
        DataManager.characterData.powerNumber += amount;
    }
    public void AddXP(int amount)
    {
        DataManager.characterData.AddXP(amount);
    }
    public void AddEnergy(int amount)
    {
        DataManager.characterData.AddEnergy(amount);
    }
    public Sprite GetCharacterImage(int i)
    {
        Sprite image = Resources.Load<Sprite>("Character Images/Character_" + i.ToString());
        return image;
    }
    public Sprite GetRankImage(int i)
    {
        Sprite image = Resources.Load<Sprite>("Ranks/rank" + i.ToString());
        return image;
    }
    public string GetRankName(int i)
    {
        return "Rank " + i;
    }
    public void InitializePlayerFirstTimeData()
    {
        Manager.PlayFabManager.OnDataSent -= InitializePlayerFirstTimeData;

        try
        {
            PlayFabManager.OnPlayersInfoRetrieved -= OnPlayersInfoRetrieved;
        }
        catch { }
        
        PlayFabManager.OnPlayersInfoRetrieved += OnPlayersInfoRetrieved;
        PlayFabManager.GetAllPlayers();
    }
    public void SaveInfoForFirstTime()
    {
        DataManager.characterData.fullyInitialized = true;
        PlayFabManager.OnDataSent += OnInfoSavedFirstTime;
        PlayFabManager.SendPlayerData();
    }
    #endregion


    #region Private Functions
    private void StartGameLoop()
    {
        PlayFabManager.OnLogin += OnLogin;
        PlayFabManager.Login();
    }
    private BuildingData FindHightLevelOfType(BuildingType type)
    {
        BuildingData result = null;

        foreach(var element in DataManager.characterData.buildingsLevels)
        {
            if (element.Value.BuildingType == type)
            {
                if(result == null) 
                {

                    result = element.Value;
                }
                else
                {
                    if(element.Value.BuildingLevel > result.BuildingLevel)
                    {
                        result = element.Value;
                    }
                }
            }
        }

        return result;
    }
    private void HandleLoginStreakCount()
    {
        if (DataManager.characterData.LastLogin == currentLogin)
        {
            //Do nothing if logged in today
        }
        else if (DataManager.characterData.LastLogin == currentLogin - 1)
        {
            //last login was yesterday
            DataManager.characterData.numberOfDaysLoggedInRow++;
            DataManager.characterData.AddVIPPoints(DataManager.characterData.numberOfDaysLoggedInRow);
        }
        else
        {
            //last login was before yesterday
            DataManager.characterData.numberOfDaysLoggedInRow = 0;
        }
        DataManager.characterData.LastLogin = currentLogin;
    }
    private void IntializePlayerLocation(Dictionary<string, CharacterData> myPlayers)
    {
        bool GeneratedUniquePoint = false;

        while (GeneratedUniquePoint == false)
        {
            Vector2Int newPosition = new Vector2Int();

            newPosition.x = Random.Range(1, WorldUIView.maxValue.x + 1);
            newPosition.y = Random.Range(1, WorldUIView.maxValue.y + 1);

            bool pointExists = false;

            foreach (var player in myPlayers)
            {
                if (player.Value.worldMapPosition == newPosition)
                {
                    pointExists = true;
                }
            }

            if (pointExists == false)
            {
                GeneratedUniquePoint = true;
                DataManager.characterData.worldMapPosition = newPosition;
                PlayFabManager.SendPlayerData();
            }
        }
    }
    private void LoadMainMenu()
    {
        Manager.ScenesManager.LoadScene(SceneName.MainMenu, OnMainMenuLoaded, 2f);
    }
    #endregion

    #region CallBacks and Coroutines
    public void OnLevelIncrease()
    {
        Debug.Log("Level Up");
        DataManager.characterData.scorePoints += 3;
    }
    private void OnPlayersInfoRetrieved(Dictionary<string, CharacterData> myPlayers)
    {
        PlayFabManager.OnPlayersInfoRetrieved -= OnPlayersInfoRetrieved;

        if(myPlayers.Count == 0)
        {
            if(GameManager.DataManager.otherPlayersInCurrentMap.ContainsKey("Me") == false)
            {
                GameManager.DataManager.otherPlayersInCurrentMap.Add("Me",GameManager.DataManager.characterData);
            }
        }
        else
        {
            GameManager.DataManager.otherPlayersInCurrentMap = myPlayers;
        }

        if (GameManager.DataManager.characterData.fullyInitialized == false)
        {
            IntializePlayerLocation(myPlayers);
            SaveInfoForFirstTime();
        }
        else
        {
            LoadMainMenu();
            View.GetViewGlobally<LoadingView>().ShowView();
        }

    }
    private void OnLogin(LoginResult result)
    {
        PlayFabManager.OnLogin -= OnLogin;
        
        if(result.LastLoginTime != null)
            currentLogin = result.LastLoginTime.Value.Day;

        PlayFabManager.OnDataRetrieved += OnDataRetrieved;
        PlayFabManager.GetPlayerData();
    }
    private void OnDataRetrieved()
    {
        PlayFabManager.OnDataRetrieved -= OnDataRetrieved;

        if (DataManager.characterData != null && DataManager.characterData.fullyInitialized)
        {
            View.GetViewGlobally<LoadingView>().ShowView();
            HandleLoginStreakCount();
            PlayFabManager.OnPlayersInfoRetrieved += OnPlayersInfoRetrieved;
            PlayFabManager.GetAllPlayers();
        }
        else
        {
            View.GetViewGlobally<IntroUIManager>().ShowView();
        }

        PlayFabManager.SendPlayerData();
    }
    private void OnInfoSavedFirstTime()
    {
        Manager.PlayFabManager.OnDataSent -= OnInfoSavedFirstTime;

        if (DataManager.characterData.fullyInitialized)
        {
            LoadMainMenu();
        }
    }
    private void OnMainMenuLoaded()
    {
        View.GetViewGlobally<LoadingView>().HideView();
    }
    #endregion
}
