using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levels;
    private void Start()
    {
        int levelReached = SaveData.GetInt("levelReached", 1);
        Debug.Log(levelReached);
        for (int i = 0; i < levels.Length; i++)
        {
            if (i + 1 > levelReached) levels[i].interactable = false;
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static void Select(int numberInBuild)
    {
        SceneManager.LoadScene(numberInBuild);
        Destroy(GameObject.Find("Audio Source"));
    }
}
