using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] private Transform aroundObj;
    [SerializeField] private float rotSpeed = 0.1f;
    [SerializeField] private float height = 2f;
    private Vector2 _positionFirst;
    private float _direction = 1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _positionFirst = transform.position;
    }

    void Update()
    {
        Rotator();
    }

    void Rotator()
    {
        if (transform.position.y >= _positionFirst.y + height)
            _direction *= -1f;
        transform.RotateAround(aroundObj.position, new Vector3(0, 0, _direction), rotSpeed);
        _spriteRenderer.transform.RotateAround(_spriteRenderer.transform.position, new Vector3(0, 0, _direction), rotSpeed);
    }
}