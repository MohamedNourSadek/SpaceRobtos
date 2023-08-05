using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    const float onPressVolume = .2f;
    const string onPressAsset = "UI Audio/uiclick";

    protected override void Start()
    {
        base.Start();

        if(gameObject.GetComponent<AudioSource>() == null)
            Destroy(gameObject.GetComponent<AudioSource>());

        onClick.AddListener(OnButtonPress);
    }
    void OnButtonPress()
    {
        AudioClip clip = Resources.Load<AudioClip>(onPressAsset);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, onPressVolume);
    }
}
