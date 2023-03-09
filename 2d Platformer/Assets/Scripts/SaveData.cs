using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    public static int AmountMoney = 0;
    public static int MainCharacher = 0;
    public static bool[] _checker = new bool[3];
    private static Dictionary<string,int> levelReached = new Dictionary<string,int>();
    public static int GetInt(string name, int defaultValue = 0)
    {
        if (!levelReached.ContainsKey(name)) return defaultValue;
        else return levelReached[name];
    }

    public static void SetInt(string name, int value)
    {
        levelReached[name] = value;
    }
}
