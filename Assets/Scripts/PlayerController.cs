using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float acceleration = 15f;
    public float jumpForce = 5.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask redLayer;

    public Rigidbody2D rb;
    public Transform pivotPoint;
    public GameManager gameManager;
    public GameObject player;
    private float cooldown = 1f;
    private bool isFireCooldown = false;
    private bool isGrounded;
    private bool  isReverse = false;
    public bool isFirePointInNoShootZone = false;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private GameObject activeBullet;
    public TextMeshProUGUI bulletTypeText;

    Vector2 movementDirection;
    Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //default state for bullet text UI
        bulletTypeText.text = "Normal";
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, redLayer);

        //Player Movement
        float moveInput = Input.GetAxis("Horizontal");
        if (isGrounded && moveInput != 0)
        {
            // Apply acceleration
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, moveInput * moveSpeed, acceleration * Time.deltaTime), rb.velocity.y);
        }
        else if(isGrounded)
        {
            // Apply deceleration
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, acceleration * Time.deltaTime), rb.velocity.y);
        }

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //Reload Level
        if (Input.GetKeyDown(KeyCode.R) && !PauseMenu.isPaused)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //switch bullet type
        if (Input.GetKeyDown(KeyCode.Q) && !PauseMenu.isPaused)
        {
            isReverse = !isReverse;
            //changes ui element for bullet type
            if(isReverse)
            {
                bulletTypeText.text = "Reverse";
            }
            else
            {
                bulletTypeText.text = "Normal";
            }
        }

        //Fires Gun
        if (Input.GetMouseButtonDown(0) && !isFireCooldown && !PauseMenu.isPaused && !isFirePointInNoShootZone)
        {
            // Find every instace tagged Bullet and destroys it
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets)
            {
                Destroy(bullet);
            }

            Fire();
            StartCoroutine(FireCooldown());
        }

        //Teleport to bullet
        if (Input.GetMouseButtonDown(1) && !PauseMenu.isPaused)
        {
            Teleport();
        }

        CheckFirePointPosition();
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

            //if isReversed then flip velocity
            if(isReverse)
            {
                rb.velocity = -rb.velocity;
            }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    
    private void CheckFirePointPosition()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(firePoint.position);
        isFirePointInNoShootZone = false;

        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == "NoShoot")
            {
                isFirePointInNoShootZone = true;
                break;
            }
        }
    }
}