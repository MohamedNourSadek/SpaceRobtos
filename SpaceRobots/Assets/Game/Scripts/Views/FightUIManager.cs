using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : View
{
    #region Public Variables
    public Button QuickButton;
    public Button Continue;
    public List<RobotSpawnUIPosition> myTroops;
    public List<RobotSpawnUIPosition> enemyTroops;
    public GameObject GameEnd;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public float attackDelay = 1f;
    #endregion

    #region Private Variables
    int selectedLevel;
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        base.Start();
        Continue.onClick.AddListener(OnContinue);
    }
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        selectedLevel = View.GetViewGlobally<MainStoryUIManager>().selectedLevel;

        base.ShowView();
        GameEnd.SetActive(false);
        SpawnTroops();
    }
    #endregion

    #region Private Functions
    private void SpawnTroops()
    {
        StopAllCoroutines();
        SpawnFriendlies();
        SpawnEnemies();
    }
    private void SpawnFriendlies()
    {
        int i = 0;

        foreach (var troop in Manager.DataManager.characterData.robotsInTroops)
        {
            myTroops[i].gameObject.SetActive(true);
            myTroops[i].amount.text = troop.Value.ToString();
            myTroops[i].robotType = troop.Key;

            StartCoroutine(FireGun(myTroops[i], true));
            i++;
        }

        for (int j = i; j < myTroops.Count; j++)
            myTroops[j].gameObject.SetActive(false);
    }
    private void SpawnEnemies()
    {
        int i = 0;

        foreach (var troop in CharacterData.levelsData[selectedLevel])
        {
            enemyTroops[i].gameObject.SetActive(true);
            enemyTroops[i].amount.text = troop.Value.ToString();
            StartCoroutine(FireGun(enemyTroops[i], false));

            i++;
        }

        for (int j = i; j < enemyTroops.Count; j++)
            enemyTroops[j].gameObject.SetActive(false);
    }
    private IEnumerator FireGun(RobotSpawnUIPosition robot, bool friendly)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));

        while(true)
        {
            if (robot.isActiveAndEnabled)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/F1 Hunter Fire Animation"), robot.FirePositions.transform);
                
                yield return new WaitForSeconds(1f);

                var enemy = GetRandomRobot(!friendly);
                
                Instantiate(Resources.Load<GameObject>("Prefabs/F1 Hunter Fire Boom"), enemy.HitPositions.transform);
                
                GameObject hitText = Instantiate(Resources.Load<GameObject>("Prefabs/Hit UI"), enemy.HitPositions.transform);
                
                if (!friendly)
                {
                    hitText.gameObject.transform.Rotate(new Vector3(0, 0, 180));
                    hitText.gameObject.transform.parent = this.transform;
                }

                int amountAfterDamage = Int32.Parse(enemy.amount.text.ToString()) - 1;
                enemy.amount.text = amountAfterDamage.ToString();
                hitText.GetComponentInChildren<TextMeshProUGUI>().text = "1";

                if (!friendly)
                {
                    Manager.DataManager.characterData.robotsInTroops[enemy.robotType] = amountAfterDamage;
                    Manager.DataManager.characterData.robotsIHave[enemy.robotType]--;
                }
                
                if (amountAfterDamage <= 0)
                {
                    yield return new WaitForSeconds(0.3f);

                    enemy.gameObject.SetActive(false);
                    CheckEndGameCondition();
                }
            }
            else
                break;

            yield return new WaitForSeconds(attackDelay);
        }
    }
    private RobotSpawnUIPosition GetRandomRobot(bool friendly)
    {
        if (friendly)
            return myTroops[UnityEngine.Random.Range(0, myTroops.Count)];
        else
            return enemyTroops[UnityEngine.Random.Range(0, enemyTroops.Count)];
    }

    private void CheckEndGameCondition()
    {
        int friendliesLeft = 0;

        foreach(var item in myTroops)
            if(item.gameObject.activeSelf)
                friendliesLeft ++;

        if(friendliesLeft == 0)
        {
            StopAllCoroutines();
            ShowLost();
        }

        int enemiesLeft = 0;

        foreach(var item in enemyTroops)
            if(item.gameObject.activeSelf)
                enemiesLeft ++;

        if(enemiesLeft == 0)
        {
            StopAllCoroutines();
            ShowWin();
        }
    }
    private void ShowLost()
    {
        GameEnd.SetActive(true);
        WinPanel.SetActive(false);
        LosePanel.SetActive(true);

        Manager.PlayFabManager.SendPlayerData();
    }
    private void ShowWin()
    {
        GameEnd.SetActive(true);
        LosePanel.SetActive(false);
        WinPanel.SetActive(true);

        Manager.DataManager.characterData.AddEnergy((int)(CharacterData.ENERGY_NEEDED_FOR_BATTLE * 1.05));
        Manager.DataManager.characterData.MainStoryProgress[selectedLevel] = 1;
        Manager.PlayFabManager.SendPlayerData();
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnContinue()
    {
        View.GetViewGlobally<MenuUIManager>().ShowView();
        View.GetViewGlobally<BaseUIManager>().ShowView();   
        View.GetViewGlobally<ResourceUIManager>().HideView();


        Destroy(this.gameObject);
    }
    #endregion
}
