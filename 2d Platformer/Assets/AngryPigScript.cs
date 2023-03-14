using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPigScript : Entity
{
    private CharacterController _controller;
    [SerializeField] private Transform first;
    [SerializeField] private Transform second;
    private Transform _transformHero;

    private Vector2 _firstPosition;
    private Vector2 _secondPosition;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private LayerMask playerLayer;
    private Rigidbody2D _rb;
    private float _speed = 1f;

    private float _direction = -1f;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _animator.SetBool("IsRunning", true);
        _firstPosition = first.position;
        _secondPosition = second.position;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _transformHero = Hero.Instance.transform;
    }

    private void Awake()
    {
        _lives = 50;
    }

    public override void GetDamage(int lives = 0, GameObject entity = null)
    {
        _lives -= lives;
        Hero.Instance.AttackSound();
        StartCoroutine(AnimationAttack(0));
        if (_lives < 0)
            StartCoroutine(AnimationAttack(1));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            if (Hero.Instance.CheckOnMonsterGround()) this.GetDamage(1);
            else Hero.Instance.GetDamage(1, this.gameObject);
        }
    }

    void Update()
    {
        Movement();
        RunningToHero();
    }

    private void Movement()
    {
        if (_direction < 0) _spriteRenderer.flipX = false;
        if (_direction > 0) _spriteRenderer.flipX = true;
        if (_firstPosition.x >= transform.position.x && _direction < 0)
        {
            if (_animator.GetBool("IsRunning"))
                StartCoroutine(AnimationStay());
            _direction *= -1f;
        }
        else if (_secondPosition.x <= transform.position.x && _direction > 0)
        {
            if (_animator.GetBool("IsRunning"))
                StartCoroutine(AnimationStay());
            _direction *= -1f;
        }
        if (_animator.GetBool("IsRunning"))
            _rb.velocity = new Vector2(_direction * 2 * _speed, 0);
    }

    private void RunningToHero()
    {
        if (_transformHero.position.y - transform.position.y <= 3f)
        {
            bool left = transform.position.x - _transformHero.position.x <= 5f &&
            _rb.velocity.x != 0f && _animator.GetBool("IsRunning") &&
            _firstPosition.x < _transformHero.position.x;
            bool right = _transformHero.position.x - transform.position.x <= 5f &&
                _rb.velocity.x != 0f && _animator.GetBool("IsRunning") &&
                _secondPosition.x > _transformHero.position.x;
            if (left &&
            transform.position.x - _transformHero.position.x > 0.1f)
            {
                _animator.SetBool("IsRunningToHero", true);
                _speed = 1.5f;
                _direction = -1f;
            }
            else if (right &&
                _transformHero.position.x - transform.position.x > 0.1f) // right
            {
                _animator.SetBool("IsRunningToHero", true);
                _speed = 1.5f;
                _direction = 1f;
            }
            else if (left || right)
            {

            }
            else
            {
                _speed = 1f;
                _animator.SetBool("IsRunningToHero", false);
            }
        }
    }

    private IEnumerator AnimationStay()
    {
        _animator.SetBool("IsRunning", false);
        _animator.SetBool("IsRunningToHero", false);
        _speed = 1f;
        yield return new WaitForSeconds(1f);
        _animator.SetBool("IsRunning", true);
    }

    private IEnumerator AnimationAttack(int i)
    {
        _animator.SetBool("IsDamaged", true);
        yield return new WaitForSeconds(0.4f);
        _animator.SetBool("IsDamaged", false);
        if (i == 1) Die();
    }
}
