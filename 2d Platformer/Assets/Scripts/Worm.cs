using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Worm: Entity
{
    private void Start()
    {
        _lives = 3;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            if (!Hero.Instance.CheckOnMonsterGround()) 
                Hero.Instance.GetDamage(1,this.gameObject);
            else 
                this.GetDamage();
        }
    }
}
