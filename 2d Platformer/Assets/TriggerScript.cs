using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private void Start()
    {
        enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.attachedRigidbody.velocity = Vector2.up*4f;
    }
}
