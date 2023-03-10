using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RockHead : Entity
{
    private Animator _animator;
    private bool _isBlinking = false;
    private bool _isReacharged = true;
    private void Awake()
    {
        _lives = 5;
        _animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isReacharged)
        {

            StartCoroutine(Blinking());
            StartCoroutine(NoBlinking());
        }
        _animator.SetBool("IsBlinking", _isBlinking);
    }

    private IEnumerator Blinking()
    {
        _isBlinking = true;
        yield return new WaitForSeconds(0.5f);
        _isBlinking = false;
    }

    private IEnumerator NoBlinking() 
    {
        _isReacharged = false;
        yield return new WaitForSeconds(4f);
        _isReacharged = true;
    }

    public override void GetDamage(int lives = 0 ,GameObject entity = null)
    {
        _animator.SetBool("Hit", true);
        StartCoroutine(Hitting());
        Hero.Instance.AttackSound();
        _lives--;
        Debug.Log("Ó Entity: " + _lives);
        if (_lives < 1)
            Die();
    }

    private IEnumerator Hitting()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("Hit", false);
    }
}
