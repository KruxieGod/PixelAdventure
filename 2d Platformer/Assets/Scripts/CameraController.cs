using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 pos;

    private void Awake()
    {
        if (!_player)
            _player = FindObjectOfType<Hero>().transform;
    }

    private void FixedUpdate()
    {
        pos = _player.position;
        pos.z = -10f;
        pos.y += 1f;
        transform.position = Vector3.Lerp(transform.position, pos,Time.deltaTime);
    }
}
