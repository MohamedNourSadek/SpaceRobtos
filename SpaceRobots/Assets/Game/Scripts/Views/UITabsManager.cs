using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UITabsManager
{
    public List<View> uiTabs;
    public List<Button> uiTabsButtonTriggers;
    public int selected;
    public Action OnSelectedChanged;

    public void Initialize(int defaultSelected = 0)
    {
        foreach (var uiTabButton in uiTabsButtonTriggers)
        {
            uiTabButton.onClick.AddListener(OnTabButtonPressed);
        }

        SetSelected(defaultSelected);
    }
    public void SetSelected(int selected)
    {
        this.selected = selected;

        for (int i = 0; i < uiTabs.Count; i++)
        {
            if (i == selected)
            {
                uiTabs[i].ShowView();

                uiTabsButtonTriggers[i].GetComponent<Image>().color =
                    new Color(uiTabsButtonTriggers[i].colors.normalColor.r,
                              uiTabsButtonTriggers[i].colors.normalColor.g,
                              uiTabsButtonTriggers[i].colors.normalColor.b,
                              1f);
            }
            else
            {
                uiTabs[i].HideView();

                uiTabsButtonTriggers[i].GetComponent<Image>().color =
                    new Color(uiTabsButtonTriggers[i].colors.normalColor.r,
                              uiTabsButtonTriggers[i].colors.normalColor.g,
                              uiTabsButtonTriggers[i].colors.normalColor.b,
                              .5f);
            }
        }

        OnSelectedChanged?.Invoke();
    }
    public int GetSelected()
    {
        for (int i = 0; i < uiTabs.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == uiTabsButtonTriggers[i].gameObject)
            {
                return i;
            }
        }

        return 0;
    }
    private void OnTabButtonPressed()
    {
        int selectedTab = GetSelected();
        SetSelected(selectedTab);
    }
}
