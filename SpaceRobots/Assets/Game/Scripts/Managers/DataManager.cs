using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : Manager
{
    #region Public Variables
    public CharacterData characterData = new CharacterData();
    public List<playerInfo> friendsList = new List<playerInfo>();
    public List<playerInfo> blockedList = new List<playerInfo>();
    public Dictionary<string, CharacterData> otherPlayersInCurrentMap = new Dictionary<string, CharacterData>();
    public List<MarchingData> marchingQueue = new List<MarchingData>();
    public Sprite F1Robot;
    public Sprite IronBuilding;
    public Sprite GoldBuilding;
    public Sprite OilBuilding;
    public Sprite TitaniumBuilding;
    public Sprite UraniumBuilding;
    #endregion

    #region Private Variables
    #endregion

    #region Unity Delgates
    public override void Awake()
    {
        base.Awake();
        Manager.DataManager = this;
    }
    #endregion

    #region Public Functions
    public Sprite GetSprite(string spriteName, Image EntityImage)
    {
        if (spriteName == "FV Hunter A1")
        {
            EntityImage.transform.rotation = Quaternion.Euler(0f, 0f, 0);
            return F1Robot;
        }
        else if (spriteName == "Iron Factory")
        {
            EntityImage.transform.rotation = Quaternion.Euler(0f, 0f, 90);
            return IronBuilding;
        }
        else if (spriteName == "Gold Factory")
        {
            EntityImage.transform.rotation = Quaternion.Euler(0f, 0f, 0);
            return GoldBuilding;
        }
        else if (spriteName == "Oil Factory")
        {
            EntityImage.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            return OilBuilding;
        }
        else if (spriteName == "Titanium Factory")
        {
            EntityImage.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            return TitaniumBuilding;
        }
        else if (spriteName == "Uranium Factory")
        {
            EntityImage.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            return UraniumBuilding;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines

    #endregion

}
