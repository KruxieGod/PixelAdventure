using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenScript : Entity
{
    [SerializeField] private Transform first;
    [SerializeField] private Transform second;

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
        _animator= GetComponent<Animator>();
        _animator.SetBool("IsRunning", true);
        _firstPosition = first.position;
        _secondPosition = second.position;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Awake()
    {
        _lives = 3;
    }

    public override void GetDamage(int lives = 0, GameObject entity = null)
    {
        _lives -= lives;
        Hero.Instance.AttackSound();
        StartCoroutine(AnimationAttack(0));
        if (_lives <0)
            StartCoroutine(AnimationAttack(1));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            if (Hero.Instance.CheckOnMonsterGround()) this.GetDamage(1);
            else Hero.Instance.GetDamage(1,this.gameObject);
        }    
    }

    void Update()
    {
        Movement();
        RunningFromHero();
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
            _rb.velocity = new Vector2(_direction*2*_speed, 0);
    }

    private void RunningFromHero()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 3f, playerLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 3f, playerLayer);

        if (hitLeft.collider != null)
        {
            _speed = 1.5f;
            _direction = 1f;
        }
        else if (hitRight.collider != null)
        {
            _speed = 1.5f;
            _direction = -1f;
        }
        else _speed = 1f;
    }

    private IEnumerator AnimationStay()
    {
        _animator.SetBool("IsRunning", false);
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
