using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabsElement : UIElement
{
    public TextMeshProUGUI tabsText;
    public Image tabsImage;

    public List<string> tabsNames = new List<string>();
    public List<Sprite> tabsSprites = new List<Sprite>();
    public List<Image> tabsIndicators = new List<Image>();


    const float tabIndicatorOffAlpha = 0.2f;
    const float tabIndicatorOnAlpha = 1f;


    public void SetTab(int tab)
    {
        tabsText.text = tabsNames[tab];
        tabsImage.sprite = tabsSprites[tab];
        
        for(int i =0; i < tabsIndicators.Count; i++) 
        {
            if(tab == i)
                tabsIndicators[i].color = new Color(tabsIndicators[i].color.r,
                                                    tabsIndicators[i].color.g,
                                                    tabsIndicators[i].color.b,
                                                    tabIndicatorOnAlpha);
            else
                tabsIndicators[i].color = new Color(tabsIndicators[i].color.r,
                                                    tabsIndicators[i].color.g,
                                                    tabsIndicators[i].color.b,
                                                    tabIndicatorOffAlpha);
        }
    }
}
