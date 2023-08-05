using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : Manager, IChatClientListener
{
    #region Public Variables
    public static string publicChannelName = "GlobalChat";
    #endregion

    #region Private Variables
    private ChatClient myClient;
    #endregion

    #region Unity Delgates
    public override void Awake()
    {
        base.Awake();
        PhotonManager = this;
        myClient = new ChatClient(this);
    }

    private void Update()
    {
        myClient.Service();
    }
    #endregion 

    #region Public Functions
    public void ConnectToChat()
    {
        string userId = Manager.PlayFabManager.playfabID;

        myClient.ChatRegion = "EU";
        myClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            PhotonNetwork.AppVersion,
            new AuthenticationValues(userId));
    }
    public void SendPublicMessage(string message)
    {
        myClient.PublishMessage(publicChannelName, message);
    }
    public void SendPrivateMessage(string desID,  string message)
    {
        myClient.SendPrivateMessage(desID, message);
    }
    #endregion 

    #region Private Functions

    #endregion

    #region CallBacks and Coroutines
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(level + message);
    }
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state);
    }
    public void OnConnected()
    {
        Debug.Log("Connected To Chat Successfuly");
        myClient.Subscribe(new string[] { publicChannelName });
    }
    public void OnDisconnected()
    {
        Debug.Log("Chat Dissconnected");
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        ChatUIView chatUIView = View.GetViewGlobally<ChatUIView>();
     
        if (chatUIView)
        {
            for(int i = 0; i < messages.Length; i ++)
                chatUIView.AddMessage(senders[i], (string)(messages[i]));
        }
    }
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if(sender == Manager.PlayFabManager.playfabID)
        {
            PlayFabManager.OnOtherPlayerDataRecieved += (CharacterData myData) => 
            {
                PlayFabManager.OnOtherPlayerDataRecieved = null;
                View.GetViewGlobally<MailUIView>().AddEmail((string)message, myData, sender);
            };

            PlayFabManager.GetOtherPlayerData(sender);
        }

    }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed successfully");
    }
    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }
    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
