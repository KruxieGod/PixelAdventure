using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private bool _canChange;
    private bool _isWas;
    private Animator _animator;
    private Vector2 _position;
    private float _direction = 1f;
    private bool _isRest = true;
    void Start()
    {
        _position = transform.position;
        _animator= GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_position.y+1f <= transform.position.y && _isRest && _direction > 0)
        {
            StartCoroutine(Idle());
            _direction *= -1f;
        }
        if (_position.y >= transform.position.y && _direction <0 && _isRest)
        {
            StartCoroutine(Idle());
            _direction *= -1f;
        }
        if (!_isWas && _isRest)
        {
            _rb.velocity = new Vector2(0, _direction);
        }
        if (_canChange)
            ChangingSprite();
        if (_rb.gravityScale == 3f)
            transform.Rotate(0,0,1);
    }

    private IEnumerator Idle()
    {
        _rb.velocity = Vector2.zero;
        _isRest = false;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.9f));
        _isRest = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isWas = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StartCoroutine(AnimationOfFalling(0));
    }

    private void ChangingSprite()
    {
        _spriteRenderer.color = new Color(_spriteRenderer.color.r,
            _spriteRenderer.color.g,
            _spriteRenderer.color.b,
            _spriteRenderer.color.a-(1f/500f));
    }

    private IEnumerator AnimationOfFalling(float value)
    {
        yield return new WaitForSeconds(value);
        _canChange = true;
        yield return new WaitForSeconds(value/5f);
        _animator.SetBool("IsDead", true);
        _rb.gravityScale = 3f;
        _boxCollider.enabled = false;
        
    }
}
