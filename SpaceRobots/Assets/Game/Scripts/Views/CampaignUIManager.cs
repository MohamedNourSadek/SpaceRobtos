using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignUIManager : View
{
    #region Public Variables
    public WorldUI world1Button;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        
        world1Button.GetComponentInChildren<Button>().onClick.AddListener(View.GetViewGlobally<MainStoryUIManager>().ShowView);
        world1Button.GetComponentInChildren<Button>().onClick.AddListener(HideView);
    }
    #endregion

    #region Public Functions
    protected override void Start()
    {
        base.Start();

        world1Button.levels.text = Manager.DataManager.characterData.GetCurrentStoryLevel() + "/12";
    }
    #endregion

    
    #region Private Functions

    #endregion

    #region CallBacks and Coroutines

    #endregion

}
