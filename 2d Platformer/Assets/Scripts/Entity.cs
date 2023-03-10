using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int _lives;
    public virtual void GetDamage(GameObject entity = null)
    {
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
}
