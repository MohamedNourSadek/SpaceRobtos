using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MarchingView : View
{
    #region Public Variables
    public GameObject activityInProgressPrefab;
    public GameObject activitiesParent;
    #endregion

    #region Private Variables
    public List<ActivityInProgress> activitiesInProgress = new List<ActivityInProgress>();
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();

        if (activitiesInProgress.Count == 0)
            View.GetViewGlobally<WorldUIView>().marching.gameObject.SetActive(false);
    }
    private void Update()
    {
        try
        {
            foreach (var activityInProgress in activitiesInProgress)
            {
                if (activityInProgress != null)
                {
                    activityInProgress.timeLeft.value -= Time.deltaTime;

                    if (activityInProgress.timeLeft.value <= 0)
                        OnTimeDone(activityInProgress);
                }
            }
        }
        catch (Exception e){ Debug.Log(e.Message); }
    }
    #endregion

    #region Public Functions
    public void AddItem(MarchingData activity)
    {
        var activityUI = Instantiate(activityInProgressPrefab, activitiesParent.transform).GetComponent<ActivityInProgress>();

        activityUI.timeLeft.maxValue = ScoutUIView.scoutWaitTime;
        activityUI.timeLeft.value = ScoutUIView.scoutWaitTime;

        if(activity.marchingType == MarchingType.Attack)
            activityUI.activityName.text = "Attacking - " + activity.playerData.playerName;
        else
            activityUI.activityName.text = "Scouting - " + activity.playerData.playerName;

        activityUI.marchingData = activity;
        activityUI.Cancel.onClick.AddListener(OnCancelPressed);
        activityUI.GoTo.onClick.AddListener(OnGoToPressed);

        activitiesInProgress.Add(activityUI);

        View.GetViewGlobally<WorldUIView>().marching.gameObject.SetActive(true);
    }
    #endregion

    #region Private Functions
    private void RemoveActivity(ActivityInProgress activity)
    {
        activitiesInProgress.Remove(activity);
        Destroy(activity.gameObject);
        Manager.DataManager.marchingQueue.Remove(activity.marchingData);

        if (activitiesInProgress.Count == 0)
        {
            View.GetViewGlobally<WorldUIView>().marching.gameObject.SetActive(false);
            HideView();
        }

        GameObject myPlayer = View.GetViewGlobally<WorldUIView>().WorldUIPositions[Manager.DataManager.characterData.worldMapPosition.x - 1]
                                                    [Manager.DataManager.characterData.worldMapPosition.y - 1].gameObject;

        ArrowUI[] arrows = myPlayer.GetComponentsInChildren<ArrowUI>();

        foreach(ArrowUI arrow in arrows)
            Destroy(arrow.gameObject);
    }
    private ActivityInProgress GetSelectedItem()
    {
        return EventSystem.current.currentSelectedGameObject.gameObject.GetComponentInParent<ActivityInProgress>();
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnCancelPressed()
    {
        RemoveActivity(GetSelectedItem());
    }
    private void OnGoToPressed()
    {
        View.GetViewGlobally<WorldUIView>().SwitchToPosition(GetSelectedItem().marchingData.playerData.worldMapPosition);
        HideView();
    }
    private void OnTimeDone(ActivityInProgress activity)
    {
        var enemyData = new MarchingData()
        {
            marchingType = MarchingType.Attack,
            playerId = activity.marchingData.playerId,
            playerData = Copy(activity.marchingData.playerData)
        };
        
        var myData = Copy(Manager.DataManager.characterData);

        if (activity.marchingData.marchingType == MarchingType.Attack)
        {
            View.GetViewGlobally<MailUIView>().AddReport(enemyData, myData);

            ApplyAttackResults(activity);
        }
        else if (activity.marchingData.marchingType == MarchingType.Scout)
        {
            View.GetViewGlobally<MailUIView>().AddReport(activity.marchingData, myData);
        }

        RemoveActivity(activity);
        HideView();
    }
    private void ApplyAttackResults(ActivityInProgress activity)
    {
        CharacterData myData = Manager.DataManager.characterData;
        CharacterData enemyData = activity.marchingData.playerData;

        int troopsCompare = myData.robotsIHave[RobotsNames.F1Hunter] - enemyData.robotsIHave[(RobotsNames.F1Hunter)];


        if (troopsCompare > 0)
        {
            myData.robotsIHave[RobotsNames.F1Hunter] -= enemyData.robotsIHave[(RobotsNames.F1Hunter)];
            enemyData.robotsIHave[RobotsNames.F1Hunter] = 0;
        }
        else if (troopsCompare < 0)
        {
            enemyData.robotsIHave[RobotsNames.F1Hunter] -= myData.robotsIHave[RobotsNames.F1Hunter];
            myData.robotsIHave[RobotsNames.F1Hunter] = 0;
        }
        else
        {
            myData.robotsIHave[RobotsNames.F1Hunter] = 0;
            enemyData.robotsIHave[RobotsNames.F1Hunter] = 0;
        }

        if (myData.robotsIHave[RobotsNames.F1Hunter] < 0)
            myData.robotsIHave[RobotsNames.F1Hunter] = 0;

        if (enemyData.robotsIHave[RobotsNames.F1Hunter] < 0)
            enemyData.robotsIHave[RobotsNames.F1Hunter] = 0;

        View.GetViewGlobally<LoadingView>().ShowView();
        if(BaseRobotsViewer.Instance != null)
            BaseRobotsViewer.Instance.RefereshParkedRobots();
        Manager.PlayFabManager.OnUpdateOtherPlayer += View.GetViewGlobally<LoadingView>().HideView;
        Manager.PlayFabManager.ModifyAPlayerData(activity.marchingData.playerId, enemyData);
        Manager.PlayFabManager.SendPlayerData();
    }

    private CharacterData Copy(CharacterData data)
    {
        CharacterData copyData = new CharacterData();

        foreach(var item in data.robotsIHave)
        {
            if(copyData.robotsIHave.ContainsKey(item.Key))
                copyData.robotsIHave[item.Key] = item.Value;
            else
                copyData.robotsIHave.Add(item.Key, item.Value);
        }

        foreach(var item in data.robotsInTroops)
        {
            if (copyData.robotsInTroops.ContainsKey(item.Key))
                copyData.robotsInTroops[item.Key] = item.Value;
            else
                copyData.robotsInTroops.Add(item.Key, item.Value);
        }

        return copyData;
    }
    #endregion

}
