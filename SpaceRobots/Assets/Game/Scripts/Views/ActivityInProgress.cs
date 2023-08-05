using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityInProgress : MonoBehaviour
{
    public Slider timeLeft;
    public TextMeshProUGUI activityName;
    public Button GoTo;
    public Button Cancel;

    public MarchingData marchingData;
}
