using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailUIView : View
{
    #region Public Variables
    public Button backgroundClickable;
    public UITabsManager tabsManager;
    public GameObject ReportsParent;
    public GameObject EmailsParent;
    public GameObject ReportPrefab;
    #endregion

    #region Private Variables
    int numberOfNotifications = 0;
    public List<EmailUIItem> emails;
    #endregion

    #region Unity Delgates
    #endregion

    #region Public Functions
    public override void ShowView()
    {
        base.ShowView();

        tabsManager.Initialize();

        numberOfNotifications = 0;
        backgroundClickable.onClick.AddListener(HideView);
        View.GetViewGlobally<BottomView>().SetNotifications(numberOfNotifications);
    }
    public void AddReport(MarchingData otherPlayerData, CharacterData myData)
    {
        AddNotification();

        EmailUIItem newReport = SpawnItem(ReportsParent);
        newReport.InitializeAsReport(otherPlayerData, myData);
    }

    public void AddEmail(string message, CharacterData myData, string playerID)
    {
        AddNotification();
        
        EmailUIItem newReport = SpawnItem(EmailsParent);
        newReport.InitializeAsEmail(message, myData, playerID);
    }
    #endregion

    #region Private Functions
    private void AddNotification()
    {
        numberOfNotifications++;
        View.GetViewGlobally<BottomView>().SetNotifications(numberOfNotifications);
    }
    private EmailUIItem SpawnItem(GameObject parent)
    {
        return Instantiate(ReportPrefab, parent.gameObject.transform).GetComponent<EmailUIItem>();
    }
    #endregion

    #region CallBacks and Coroutines

    #endregion

}
