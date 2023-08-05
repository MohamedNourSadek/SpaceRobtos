using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIView : View
{
    #region Public Variables
    public Button ClickableBackground;
    public TextMeshProUGUI Coordinates;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI Power;
    public Image PlayerImage;
    public Button Attack;
    public Button Scout;
    public Button BookMark;
    public GameObject arrowPrefab;
    #endregion

    #region Private Variables
    CharacterData currentCharacter;
    string playerID;
    #endregion

    #region Unity Delgates
    protected override void Awake()
    {
        base.Awake();
        ClickableBackground.onClick.AddListener(HideView);
        BookMark.onClick.AddListener(OnBookMarkPressed);
        Scout.onClick.AddListener(OnScoutPressed);
        Attack.onClick.AddListener(OnAttackPressed);
    }


    #endregion

    #region Public Functions
    public void Initialize(CharacterData data, string myPlayerId)
    {
        currentCharacter = data;
        playerID = myPlayerId;

        PlayerImage.sprite = GameManager.instance.GetCharacterImage(data.characterImage);
        PlayerName.text = data.playerName;
        Power.text = "Power : " + data.powerNumber;
        Coordinates.text = "Coordinates" + "[" + data.worldMapPosition.x + "," + data.worldMapPosition.y + "]";

        bool sameName = (currentCharacter.playerName == Manager.DataManager.characterData.playerName);
        bool samePosition = (currentCharacter.worldMapPosition == Manager.DataManager.characterData.worldMapPosition);

        if (sameName && samePosition)
        {
            Attack.interactable = false;
            Scout.interactable = false;
        }
        else
        {
            Attack.interactable = true;
            Scout.interactable = true;
        }
    }
    #endregion
    
    #region Private Functions
    
    #endregion
    
    #region CallBacks and Coroutines
    private void OnBookMarkPressed()
    {
        HideView();
        View.GetViewGlobally<SetBookmarkUIView>().CharacterData = currentCharacter;
        View.GetViewGlobally<SetBookmarkUIView>().ShowView();
    }
    private void OnScoutPressed()
    {
        View.GetViewGlobally<ScoutUIView>().character = currentCharacter;
        View.GetViewGlobally<ScoutUIView>().playerID = playerID;
        View.GetViewGlobally<ScoutUIView>().ShowView();
        HideView();
    }

    private void OnAttackPressed()
    {
        View.GetViewGlobally<ExpeditionUIManager>().expeditionType = ExpeditionType.AttackVsPlayer;
        View.GetViewGlobally<ExpeditionUIManager>().attackedPlayer =
            new MarchingData() {
                marchingType = MarchingType.Attack, playerData = currentCharacter, playerId = playerID
            };
        View.GetViewGlobally<ExpeditionUIManager>().ShowView();
        HideView();

        GameObject myPlayer = View.GetViewGlobally<WorldUIView>().WorldUIPositions[Manager.DataManager.characterData.worldMapPosition.x - 1]
                                                            [Manager.DataManager.characterData.worldMapPosition.y - 1].gameObject;

        GameObject enemyPosition = View.GetViewGlobally<WorldUIView>().WorldUIPositions[currentCharacter.worldMapPosition.x - 1]
                                                                                       [currentCharacter.worldMapPosition.y - 1].gameObject;


        Vector3 arrowDirection = (enemyPosition.transform.position - myPlayer.transform.position);

        float remaingingDistance = arrowDirection.magnitude;
        int numberOfArrows = 0;
        int step  = 250;

        while(remaingingDistance > (2*step))
        {
            numberOfArrows++;

            GameObject arrow =  Instantiate(arrowPrefab, myPlayer.transform);
            
            arrow.transform.LookAt(enemyPosition.transform);
            arrow.transform.position += (arrowDirection.normalized * (numberOfArrows * step));

            remaingingDistance -= step;
        }
    }

    #endregion

}
