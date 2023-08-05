using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUIElement : MonoBehaviour
{
    public List<Image> stars;
    public GameObject Lock;


    public void SetLock(bool state)
    {
        Lock.SetActive(state);
        GetComponent<Button>().interactable = !state;
    }
    public void SetStarts(int k)
    {
        for(int i = 0 ; i < stars.Count; i++)
        {
            if (i <= k - 1)
                stars[i].color = Color.white;
            else
                stars[i].color = Color.black;
        }
    }
}
