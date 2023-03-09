using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int _lives;
    public virtual void GetDamage(GameObject entity = null)
    {
        StartCoroutine(EnemyOnAttack(this.GetComponent<Collider2D>()));
        Hero.Instance.AttackSound();
        _lives--;
        Debug.Log("Ó Entity: "+_lives);
        if (_lives<1)
            Die();
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    public static IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
        Color pastColor = new Color(enemyColor.color.r, enemyColor.color.g, enemyColor.color.b);
        enemyColor.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemyColor.color = pastColor;
    }
}
