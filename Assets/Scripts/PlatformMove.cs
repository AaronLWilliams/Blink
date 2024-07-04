using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform point;
    public bool isReversed;
    //For Rotate
    public float rotationSpeed = 100f; // Rotation speed in degrees per second
    public float currentAngle = 0f; // The current rotation angle
    //For Move
    public float moveSpeed = 2f; // Speed of the door movement
    public Transform targetPosition; // Target position to move to
    public enum State
    {
        rotate,
        move
    }
    public State currentState;
    // Start is called before the first frame update
    void Start()
    {
        if (isReversed)
            rotationSpeed = -rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.rotate:
                float rotationThisFrame = rotationSpeed * Time.deltaTime;

                rb.transform.RotateAround(point.position, Vector3.forward, rotationThisFrame);
                currentAngle += rotationThisFrame;
                break;
            case State.move:
                // Calculate direction and velocity
                Vector3 direction = (targetPosition.position - rb.transform.position).normalized;
                Vector3 velocity = direction * moveSpeed;

                // Apply velocity to Rigidbody
                rb.velocity = velocity;

                // Check if the Rigidbody is close enough to the target position
                if (Vector3.Distance(rb.transform.position, targetPosition.position) < 0.001f)
                {
                    rb.transform.position = targetPosition.position; // Ensure exact position
                    rb.velocity = Vector3.zero; // Stop the Rigidbody

                    // Swap Point and TargetPosition
                    point.position = point.position + targetPosition.position;
                    targetPosition.position = point.position - targetPosition.position;
                    point.position = point.position - targetPosition.position;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
