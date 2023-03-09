using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;
using UnityEngine;

public class WalkingMonster : Entity
{
    public WalkingMonster Instance;
    private float _speed = 0.5f;
    private Vector3 dir;
    [SerializeField]private SpriteRenderer sprite;
    Animator animator;

    private void Start()
    {
        Instance = this;
        _lives = 5;
        dir = transform.right;
        animator = sprite.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position + transform.up*0.1f + 
            transform.right*dir.x*0.7f,0.1f);

        if (colliders.Length > 0) dir *= -1f;
        sprite.flipX = dir.x > 0f;
        animator.SetBool("IsRunning", true);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, _speed/10);
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
