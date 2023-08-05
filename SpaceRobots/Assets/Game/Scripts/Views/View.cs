using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    #region Private Variables
    public Button closeButton;
    #endregion

    #region Public Variables
    public List<View> mySubViews = new List<View>();
    public List<UIElement> myElements = new List<UIElement>();
    public static List<View> allViews = new List<View>();
    #endregion

    #region Unity Delgates

    protected virtual void Awake()
    {
        allViews.Add(this);
    }
    protected virtual void Start()
    {
        closeButton?.onClick.AddListener(OnClosePressed);
    }
    private void OnDestroy()
    {
        allViews.Remove(this);
    }
    #endregion

    #region Public Functions
    public virtual void ShowView()
    {
        foreach (View view in mySubViews)
            view.ShowView();

        foreach (UIElement element in myElements)
            if(element.Visible == true)
                element.gameObject.SetActive(true);
    }
    public virtual void HideView()
    {
        foreach (View view in mySubViews)
            view.HideView();

        foreach (UIElement element in myElements)
            element.gameObject.SetActive(false);
    }
    public T GetView<T>() where T : View
    {
        T view =  (T)mySubViews.Find(r => r.GetType() == (typeof(T)));
        
        if (view != null)
        {
            return view;
        }
        else
        {
            return null;
        }
    }
    
    public static T GetViewGlobally<T>() where T : View
    {
        View view = allViews.Find(r => r.GetType() == (typeof(T)));

        if (view != null)
        {
            return (T)view;
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region Private Functions
    #endregion

    #region CallBacks and Coroutines
    private void OnClosePressed()
    {
        HideView();
    }
    #endregion

    #region Editor Buttons
    [Button("Show View")]
    public void ShowViewInEditor()
    {
        ShowView();
    }

    [Button("Hide View")]
    public void HideViewInEditor()
    {
        HideView();
    }
    #endregion

}
