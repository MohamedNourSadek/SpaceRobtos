using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Manager
{

    public override void Awake()
    {
        base.Awake();
        Manager.ScenesManager = this;
    }
    public void LoadScene(SceneName sceneName, Action OnSceneLoaded)
    {
        LoadScene(sceneName, OnSceneLoaded, 0);
    }

    public void LoadScene(SceneName sceneName, Action OnSceneLoaded, float delay)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
        StartCoroutine(LoadScene(asyncOperation, OnSceneLoaded, delay));
    }


    IEnumerator LoadScene(UnityEngine.AsyncOperation operation, Action OnSceneLoaded, float delay)
    {
        while(!operation.isDone)
            yield return null;

        yield return new WaitForSecondsRealtime(delay); 

        OnSceneLoaded?.Invoke();
    }
}

public enum SceneName
{
    Intro,
    MainMenu,
    InGame
}
