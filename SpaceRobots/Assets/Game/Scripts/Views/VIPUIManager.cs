using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VIPUIManager : View
{
    #region Public Variables
    public Slider vipSlider;
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI targetLevel;
    public List<VIPUIItem> vipUIItems;
    public List<GameObject> vipInfo;
    public GameObject downBar;
    public GameObject normalView;
    public Button closeInfoButton;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();

        foreach(var item in vipUIItems)
            item.myButton.onClick.AddListener(OnVIPButtonPress);

        closeInfoButton.onClick.AddListener(OnCloseInfo); 
        closeButton.onClick.AddListener(OnCloseInfo);   
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
        RefreshUI();    
    }
    public void RefreshUI()
    {
        int vipLevel = GameManager.DataManager.characterData.GetVIPLevel();
        int vipPoints = GameManager.DataManager.characterData.vipPoints;

        for (int i = 0; i < vipUIItems.Count; i++)
        {
            if(vipLevel-1 >= i)
            {
                vipUIItems[i].lockImage.gameObject.SetActive(false);
            }
        }

        vipSlider.maxValue = CharacterData.MAX_VIP_POINTS;

        if(vipLevel < 12)
            vipSlider.value = vipPoints - ((vipPoints / CharacterData.MAX_VIP_POINTS) * CharacterData.MAX_VIP_POINTS);
        else 
            vipSlider.value = CharacterData.MAX_VIP_POINTS;

        int nextLevel = vipLevel + 1;
        currentLevel.text = "LVL\n" + vipLevel;
        targetLevel.text = nextLevel < 12 ? ("LVL\n" + nextLevel) : "";
    }
    #endregion

    #region Private Functions
    #endregion

    #region CallBacks and Coroutines
    private void OnVIPButtonPress()
    {
        downBar.SetActive(true);
        normalView.SetActive(false);

        for(int i = 0;i < vipUIItems.Count; i++)
        {
            if(EventSystem.current.currentSelectedGameObject == vipUIItems[i].myButton.gameObject)
                vipInfo[i].gameObject.SetActive(true);
            else
                vipInfo[i].gameObject.SetActive(false);
        }
    }
    private void OnCloseInfo()
    {
        downBar.gameObject.SetActive(false);
        normalView.SetActive(true);

        for (int i = 0; i < vipInfo.Count ; i++)
        {
            vipInfo[i].gameObject.SetActive(false);
        }
    }
    #endregion

}
