using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMove : MonoBehaviour
{
    [SerializeField] private Transform first;
    [SerializeField] private Transform last;
    [SerializeField] private float _speed;
    private Vector3 _firstPosition;
    private Vector3 _lastPosition;
    private bool _directionUp = true;
    public bool _isReacharged = true;
    public bool IsRepeated = true;
    public bool RotationHorizontal;
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        _firstPosition = first.position;
        _lastPosition = last.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 positionXY = new Vector3(transform.position.y, _lastPosition.y, _firstPosition.y);
        if (RotationHorizontal) positionXY = new Vector3(transform.position.x, _lastPosition.x, _firstPosition.x);
        if (positionXY.x > positionXY.y && IsRepeated)
        {
            _directionUp = false;
            StartCoroutine(Moving());
            StartCoroutine(Repeated());
        }

        else if (positionXY.x < positionXY.z && IsRepeated)
        {
            _directionUp = true;
            StartCoroutine(Moving());
            StartCoroutine(Repeated());
        }

        if (_isReacharged)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = _directionUp;
            animator.SetBool("Moving", true);
            float direction = _directionUp ? 1f : -1f;
            Vector3 movement = new Vector3(0, (_speed / 50.0f) * direction);
            if (RotationHorizontal) movement = new Vector3((_speed / 50.0f) * direction,0);
            transform.Translate(movement);
        }
    }

    private IEnumerator Moving()
    {
        _isReacharged = false;
        animator.SetBool("Moving", false);
        yield return new WaitForSeconds(1f);
        _isReacharged = true;
    }

    private IEnumerator Repeated()
    {
        IsRepeated = false;
        yield return new WaitForSeconds(1.1f);
        IsRepeated = true;
    }
}
