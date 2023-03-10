using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RinoScript : Entity
{
    [SerializeField] private LayerMask layerGround; 
    [SerializeField] private float speed = 7f;
    private Animator _animator;
    private float _direction = -1f;
    private bool _flipSprite;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;
    private bool _isRunning = true;

    private void Start()
    { 
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        _lives = 5;
        _animator = GetComponent<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Collider2D collider = Physics2D.Raycast(new Vector2(
            transform.position.x + 1.15f * _direction, transform.position.y),
            new Vector2(_direction,0), 0.2f, layerGround)
            .collider;
        if (_isRunning && collider != null)
        {
            StartCoroutine(ReboundRino());
            StartCoroutine(AnimationHitByWall());
        }
        if (_isRunning)
            _rb.velocity = new Vector2(_direction * speed, _rb.velocity.y);

    }

    private IEnumerator ReboundRino()
    {
        _isRunning = false;
        _animator.SetBool("IsRunning", _isRunning);
        _direction *= -1f;
        _rb.velocity = (new Vector2(_direction,0) + Vector2.up)*4f;
        yield return new WaitForSeconds(2f);
        _flipSprite = !_flipSprite;
        _sprite.flipX = _flipSprite;
        _isRunning = true;
        _animator.SetBool("IsRunning", _isRunning);
    }

    private IEnumerator AnimationHitByWall()
    {
        _animator.SetBool("IsHittedByWall", true);
        yield return new WaitForSeconds(0.7f);
        _animator.SetBool("IsHittedByWall", false);
    }

    public override void GetDamage(int lives = 0,GameObject entity = null)
    {
        StartCoroutine(AnimatorDamage());
        _lives--;
        Hero.Instance.AttackSound();
        Debug.Log("Ó Entity: " + _lives);
        if (_lives < 1) 
            Die();
    }

    private IEnumerator AnimatorDamage()
    {
        _animator.SetBool("IsDamaged", true);
        yield return new WaitForSeconds(0.4f);
        _animator.SetBool("IsDamaged", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            if (!Hero.Instance.CheckOnMonsterGround())
            {
                Hero.Instance.GetDamage(1,this.gameObject);
                if (_isRunning)
                {
                    StartCoroutine(AnimationHitByWall());
                    StartCoroutine(ReboundRino());
                }
            }
            else 
                this.GetDamage(1);
        }
    }
}
