using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainStoryUIManager : View
{
    #region Public Variables
    public List<StoryUIElement> lvls;
    public int selectedLevel;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();

        foreach(var lvl in lvls) {

            Button button = lvl.GetComponent<Button>();
            button.onClick.AddListener(OnLevelPressed);
        }
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
        RefreshLevels();
    }
    #endregion

    #region Private Functions
    private void RefreshLevels()
    {
        for(int i =0 ; i < lvls.Count; i++)
        {
            int levelProgress = Manager.DataManager.characterData.MainStoryProgress[i];

            lvls[i].SetStarts(levelProgress);

            int prevLevel = i - 1;

            if (prevLevel >= 0)
            {
                int prevLevelProgress = Manager.DataManager.characterData.MainStoryProgress[prevLevel];

                if (prevLevelProgress == 0)
                    lvls[i].SetLock(true);
                else
                    lvls[i].SetLock(false);
            }
            else
            {
                lvls[i].SetLock(false);
            }
        }
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnLevelPressed()
    {
        for(int i =0; i < lvls.Count;i++)
            if (EventSystem.current.currentSelectedGameObject == lvls[i].gameObject)
                selectedLevel = i;

        View.GetViewGlobally<ExpeditionUIManager>().expeditionType = ExpeditionType.AttackVsAi;
        View.GetViewGlobally<ExpeditionUIManager>().ShowView();
        HideView();
    }
    #endregion

}
