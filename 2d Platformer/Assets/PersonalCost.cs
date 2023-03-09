using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PersonalCost : MonoBehaviour
{
    public static void SelectCharacherFirst()
    {
        SaveData.MainCharacher= 0;
    }

    public static void SelectCharacherSecond()
    {
        if (SaveData.AmountMoney >= 5 && !SaveData._checker[0])
        {
            SaveData.MainCharacher = 1;
            SaveData.AmountMoney -= 5;
            SaveData._checker[0] = true;
        }
        else if (SaveData.AmountMoney < 5 && !SaveData._checker[0]) return;
        else SaveData.MainCharacher = 1;
        Debug.Log(SaveData.MainCharacher);
    }

    public static void SelectCharacherThird()
    {
        if (SaveData.AmountMoney >= 10 && !SaveData._checker[1])
        {
            SaveData.MainCharacher = 2;
            SaveData.AmountMoney -= 10;
            SaveData._checker[1] = true;
        }
        else if (SaveData.AmountMoney < 10 && !SaveData._checker[1]) return;
        else SaveData.MainCharacher = 2;
        Debug.Log(SaveData.MainCharacher);

    }

    public static void SelectCharacherFourth()
    {
        if (SaveData.AmountMoney >= 15 && !SaveData._checker[2])
        {
            SaveData.MainCharacher = 3;
            SaveData.AmountMoney -= 15;
            SaveData._checker[2] = true;
        }
        else if (SaveData.AmountMoney < 15 && !SaveData._checker[2]) return;
        else SaveData.MainCharacher = 3;
        Debug.Log(SaveData.MainCharacher);
    }
}
