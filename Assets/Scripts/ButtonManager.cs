using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public bool isPressed;
    public bool isRotating;
    public Rigidbody2D rb;
    public Rigidbody2D target;
    public Transform pivot;
    public float rotationSpeed = 100f; // Rotation speed in degrees per second
    private float targetAngle = 90f; // The angle to rotate to
    private float currentAngle = 0f; // The current rotation angle
    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        isRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
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

            target.transform.RotateAround(pivot.position, Vector3.forward, rotationThisFrame);
            currentAngle += rotationThisFrame;
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
