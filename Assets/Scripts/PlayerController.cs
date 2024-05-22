using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public Rigidbody2D rb;
    public Transform pivotPoint;
    public GameManager gameManager;
    public GameObject player;
    private float cooldown = 1f;
    private bool isFireCooldown = false;
    private bool isGrounded;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private GameObject activeBullet;

    Vector2 movementDirection;
    Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //restart level
        }

        //Fires Gun
        if (Input.GetMouseButtonDown(0) && !isFireCooldown)
        {
            Fire();
            StartCoroutine(FireCooldown());
        }

        if (Input.GetMouseButtonDown(1))
        {
            Teleport();
        }

        //If Player dies
    }

    private void FixedUpdate()
    {

        //player Aiming
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        pivotPoint.rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle));
    }

    public void Fire()
    {
        if (activeBullet == null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            Bullet bulletObj = bullet.GetComponent<Bullet>();

            Vector2 shootingDirection = firePoint.up;
            bulletObj.SetVelocity(shootingDirection);

            activeBullet = bullet;

            bulletObj.OnDestroyed += HandleBulletDestroyed;
        }

    }

    public void Teleport()
    {
        if (activeBullet != null)
        {
            // Teleport the player to the bullet's position
            transform.position = activeBullet.transform.position;

            // Destroy the bullet
            Destroy(activeBullet);
            activeBullet = null; // Reset the active bullet reference
        }
    }

    void HandleBulletDestroyed()
    {
        activeBullet = null; // Reset the active bullet reference when the bullet is destroyed
    }

    IEnumerator FireCooldown()
    {
        isFireCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isFireCooldown = false;
    }
}