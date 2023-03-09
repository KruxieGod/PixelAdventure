using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayCurrentLevel()
    {
        if (SaveData.GetInt("levelReached") == 0) return;
        else SceneManager.LoadScene(SaveData.GetInt("levelReached") + 2);
    }

    public void OpenLevelsList()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenPersonal()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }
}
