using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animator : MonoBehaviour
{
    public List<Sprite> sprites;
    public float timeInbetween;
    public bool loop;

    Image myImage;

    private void Start()
    {
        myImage =GetComponent<Image>();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        do
        {
            for(int i =0; i < sprites.Count; i++)
            {
                myImage.sprite = sprites[i];
                yield return new WaitForSecondsRealtime(timeInbetween);
            }
        }
        while (loop);
    }
}

     