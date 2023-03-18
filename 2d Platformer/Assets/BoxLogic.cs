using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLogic : MonoBehaviour
{
    [SerializeField] private Rigidbody2D otherDown;
    [SerializeField] private Rigidbody2D otherUp;
    [SerializeField] private Rigidbody2D leftBreak;
    [SerializeField] private Rigidbody2D rightBreak;
    private Animator animator;

    [SerializeField] private Rigidbody2D skillAttack;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Hero.Instance.IsGrounded() &&
            Hero.Instance.GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            StartCoroutine(AnimationHitting());
        }
    }

    private IEnumerator AnimationHitting()
    {
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(AppearSkillAttack());
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        EnableAllBodies();
        animator.SetBool("Hit", false);
    }

    private void EnableAllBodies()
    {
        otherDown.gameObject.SetActive(true);
        otherUp.gameObject.SetActive(true);
        leftBreak.gameObject.SetActive(true);
        rightBreak.gameObject.SetActive(true);
        otherDown.velocity = (Vector2.up + Vector2.left)*Random.Range(1.2f,1.7f);
        otherUp.velocity = (Vector2.up + Vector2.right) * Random.Range(1.2f, 1.7f);
    }

    private IEnumerator AppearSkillAttack()
    {
        skillAttack.gameObject.SetActive(true);
        skillAttack.velocity = Vector2.up * 7f;
        yield return new WaitForSeconds(1f);
        skillAttack.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        skillAttack.GetComponent<BoxCollider2D>().enabled = true;
    }
}
