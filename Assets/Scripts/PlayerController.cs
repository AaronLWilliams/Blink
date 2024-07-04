using System;
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
    private Vector3 previousPlatformPosition;
    private Transform platformTransform;
    public GameManager gameManager;
    public GameObject player;
    private float cooldown = 1f;
    private bool isFireCooldown = false;
    private bool isGrounded;
    private bool  isReverse = false;
    public static bool isInNoTeloportZone = false;
    public bool isFirePointInNoShootZone = false;
    private bool isDead;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private GameObject activeBullet;
    public TextMeshProUGUI bulletTypeText;
    public GameObject gameOver;

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip landingSound;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    Vector2 movementDirection;
    Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        //default state for bullet text UI
        bulletTypeText.text = "Normal";
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, redLayer);

        if (!isDead)
        {
            //Player Movement
            float moveInput = Input.GetAxis("Horizontal");
            if (isGrounded && moveInput != 0)
            {
                // Apply acceleration
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, moveInput * moveSpeed, acceleration * Time.deltaTime), rb.velocity.y);
            }
            else if (isGrounded)
            {
                // Apply deceleration
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, acceleration * Time.deltaTime), rb.velocity.y);
            }

            // Jumping
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                PlaySound(jumpSound);
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
                if (isReverse)
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
            if (Input.GetMouseButtonDown(1) && !PauseMenu.isPaused && !isInNoTeloportZone)
            {
                Teleport();
            }

            bool isWalking = Mathf.Abs(moveInput) > 0.1f;
            animator.SetBool("isWalking", isWalking);

            if (moveInput > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        

        CheckFirePointPosition();
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            //player Aiming
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDirection = mousePosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            pivotPoint.rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle));
        }
        
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

    void PlaySound(AudioClip clip)
    {
        if (jumpSound != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death" && !isDead)
        {
            isDead = true;
            PlaySound(deathSound);
            gameOver.SetActive(true);
            //Play death animation
            gameManager.StopTimer();
        }

        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("NoBounce")) && isGrounded)
        {
            PlaySound(landingSound);
            platformTransform = collision.transform;
            previousPlatformPosition = platformTransform.position;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("NoBounce")) && isGrounded && platformTransform != null)
        {
            Vector3 platformMovement = platformTransform.position - previousPlatformPosition;
            transform.position += platformMovement;
            previousPlatformPosition = platformTransform.position;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("NoBounce"))
        {
            platformTransform = null;
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