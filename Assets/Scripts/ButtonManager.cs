using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private bool isPressed;
    private bool isRotating;
    public bool isMoving;
    private bool hasMoved;
    public Rigidbody2D rb;
    public Rigidbody2D target;
    public Transform point;
    //For Rotate
    public float rotationSpeed = 100f; // Rotation speed in degrees per second
    public float targetAngle = 90f; // The angle to rotate to
    public float currentAngle = 0f; // The current rotation angle
    //For Move
    public float moveDistance = 5f; // Distance to move the door
    public float moveSpeed = 2f; // Speed of the door movement
    public Transform targetPosition; // Target position to move to
    public enum State
    {
        rotate,
        move,
        dissapear
    }
    public State currentState;
    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        isRotating = false;
        isMoving = false;
        hasMoved = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.rotate:
                if (isPressed && !isRotating)
                {
                    isRotating = true;
                }

                if (isRotating)
                {
                    float rotationThisFrame = rotationSpeed * Time.deltaTime;
                    if (currentAngle + rotationThisFrame >= targetAngle)
                    {
                        rotationThisFrame = targetAngle - currentAngle;
                        isRotating = false;
                    }

                    target.transform.RotateAround(point.position, Vector3.forward, rotationThisFrame);
                    currentAngle += rotationThisFrame;
                }
                break;
            case State.move:
                //for move
                if (isPressed && !isMoving && !hasMoved)
                {
                    isMoving = true;
                    hasMoved = true;
                }
                if (isMoving)
                {
                    float step = moveSpeed * Time.deltaTime; // Calculate movement step
                    target.transform.position = Vector3.MoveTowards(target.transform.position, targetPosition.position, step);

                    if (Vector3.Distance(target.transform.position, targetPosition.position) < 0.001f)
                    {
                        target.transform.position = targetPosition.position; // Ensure exact position
                        isMoving = false; // Stop moving after reaching target position
                    }
                }
                break;
            case State.dissapear:
                if (isPressed && target != null)
                    Destroy(target.gameObject);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && (other.gameObject.tag == "Player" || other.gameObject.tag == "Bullet"))
        {
            isPressed = true;
            //change sprite
        }
    }
}
