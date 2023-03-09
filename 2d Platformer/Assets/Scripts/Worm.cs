using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Worm: Entity
{
    public Worm Instance;
    private void Start()
    {
        Instance = this;
        _lives = 3;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            if (!Hero.Instance.CheckOnMonsterGround()) 
                Hero.Instance.GetDamage(Instance.gameObject);
            else 
                this.GetDamage();
        }
    }
}
