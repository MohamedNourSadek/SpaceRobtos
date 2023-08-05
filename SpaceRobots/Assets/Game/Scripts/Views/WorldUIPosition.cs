using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIPosition : MonoBehaviour
{
    #region Public Variables
    public Image playerIcon;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI powerAmount;
    public TextMeshProUGUI cooardinates;
    public Button myButton;
    public string PlayerID;
    public CharacterData characterData;
    #endregion
    
    #region Private Variables
    #endregion
    
    #region Unity Delgates
    
    #endregion
    
    #region Public Functions
    public void Initialize(CharacterData playerData, string playerId, bool Debug = false)
    {
        if (playerData == null)
        {
            playerName.text = "";
            powerAmount.text = "";
            playerIcon.enabled = false;
            cooardinates.text = "";
        }
        else
        {
            playerName.text = playerData.playerName;
            playerName.gameObject.name = playerData.playerName;
            powerAmount.text = "";  
            //powerAmount.text = "Power: " + playerData.powerNumber.ToString();
            playerIcon.enabled = true;
            cooardinates.text = 
                (playerData.worldMapPosition.y + 1).ToString() + "," +
                (playerData.worldMapPosition.x + 1).ToString();

            characterData = playerData;
            PlayerID = playerId;
        }

        cooardinates.enabled = Debug;
    }
    #endregion

    #region Private Functions
    #endregion

    #region CallBacks and Coroutines

    #endregion

}
