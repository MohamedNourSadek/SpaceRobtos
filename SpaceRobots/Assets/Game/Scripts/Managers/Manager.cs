using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    public static Dictionary<Type, Manager> Managers = new Dictionary<Type, Manager>();

    public static GameManager GameManager;
    public static DataManager DataManager;
    public static ScenesManager ScenesManager;
    public static PlayFabManager PlayFabManager;
    public static PhotonManager PhotonManager;

    public virtual void Awake()
    {
        if (!Managers.ContainsKey(this.GetType()))
            Managers.Add(this.GetType(), this);
        else
            Debug.LogError("Duplicate Managers Found");
    }
    public virtual void OnDestroy()
    {
        if (Managers.ContainsKey(this.GetType()))
        {
            Managers[this.GetType()] = null;
            Managers.Remove(this.GetType());
        }
    }

    public static T GetManager<T>()
    {
        if (Managers.ContainsKey(typeof(T)))
            return (T)Convert.ChangeType(Managers[typeof(T)], typeof(T));
        return (T)Convert.ChangeType(null, typeof(T));
    }
}
