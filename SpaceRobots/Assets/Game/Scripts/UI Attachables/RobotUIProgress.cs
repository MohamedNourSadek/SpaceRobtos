using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RobotUIProgress : MonoBehaviour
{
    public Button OnCloseButton;
    public Button OnInstantButton;
    public Slider TimeSlider;

    public Action<RobotUIProgress> OnComplete;

    public float time;
    public bool isTimeComplete = false;
    
    public void ReduceTime(float deltaTime)
    {
        time -= deltaTime;
    }
    public void Initialize(UnityAction OnClosePress, UnityAction OnInstantPress, Action<RobotUIProgress> OnComplete, float MaxTime)
    {
        TimeSlider.maxValue = MaxTime;
        time = MaxTime;
        TimeSlider.value = MaxTime;

        OnCloseButton.onClick.AddListener(OnClosePress);
        OnInstantButton.onClick.AddListener(OnInstantPress);
        this.OnComplete += OnComplete;
    }
    public void OnTimeComplete()
    {
        isTimeComplete = true;
        OnComplete?.Invoke(this);
        View.GetViewGlobally<FloatingMessage>().ShowMessage("+ Robot Created");
    }
}
