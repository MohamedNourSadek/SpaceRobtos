using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using System;
using UnityEngine.Events;
using PlayFab.Internal;

public class PlayFabManager : Manager
{
    #region Public Variables
    public Action OnDataRetrieved;
    public Action OnDataSent;
    public Action<Dictionary<string, CharacterData>> OnPlayersInfoRetrieved;
    public Action<LoginResult> OnLogin;
    public Action OnUpdateOtherPlayer;
    public Action<CharacterData> OnOtherPlayerDataRecieved;
    public string playfabID;
    #endregion

    #region Private Variables
    private string playerDataKey = "PlayerData";
    private string playerFriendsData = "Friends";
    private string playerBlockedData = "BlockedList";
    #endregion

    #region Unity Delgates
    public override void Awake()
    {
        base.Awake();
        Manager.PlayFabManager = this;
    }
    #endregion

    #region Public Functions
    public void Login()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();
        request.CustomId = SystemInfo.deviceUniqueIdentifier;
        request.CreateAccount = true;

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailed);

#elif UNITY_ANDROID 
        LoginWithAndroidDeviceIDRequest requestAnd = new LoginWithAndroidDeviceIDRequest();
        requestAnd.CreateAccount = true;
        requestAnd.AndroidDeviceId = SystemInfo.deviceUniqueIdentifier;

        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAnd, OnLoginSuccess, OnLoginFailed);
#endif

    }
    public void GetPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnPlayerDataRetrieved, OnPlayerDataRetrievedFailed);
    }
    public void GetOtherPlayerData(string playerId)
    {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() { 
                FunctionName = "GetOtherPlayerData" ,
                FunctionParameter = new { PlayerID = playerId}
            },
            OnOtherPlayerDataRecievedCallback, OnOtherPlayerDataRecievedCallbackError);

    }
    public void SendPlayerData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { playerDataKey, JsonConvert.SerializeObject(DataManager.characterData)}
            }
        }, OnPlayerDataSent, OnPlayerDataSentFailed);
    }
    public void SendPlayerFriendsData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { playerFriendsData, JsonConvert.SerializeObject(DataManager.friendsList)}
            }
        }, OnPlayerDataSent, OnPlayerDataSentFailed);
    }
    public void SendPlayerBlockedData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { playerBlockedData, JsonConvert.SerializeObject(DataManager.blockedList)}
            }
        }, OnPlayerDataSent, OnPlayerDataSentFailed);
    }
    public void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataRetrieved, OnTitleDataFailed);
    }
    public void GetAllPlayers()
    {
        PlayFabClientAPI.ExecuteCloudScript(
    new ExecuteCloudScriptRequest() { FunctionName = "GetAllPlayersInSegmenet" },
    OnSegmentRecieved, OnSegmentError);
    }



    public void ModifyAPlayerData(string playerID, CharacterData characterData)
    {
        Dictionary<string, string> updateData = new Dictionary<string, string>()
        {
            {playerDataKey, JsonConvert.SerializeObject(characterData) }
        };

        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "updateGeneralData",
                FunctionParameter = new
                {
                    playFabId = playerID,
                    updateData = updateData
                },
            }
            , OnUpdateOtherUserSuccess, OnUpdateOtherUserFail);
    }
    private void OnUpdateOtherUserSuccess(ExecuteCloudScriptResult obj)
    {
        OnUpdateOtherPlayer?.Invoke();
    }
    private void OnUpdateOtherUserFail(PlayFabError obj)
    {

    }
    #endregion

    #region Private Functions


    #endregion

    #region CallBacks and Coroutines
    private void OnLoginSuccess(LoginResult obj)
    {
        OnLogin?.Invoke(obj);
        playfabID = obj.PlayFabId;
        Manager.GetManager<PhotonManager>().ConnectToChat();
    }
    private void OnLoginFailed(PlayFabError obj)
    {
        Debug.LogError("Login Failed " + obj.ErrorMessage);
    }
    private void OnTitleDataFailed(PlayFabError obj)
    {
        throw new NotImplementedException();
    }
    private void OnTitleDataRetrieved(GetTitleDataResult obj)
    {
        var x = JsonConvert.DeserializeObject<CharacterData>(obj.Data[playerDataKey]);
        Debug.Log("Data Recieved");
    }
    private void OnPlayerDataSent(UpdateUserDataResult obj)
    {
        OnDataSent?.Invoke();
    }
    private void OnPlayerDataSentFailed(PlayFabError obj)
    {
        Debug.LogError("Data sending failed, Trying again .. ");
    }
    private void OnPlayerDataRetrieved(GetUserDataResult obj)
    {
        if (obj.Data.ContainsKey(playerDataKey))
        {
            CharacterData newData = JsonConvert.DeserializeObject<CharacterData>(obj.Data[playerDataKey].Value);
            
            if(BaseRobotsViewer.Instance != null)
                BaseRobotsViewer.Instance.RefereshParkedRobots();
            DataManager.characterData = newData;
        }
        else
        {
            DataManager.characterData = new CharacterData(); 
        }

        if(obj.Data.ContainsKey(playerFriendsData))
        {
            List<playerInfo> myFriends = JsonConvert.DeserializeObject<List<playerInfo>>(obj.Data[playerFriendsData].Value);
            DataManager.friendsList = myFriends;
        }
        else
        {
            DataManager.friendsList = new List<playerInfo>();
        }

        if (obj.Data.ContainsKey(playerBlockedData))
        {
            List<playerInfo> myBlockList = JsonConvert.DeserializeObject<List<playerInfo>>(obj.Data[playerBlockedData].Value);
            DataManager.blockedList = myBlockList;
        }
        else
        {
            DataManager.blockedList = new List<playerInfo>();
        }

        OnDataRetrieved?.Invoke();
    }
    private void OnPlayerDataRetrievedFailed(PlayFabError obj)
    {
        Debug.LogError("Data Retrieve failed, Trying again .. ");
        GetPlayerData();
    }
    private void OnSegmentRecieved(ExecuteCloudScriptResult obj)
    {
        Dictionary<string, GetUserDataResult> Info = new Dictionary<string, GetUserDataResult>();
       
        try
        {
            Info = JsonConvert.DeserializeObject<Dictionary<string, GetUserDataResult>>(obj.FunctionResult.ToString());
        }
        catch { }

        Dictionary<string, CharacterData> myPlayers = new Dictionary<string, CharacterData>();

        foreach (var info in Info)
        {
            if (info.Key != "")
                myPlayers[info.Key] = JsonConvert.DeserializeObject<CharacterData>(info.Value.Data[playerDataKey].Value);
        }

        OnPlayersInfoRetrieved?.Invoke(myPlayers);
    }
    private void OnSegmentError(PlayFabError obj)
    {
        Debug.LogError(obj.ErrorMessage);
    }
    private void OnOtherPlayerDataRecievedCallback(ExecuteCloudScriptResult obj)
    {
        try
        {
            string data = JsonConvert.DeserializeObject<GetUserDataResult>(obj.FunctionResult.ToString()).Data[playerDataKey].Value;

            CharacterData otherPlayer = JsonConvert.DeserializeObject<CharacterData>(data);


            OnOtherPlayerDataRecieved?.Invoke(otherPlayer);
        }
        catch { }
    }
    private void OnOtherPlayerDataRecievedCallbackError(PlayFabError obj)
    {
        Debug.Log("Other Player Data Failed");
    }
    #endregion

}
