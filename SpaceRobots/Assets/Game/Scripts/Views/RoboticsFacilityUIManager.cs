using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoboticsFacilityUIManager : BuildingButton
{
    #region public variables
    public UITabsManager UITabsManager;

    public Button instant;
    public Button createFVHunter1;
    public GameObject progressListParentObject;
    public List<RobotUIProgress> progressList;
    public int MaxBuildTime = 30;
    #endregion

    #region private variables
    private List<List<requirement>> robotsRequirements;
    #endregion

    #region Unity
    protected override void Awake()
    {
        base.Awake();

        UITabsManager.Initialize();
        UITabsManager.OnSelectedChanged += OnSelectedChanged;
        createFVHunter1.onClick.AddListener(OnF1HunterHunterPressed);
        levelHandler.OnLevelUpdate += OnUpgradePressed;
    }
    public void Update()
    {
        if(progressList.Count > 0)
        {
            progressList[0].ReduceTime(Time.deltaTime);

            if (progressList[0].time > 0)
            {
                progressList[0].TimeSlider.value = progressList[0].time;
            }
            else
            {
                if (progressList[0].isTimeComplete == false)
                {
                    progressList[0].OnTimeComplete();
                }
            }
        }
    }
    #endregion

    #region private functions
    protected override void LoadAndApplyData()
    {
        base.LoadAndApplyData();

        levelHandler.SetLevelsData(GetUpgradeInfo());
        IntializeData();
    }
    private void IntializeData()
    {
        robotsRequirements = new List<List<requirement>>();

        //Robots F1 Hunter requirements
        robotsRequirements.Add(
            new List<requirement>()
            {
                new ResourceRequirement () { requirementType = RequirementType.Resource, resourceType = ResourceType.Iron, requirementAmount = 1} ,
                new ResourceRequirement () { requirementType= RequirementType.Resource, resourceType= ResourceType.Oil, requirementAmount = 6 }
            }
        );
    }
    private List<List<requirement>> GetUpgradeInfo()
    {
       var upgradeListRequirements =  new List<List<requirement>>();

        for (int i = 0; i < 30; i++)
        {
            upgradeListRequirements.Add(
                new List<requirement>()
                {
                    new BuildingRequirement()
                    {
                        requirementType = RequirementType.Building,
                        buildingType = BuildingType.RoboticsCenter,
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
    private float GetTimeToComplete()
    {
        return ((MaxBuildTime * 1.0f) / (levelHandler.currentLevel * 1.0f));
    }
    #endregion

    #region CallBacks
    private void OnF1HunterHunterPressed()
    {
        View.GetViewGlobally<CreationPopUpView>().ShowView(robotsRequirements[0], "FV Hunter A1", "Manufacture Time 3s", OnRobotsBuildRequested);
    }
    private void OnSelectedChanged()
    {
        if (UITabsManager.selected == 0)
        {
            instant.gameObject.SetActive(true);
            levelHandler.UpgradeButton.gameObject.SetActive(true);
        }
        else
        {
            instant.gameObject.SetActive(false);
            levelHandler.UpgradeButton.gameObject.SetActive(false);
        }
    }
    private void OnRobotsBuildRequested(int amount)
    {
        int i = amount;

        while(i != 0)
        {
            OnF1HunterHunterBuildPressed();
            i--;
        }
    }
    private void OnF1HunterHunterBuildPressed()
    {
        View.GetViewGlobally<CreationPopUpView>().HideView();
        
        UITabsManager.SetSelected(2);

        RobotUIProgress robotInProgress= 
            Instantiate(Resources.Load<GameObject>("Prefabs/Robot In Progress"),
                        progressListParentObject.transform).GetComponent<RobotUIProgress>();

        float timeToComplete = GetTimeToComplete();

        robotInProgress.Initialize(
           () => {
               progressList.Remove(robotInProgress);
               Destroy(robotInProgress.gameObject);
           },
           () => {
               progressList.Remove(robotInProgress);
               Destroy(robotInProgress.gameObject);
           },
           OnF1HunterBuildTimeCompelte,
           timeToComplete);
        
        progressList.Add(robotInProgress);

        IsRequirementsMet isRequirementsMet = Manager.GameManager.DoIHaveResources(robotsRequirements[0]);
        Manager.GameManager.SpendResources(isRequirementsMet);
    }
    private void OnF1HunterBuildTimeCompelte(RobotUIProgress robotInProgress)
    {
        Manager.DataManager.characterData.robotsIHave[RobotsNames.F1Hunter]++;
        Manager.PlayFabManager.SendPlayerData();
        
        if (BaseRobotsViewer.Instance != null)
            BaseRobotsViewer.Instance.RefereshParkedRobots();


        progressList.Remove(robotInProgress);
        Destroy(robotInProgress.gameObject);
    }
    private void OnUpgradePressed()
    {
        foreach(var progress in progressList)
        {
            float newMaxTime = GetTimeToComplete();
            float finishedPercent = progress.time / progress.TimeSlider.maxValue;

            progress.time = finishedPercent * newMaxTime;
            progress.TimeSlider.maxValue = newMaxTime;
        }
    }
    #endregion
}
