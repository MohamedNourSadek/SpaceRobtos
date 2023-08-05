using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CharacterSelect : UIElement
{
    #region Public Variables
    public float selectedSize;
    public float unSelectedSize;
    public List<Button> characterButtons;
    #endregion

    #region Private Variables
    Button selectedButton;
    #endregion

    #region Unity Delgates
    protected override void Start()
    {
        foreach (var button in characterButtons)
            button.onClick.AddListener(OnButtonClick);

        SelectButton(characterButtons[0]);
    }
    #endregion

    #region Public Functions
    public int GetSelectedCharacter()
    {
        return Int32.Parse(selectedButton.name);
    }
    #endregion

    #region Private Functions
    private Button GetSelectedButton()
    {
        foreach (var button in characterButtons)
        {
            if (EventSystem.current.currentSelectedGameObject == button.gameObject)
            {
                return button;
            }
        }

        return null;
    }
    private void SelectButton(Button _button)
    {
        foreach (var button in characterButtons)
        {
            if (_button.name == button.gameObject.name)
            {
                selectedButton = button;
                button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selectedSize);
                button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selectedSize);
            }
            else
            {
                button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, unSelectedSize);
                button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, unSelectedSize);
            }
        }
    }
    #endregion

    #region CallBacks and Coroutines
    private void OnButtonClick()
    {
        SelectButton(GetSelectedButton());
    }
    #endregion

}
