using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingMessage : View
{
    public bool IsEnabled;
    public Color BlueColor;
    public Color OrangeColor;
    public float speed = 0.2f;
    public float msgTime = 2f;
    
    public void ShowMessage(string message)
    {
        if(IsEnabled)
        {
            GameObject messageObject = Instantiate(Resources.Load<GameObject>("Prefabs/Message Example"), this.transform);
            TextMeshProUGUI textComponent = messageObject.GetComponentInChildren<TextMeshProUGUI>();

            textComponent.text = message;
            textComponent.color = BlueColor;

            StartCoroutine(AniamteMessage(messageObject));
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) 
        {
            ShowMessage("Message Test");
        }
    }
#endif

    IEnumerator AniamteMessage(GameObject messageObject)
    {
        float timeLeft = msgTime;

        TextMeshProUGUI text = messageObject.GetComponentInChildren<TextMeshProUGUI>();

        while (timeLeft >= 0)
        {
            timeLeft -= Time.fixedDeltaTime;
            messageObject.transform.Translate(0, speed, 0);
            float newAlpha = Mathf.Lerp(0f, 1f, timeLeft / msgTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, newAlpha);

            yield return new WaitForFixedUpdate();
        }
        
        Destroy(messageObject);
    }
}
