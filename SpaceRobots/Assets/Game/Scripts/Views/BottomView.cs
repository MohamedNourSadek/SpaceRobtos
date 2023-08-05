using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BottomView : View
{
    public GameObject NotificationObject;
    public TextMeshProUGUI numberOfNotifications;

    protected override void Awake()
    {
        base.Awake();

        SetNotifications(0);
    }

    public override void ShowView()
    {
        base.ShowView();
    }

    public void SetNotifications(int i)
    {
        if (i == 0)
        {
            NotificationObject.SetActive(false);
        }
        else
        {
            NotificationObject.SetActive(true);
            numberOfNotifications.text = i.ToString();
        }
    }
}
