using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptFan : MonoBehaviour
{
    [SerializeField] private GameObject trigger;
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
        _animator.SetBool("IsMoving", true);
        _isReload = false;
        trigger.SetActive(true);
        yield return new WaitForSeconds(Random.Range(2.7f,4f));
        trigger.SetActive(false);
        _animator.SetBool("IsMoving", false);
        yield return new WaitForSeconds(Random.Range(4f, 5f));
        _isReload = true;
    }
}
