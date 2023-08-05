using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Windows;

[System.Serializable]
public class CharacterData
{
    public bool fullyInitialized = false;
    public string playerName = "Name";
    public int powerNumber = 0;
    [JsonProperty] public int xp { get; private set; } = 0;
    [JsonProperty] public int vipPoints { get; private set; } = 0;
    public int gemAmount = 50;
    public int goldAmount = 3434;
    public int ironAmount = 4444;
    public int oilAmount = 4214;
    public int titaniumAmount = 3444;
    public int uraniumAmount = 4445;
    public int characterImage = 1;
    public int leadershipAmount = 0;
    public int chips = 0;
    public int accuracy = 0;
    public int interference = 0;
    public int criticalHit = 0;
    public int tenacity = 0;
    public int fireOutbreak = 0;
    public int scorePoints = 0;
    [JsonProperty] public int energyAmount { get; private set; } = 0;

    public Dictionary<int, BuildingData> buildingsLevels = new Dictionary<int, BuildingData>() { };

    public Dictionary<RobotsNames, int> robotsIHave = new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 0 } };

    public Dictionary<RobotsNames, int> robotsInTroops = new Dictionary<RobotsNames, int>() { };

    public int[] MainStoryProgress = new int[12];

    public int LastLogin = 0;
    public int numberOfDaysLoggedInRow = 0;

    public static int MAX_XP = 4000;
    public static int MAX_VIP_POINTS = 1100;
    public static int MAX_ENERGY = 3000;
    public static int MAX_IRON = 8000;
    public static int MAX_GOLD = 8000;
    public static int MAX_TIE = 8000;
    public static int MAX_OIL = 8000;
    public static int MAX_URANIUM = 8000;
    public static int MAX_GAZ = 8000;
    public static int ENERGY_NEEDED_FOR_BATTLE = 100;
    public Vector2Int worldMapPosition = new Vector2Int(0, 0);
    public List<BookMarkData> friendBookMarks = new List<BookMarkData>();
    public List<BookMarkData> favBookMarks = new List<BookMarkData>();
    public List<BookMarkData> enemyBookMarks = new List<BookMarkData>();


    public static Dictionary<int, Dictionary<RobotsNames, int>> levelsData = new Dictionary<int, Dictionary<RobotsNames, int>>()
    {
        { 0, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 2 } } },
        { 1, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 3 } } },
        { 2, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 6 } } },
        { 3, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 8 } } },
        { 4, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 10 } } },
        { 5, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 12 } } },
        { 6, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 16 } } },
        { 7, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 20 } } },
        { 8, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 22 } } },
        { 9, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 26 } } },
        { 10, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 28 } } },
        { 11, new Dictionary<RobotsNames, int>() { { RobotsNames.F1Hunter, 40 } } }
    };


    public int GetCurrentlevel()
    {
        return (xp / MAX_XP);
    }

    public void AddXP(int amount)
    {
        int currentLevel = GetCurrentlevel();
        xp += amount;
        int newLevel = GetCurrentlevel();

        if (currentLevel != newLevel)
        {
            GameManager.instance.OnLevelIncrease();
        }
    }
    public void AddVIPPoints(int amount)
    {
        int newAmount = vipPoints + amount;
        
        if(newAmount >= (MAX_VIP_POINTS * 12))
            vipPoints = (MAX_VIP_POINTS * 12);
        else
            vipPoints += amount;
    }
    public int GetVIPLevel()
    {
        return Mathf.Clamp((vipPoints / MAX_VIP_POINTS), 0,12);
    }
    public int GetRank()
    {
        return Mathf.Clamp((int)(powerNumber+1 / 1000f), 1,7);
    }
    public void AddEnergy(int amount)
    {
        if (energyAmount + amount >= 0)
            energyAmount += amount;
        else
            energyAmount = 0;

        Debug.Log("Energy Added " + amount);
    }
    public int GetCurrentStoryLevel()
    {
        int i = 0;

        foreach(var level in MainStoryProgress)
            if(level != 0)
                i++;

        return i;
    }
}


public class BookMarkData : IEquatable<BookMarkData> 
{
    public string playerName;
    public Vector2Int position;

    public bool Equals(BookMarkData other)
    {
        if((this.playerName ==  other.playerName) && (this.position == other.position))
            return true;
        else 
            return false;
    }
}

