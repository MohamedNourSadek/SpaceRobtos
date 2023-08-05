using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseRobotsViewer : MonoBehaviour
{
    public static BaseRobotsViewer Instance;

    public Image F1Hunter;

    private void Awake()
    {
        Instance = this;
        RefereshParkedRobots();
    }

    public void RefereshParkedRobots()
    {
        var robotsIHave = Manager.DataManager.characterData.robotsIHave;

        if(robotsIHave.ContainsKey(RobotsNames.F1Hunter) && robotsIHave[RobotsNames.F1Hunter] > 0)
        {
            F1Hunter.gameObject.SetActive(true);
        }
        else
        {
            F1Hunter.gameObject.SetActive(false);
        }
    }
}
