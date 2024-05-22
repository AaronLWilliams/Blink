using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    private Vector3 velocityBeforePhysicsUpdate;

    // Event to notify when the bullet is destroyed
    public event Action OnDestroyed;

    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rb.velocity;
    }

    public void SetVelocity(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the collision normal
        Vector2 normal = collision.contacts[0].normal;

        // Reverse the velocity component based on the normal direction
        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            // Collision on the left or right side, flip the x component
            rb.velocity = new Vector2(-velocityBeforePhysicsUpdate.x, velocityBeforePhysicsUpdate.y);
        }
        else
        {
            // Collision on the top or bottom side, flip the y component
            rb.velocity = new Vector2(velocityBeforePhysicsUpdate.x, -velocityBeforePhysicsUpdate.y);
        }
    }

    void OnDestroy()
    {
        // Notify that the bullet is destroyed
        if (OnDestroyed != null)
        {
            OnDestroyed();
        }
    }
}
