using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ExpeditionUIManager : View
{
    #region Public Variables
    public TextMeshProUGUI energyNeededText;
    public Button battleButton;
    public Button maxTroopButton;
    public List<TroopsSlotUi> addTroops;
    public ExpeditionType expeditionType;
    public MarchingData attackedPlayer;
    #endregion

    #region Private Variables
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();
        
        battleButton.onClick.AddListener(OnFightPressed);
        
        foreach (var button in addTroops)
            button.GetComponent<Button>().onClick.AddListener(View.GetViewGlobally<SetTroopUIManager>().ShowView);
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();
        RefereshTroops();

        if(expeditionType == ExpeditionType.Setting)
            battleButton.gameObject.SetActive(false);
        else if(expeditionType == ExpeditionType.AttackVsAi)
            battleButton.gameObject.SetActive(true);
        else if(expeditionType == ExpeditionType.AttackVsPlayer)
            battleButton.gameObject.SetActive(true);
    }
    public void RefereshTroops()
    {
        int i = 0;

        foreach (var t in Manager.DataManager.characterData.robotsInTroops)
        {
            addTroops[i].SetTroops(t.Key, t.Value);
            addTroops[i].SetState(true);
            i++;
        }

        for (int k = i; k < addTroops.Count; k++)
        {
            addTroops[k].SetState(false);
        }

        bool haveEnergy = Manager.DataManager.characterData.energyAmount >= CharacterData.ENERGY_NEEDED_FOR_BATTLE;
        bool haveTroops = Manager.DataManager.characterData.robotsInTroops.Count >= 1;

        if (haveEnergy)
        {
            energyNeededText.color = Color.green;
            energyNeededText.text = "You've Enough Energy To Join (" + Manager.DataManager.characterData.energyAmount.ToString() + ")";
        }
        else
        {
            energyNeededText.color = Color.red;
            energyNeededText.text = "You need (" + (CharacterData.ENERGY_NEEDED_FOR_BATTLE - Manager.DataManager.characterData.energyAmount).ToString() + ") more energy to battle";
        }

        if (haveEnergy && haveTroops)
            battleButton.interactable = true;
        else
            battleButton.interactable = false;
    }
    #endregion
     
    #region Private Functions
    private void StartFighting()
    {
        Instantiate(Resources.Load("Prefabs/Fight View"), this.transform.parent).GetComponent<FightUIManager>().ShowView();
        GameManager.DataManager.characterData.AddEnergy(-CharacterData.ENERGY_NEEDED_FOR_BATTLE);
        View.GetViewGlobally<MenuUIManager>().HideView();
        HideView();
    }
    private void Attack()
    {
        bool exists = Manager.DataManager.marchingQueue.Find(d => d.playerId == attackedPlayer.playerId) != null;

        if (exists == false)
        {
            MarchingData activityData = new MarchingData()
            {
                marchingType = MarchingType.Attack,
                playerData = attackedPlayer.playerData,
                playerId = attackedPlayer.playerId
            };

            Manager.DataManager.marchingQueue.Add(activityData);
            View.GetViewGlobally<MarchingView>().AddItem(activityData);
            HideView();
        }
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnFightPressed()
    {
        if(expeditionType == ExpeditionType.AttackVsPlayer)
        {
            Attack();
        }
        else if(expeditionType == ExpeditionType.AttackVsAi)
        {
            StartFighting();
        }
    }
    #endregion

}
public enum ExpeditionType
{
    Setting,
    AttackVsAi,
    AttackVsPlayer
}