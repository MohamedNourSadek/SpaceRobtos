using PlayFab.MultiplayerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : View, ILevelHolder
{
    #region Public Variables
    public int MyID = 0;
    public EntityLevelHandler levelHandler = new EntityLevelHandler();
    public Button myButton;
    #endregion

    #region Private Variables
    static List<BuildingButton> myBuildings = new List<BuildingButton>();
    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        if(myBuildings.Contains(this) == false)
            myBuildings.Add(this);

#if UNITY_EDITOR
        
        foreach (var building in myBuildings)
        {
            if (building != this && building.MyID == MyID)
                Debug.LogError("Two identical Ids between" + building.gameObject.name + " and " + this.gameObject.name);
        }
#endif

    }
    private void OnDestroy()
    {
        myBuildings.Remove(this);
    }
    protected override void Start()
    {
        base.Start();
        myButton.onClick.AddListener(OnPress);
        LoadAndApplyData();
    }
    public override void ShowView()
    {
        base.ShowView();
        levelHandler.RefreshUi();
    }
    #endregion

    #region Public Functions
    protected virtual void LoadAndApplyData()
    {
        levelHandler.Intialize(GetLevel(), this);
    }
    public void SetLevel(int level)
    {
        if (GameManager.DataManager.characterData.buildingsLevels.ContainsKey(MyID))
        {
            Manager.DataManager.characterData.buildingsLevels[MyID] = new BuildingData() 
            {
                BuildingType = levelHandler.buildingType,
                BuildingLevel = levelHandler.currentLevel 
            };
        }
        else
        {
            Manager.DataManager.characterData.buildingsLevels.Add(
                
                MyID, 
                new BuildingData()
                {
                    BuildingType = levelHandler.buildingType,
                    BuildingLevel = levelHandler.currentLevel
                });
        }
    }
    private int GetLevel()
    {
        if (GameManager.DataManager.characterData.buildingsLevels.ContainsKey(MyID))
            return GameManager.DataManager.characterData.buildingsLevels[MyID].BuildingLevel;
        else
            return 1;
    }

    #endregion

    #region Private Functions
    #endregion

    #region CallBacks and Coroutines
    protected virtual void OnPress()
    {
        ShowView();
    }
    #endregion

}
