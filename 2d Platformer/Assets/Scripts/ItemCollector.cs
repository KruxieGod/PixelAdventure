using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int Apples = 0;
    public ItemCollector Instance;
    [SerializeField] Text text;
    [SerializeField] AudioSource audioGold;
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
    }

    private IEnumerator IsCollected(Collider2D collision)
    {
        collision.GetComponent<Animator>().SetBool("Collected", true);
        collision.enabled = true;
        yield return new WaitForSeconds(0.4f);
        Destroy(collision.gameObject);
    }
}
