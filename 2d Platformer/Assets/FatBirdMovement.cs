using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FatBirdMovement : Entity
{
    private Rigidbody2D _rb;
    [SerializeField] private Transform positionEnd;
    private Vector2 _positionEnd;
    private bool _isGrounded;
    private bool _isFalling = false;
    private bool _isFlying = true;
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();   
    }

    private void Awake()
    {
        _positionEnd = positionEnd.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnGround();
        _animator.SetBool("IsFalling", _isFalling);
        _animator.SetBool("IsFlying", _isFlying);
        if (transform.position.y >= _positionEnd.y)
        {
            _isFlying = false;
            _isFalling = true;
            _rb.gravityScale = 1f;
        }
        if (_isGrounded)
        {
            StartCoroutine(AnimationIdleGround());
        }
        if (!_isFalling)
        {
            _isFlying = true;
            _rb.velocity = Vector2.up;
        }
    }

    private void CheckOnGround()
    {
        _isGrounded = Physics2D.Raycast(transform.position,Vector2.down,0.1f).collider != null;
    }

    private IEnumerator AnimationIdleGround()
    {
        _animator.SetBool("IsGrounded", true);
        yield return new WaitForSeconds(2f);
        _animator.SetBool("IsGrounded", false);
        _rb.gravityScale = 0f;
        _isFalling = false;
    }
}
