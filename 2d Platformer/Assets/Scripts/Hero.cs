using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource attackAirSound;
    [SerializeField] private AudioSource attackSound;

    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private float _jump;
    [SerializeField] private Image[] _hearts;
    private int _health;
    public float maxVelocityX;
    [SerializeField] private Sprite _aliveHeart;
    [SerializeField] private Sprite _deadHeart;
    public int NumberHero;
    private bool _dead;
    public static Hero Instance { get; set; }

    private bool _flipped;

    public bool IsMonster;
    private bool _isDamaged;
    public bool IsFalling;
    [SerializeField]private bool _IsDoobleJump = false;
    [SerializeField]private bool _doobleJump = false;

    public Transform groundCheckTransformFirst;
    public Transform groundCheckTransformSecond;
    public bool isGrounded;
    public LayerMask groundCheckLayerMask;
    public LayerMask layerMonster;
    private Animator animator;

    public bool IsAttacking = false;
    public bool IsReacharged = true;

    public Transform AttackPos;
    public float AttackRange;

    private Rigidbody2D rb;
    public SpriteRenderer Sprite;

    public bool isRunning;
    private void Awake()
    {
        if (NumberHero != SaveData.MainCharacher) gameObject.active = false;
        if (MusicController.Instance != null) Destroy(MusicController.Instance.gameObject);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Instance = this;
        IsReacharged = true;
        _lives = 5;
        _health = _lives;
    }

    private void Update()
    {
        IsFalling = rb.velocity.y < 0.0f;
        animator.SetBool("IsFalling", IsFalling);
        if (Input.GetButtonDown("Jump") && isGrounded) 
            Jump();

        LogicDoobleJump();

        if (Input.GetButtonDown("Jump") && _IsDoobleJump)
            DoobleJump();

        if (Input.GetButton("Fire1") && !isRunning)
            Attack();
        animator.SetBool("IsAttacking", IsAttacking);

        if (_health > _lives) _health = _lives;
        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i < _health) _hearts[i].sprite = _aliveHeart;
            else _hearts[i].sprite = _deadHeart;
        }
    }

    private void LogicDoobleJump()
    {
        if (!isGrounded && !_doobleJump)
            StartCoroutine(CanDoobleJump());

        if (isGrounded)
        {
            _doobleJump = false;
            _IsDoobleJump = false;
        }
    }

    private IEnumerator CanDoobleJump()
    {
        yield return new WaitForSeconds(0.01f);
        _doobleJump = true;
        _IsDoobleJump = true;
    }

    private void DoobleJump()
    {
        rb.velocity = Vector2.up * _jumpForce;
        jumpSound.Play();
        Debug.Log("DoobleJump");
        StartCoroutine(AnimationDoobleJump());
        _IsDoobleJump = false;
    }

    private IEnumerator AnimationDoobleJump()
    {
        animator.SetBool("IsDoobleJump", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("IsDoobleJump", false);
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * _jumpForce;
        jumpSound.Play();
    }

    void FixedUpdate()
    {
        CheckGround();
        if (!_dead)
            Run();
    }

    private void Run()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * (_speed/50.0f), 0.0f);
        transform.Translate(movement);
        Vector3 vectorAttack = new Vector3(1.6f, 0);
        if (movement.x < 0)
        {
            Sprite.flipX = true;
            if (transform.position.x - 0.75f < AttackPos.transform.position.x)
                AttackPos.transform.position -= vectorAttack;
        }
        else if (movement.x > 0)
        {
            Sprite.flipX = false;
            if (transform.position.x + 0.75f > AttackPos.transform.position.x)
                AttackPos.transform.position += vectorAttack;
        }
        isRunning = Input.GetAxis("Horizontal") != 0;
        animator.SetBool("IsRunning",isRunning);
    }

    private void CheckGround()
    {
        CheckOnMonsterGround();
        isGrounded = Physics2D.Raycast(
            groundCheckTransformFirst.position, Vector2.down, 0.02f, groundCheckLayerMask)
            .collider != null || Physics2D.Raycast(
            groundCheckTransformSecond.position, Vector2.down, 0.02f, groundCheckLayerMask)
            .collider != null || IsMonster;
        animator.SetBool("IsGrounded", isGrounded);
    }

    public bool CheckOnMonsterGround()
    {
        IsMonster = Physics2D.Raycast(
            groundCheckTransformFirst.position, Vector2.down, 0.02f, layerMonster)
            .collider != null || Physics2D.Raycast(
            groundCheckTransformSecond.position, Vector2.down, 0.02f, layerMonster)
            .collider != null;
        return IsMonster;
    }
    public void AttackSound()
    {
        attackSound.Play();
    }

    public override void GetDamage(GameObject entity = null)
    {
        _lives -= 1;
        if (_lives == 0)
        {
            foreach (var item in _hearts)
            {
                item.sprite = _deadHeart;
            }
        }

        if (_lives < 1) StartCoroutine(AnimationAndDeath());
        else
        {
            damageSound.Play();
            StartCoroutine(Hitting());
            Vector2 direction = (entity.transform.position - transform.position).normalized.x < 0 ?
                Vector2.right : Vector2.left;
            rb.AddForce((Vector2.up + direction) * _jump, ForceMode2D.Impulse);
            if (rb.velocity.magnitude >= maxVelocityX)
                rb.velocity = rb.velocity.normalized * maxVelocityX;
        }
    }

    private IEnumerator AnimationAndDeath()
    {
        rb.bodyType = RigidbodyType2D.Static;
        _dead = true;
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsDead", false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Die();
    }

    public void Attack()
    {
        if (isGrounded && IsReacharged)
        {
            IsAttacking = true;
            IsReacharged= false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    public virtual void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, layerMonster);
        if (colliders.Length == 0) attackAirSound.Play();
        else attackSound.Play();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(1.5f);
        IsReacharged = true;
    }
    private IEnumerator Hitting()
    {
        animator.SetBool("IsDamaged", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("IsDamaged", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) this.GetDamage(collision.gameObject);
    }
}
