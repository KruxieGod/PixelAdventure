using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private int levelReached;
    [SerializeField] private AudioSource audioFinish;
    [SerializeField] private ItemCollector[] hero;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == hero[SaveData.MainCharacher].Instance.gameObject)
        {
            audioFinish.Play();
            if (SaveData.GetInt("levelReached") == 0 || SaveData.GetInt("levelReached") == levelReached - 1)
                SaveData.AmountMoney += hero[SaveData.MainCharacher].Apples;
            Debug.Log(SaveData.AmountMoney);
            StartCoroutine(LoadScrollMenu());
        }
    }

    private IEnumerator LoadScrollMenu()
    {
        SaveData.SetInt("levelReached", levelReached);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
