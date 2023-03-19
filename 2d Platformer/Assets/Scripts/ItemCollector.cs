using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int Apples = 0;
    public static int CollectedAttack = 0;
    public ItemCollector Instance;
    [SerializeField] private Text text;
    [SerializeField] private AudioSource audioGold;
    [SerializeField] private GameObject platformScript;
    [SerializeField] private GameObject fanScript;
    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            audioGold.Play();
            StartCoroutine(IsCollected(collision));
            Apples++;
            text.text = "Apples: " + Apples;
        }
        else if (collision.gameObject.CompareTag("Attack"))
        {
            audioGold.Play();
            StartCoroutine(IsCollected(collision));
            CollectedAttack++;
        }
        else if (collision.gameObject.CompareTag("MovementPlatforms"))
        {
            audioGold.Play();
            StartCoroutine(IsCollected(collision));
            platformScript.GetComponent<SawMove>().enabled = true;
        }
        else if (collision.gameObject.CompareTag("MovementFan"))
        {
            audioGold.Play();
            StartCoroutine(IsCollected(collision));
            fanScript.GetComponent<ScriptFan>().enabled = true;
        }    
    }

    private IEnumerator IsCollected(Collider2D collision)
    {
        collision.GetComponent<Animator>().SetBool("Collected", true);
        collision.enabled = true;
        yield return new WaitForSeconds(0.4f);
        Destroy(collision.gameObject);
    }
}
