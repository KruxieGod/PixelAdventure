using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ScriptFan : MonoBehaviour
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private ParticleSystem particle;
    private Animator _animator;
    private bool _isReload = true;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        trigger.SetActive(false);
    }

    private void Update()
    {
        if (_isReload)
            StartCoroutine(AnimationAir());
    }

    private IEnumerator AnimationAir()
    {
        _isReload = false;
        SetOptions(true);
        yield return new WaitForSeconds(Random.Range(2.7f,4f));
        SetOptions(false);
        yield return new WaitForSeconds(Random.Range(4f, 5f));
        _isReload = true;
    }

    private void SetOptions(bool opt)
    {
        trigger.SetActive(opt);
        if (opt)
            particle.startLifetime = 1.5f;
        else
            particle.startLifetime = 0f;
        _animator.SetBool("IsMoving", opt);
    }
}
