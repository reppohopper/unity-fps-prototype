using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Purpose is to provide forward, backward, and side to side motion. 
    // Learning note: Lives on our "Player (Capsule)" game object, so we don't actually need to 
    // identify the player as a capital G GameObject variable. 
    [SerializeField] private float playerSpeed = 16f;  // Player speed
    [SerializeField] private float rotationSensitivity = 2f; // Rotation sensitivity
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float verticalRotationSensitivity = 2f;


    [SerializeField] private GameObject playerProjectile;   // Specify the projectile. 
    [SerializeField] private GameObject spawnPoint;         // Specify the spawn point
    [SerializeField] private float projectileSpeed = 95f;         // Projectile Speed
    [SerializeField] private int ammoCount = 30;                 // Ammo Count

    [SerializeField] private int playerHealth = 100;        // Player health. 
    [SerializeField] private int playerMaxHealth = 100; 
    [SerializeField] private TextMeshProUGUI healthText; // Text blob for health
    [SerializeField] private TextMeshProUGUI ammoText; // Text blob for ammo count. 

    [SerializeField] private GameObject gameOverText; // Game over text blob. 
    [SerializeField] private GameObject restartText; 
    [SerializeField] private GameObject youWinText;  // You win text blob. 

    // Learning Note: We need to find and access the player capsule's rigid body. 
    // Contianer for the rigidbody
    private Rigidbody playerRB;
    // Identify the transform for the camera
    private float upDownrotation;   // Vertical rotation limit for camera
 

    private float timeToNextShot;  // Shot timer cooldown. 
    private float fireRate = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Print HP and Ammo count to HUD. 
        healthText.text = "HP: " + playerHealth.ToString() + " / " + playerMaxHealth.ToString();;
        ammoText.text = "Ammo: " + ammoCount.ToString();

        // Fill the RigidBody Container
        // Learning note: alternative syntax would be to omit `gameObject.` altogether. 
        playerRB = gameObject.GetComponent<Rigidbody>();

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        
        // Hide the cursor
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {

        // Calculate the player velocity based on input. 
        //      (Identify the object's location and rotation in space, and then
        //      incorporate any active input. We must avoid blipping back and forth)
        Vector3 playerVelocity = (
            gameObject.transform.rotation 
            // Vector 3 based off of our inputs
            * new Vector3(
                Input.GetAxis("Horizontal"), // X-axis
                0f,  // Y-axis
                Input.GetAxis("Vertical") // Z-axis
            ).normalized 
            * playerSpeed
        );
        
        // Preserve the effects of gravity
        playerVelocity.y = playerRB.linearVelocity.y;

        // Apply the Player Velocity we just calculated. 
        playerRB.linearVelocity = playerVelocity;

        // Apply the Player Rotation
        //      Learning note: using .Rotate is more powerful, considerers start pos. etc.  
        gameObject.transform.Rotate(
            Vector3.up, // Rotation axis (the y axis)
            Input.GetAxis("Mouse X") * rotationSensitivity // Rotation amount (uses screen space x and y)
            // ?? Rotation speed ??
        );
        // Clamped looking up and down. 
        // (+) Apply that camera rotation
        // Get the rotation for the camera
        upDownrotation = Mathf.Clamp(
            -1 * (Input.GetAxis("Mouse Y") * verticalRotationSensitivity) + upDownrotation,
            -90f,
            60f   
        );
        // Apply the rotation for the camera
        playerCamera.localRotation = Quaternion.Euler(new Vector3(upDownrotation, 0f, 0f));

        // Shooting stuff
        // Can we shoot? 
        //      Did we press the fire button?
        //      Do we have ammo? 
        //      Has the cooldown timer reset?
        if (Input.GetButtonDown("Fire1") && ammoCount > 0 && Time.time >= timeToNextShot) 
        {
            // Call the ShootingStuff method. 
            ShootingStuff();
        }

        if (Time.timeScale == 0f && Input.GetKeyDown(KeyCode.R)) {
            // Reset the game 
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    // Create a method to shoot stuff
    void ShootingStuff() 
    {
        // Reduce ammo count and ammo display count by 1. 
        ammoCount -= 1;
        ammoText.text = "Ammo: " + ammoCount.ToString();


        // Reset the shot cooldown timer
        timeToNextShot = Time.time + 1 / fireRate;

        // Instatiate a Projectile
        GameObject projectile = Instantiate(
            playerProjectile, 
            spawnPoint.transform.position, // At this point
            spawnPoint.transform.rotation // In this direction
        );

        // Get hte Rigidbody from the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Apply the velocity to the projectily Rigidbody
        rb.linearVelocity = projectile.transform.forward * projectileSpeed;

        // Destroy itself after x seconds
        Destroy(projectile, 3f);
    }

    public void PlayerTakesDamage(int damage) 
    {
        // Reduce player health
        playerHealth = Math.Max(playerHealth - damage, 0); // Let's not let it fall below zero. 
        
        // Update display health. 
        healthText.text = "HP: " + playerHealth.ToString() + " / " + playerMaxHealth.ToString();

        // Check to see if we are dead
        if (playerHealth <= 0) {
            gameOverText.SetActive(true);
            restartText.SetActive(true);
            SimulateFreezingTheGame();
        }
    }
            
    // Method to simulate Freezing the Game 
    public void SimulateFreezingTheGame() 
    {
            rotationSensitivity = 0f;
            verticalRotationSensitivity = 0f;
            ammoCount = 0; // Falsely remove all ammo to prevent final shot. 
            Time.timeScale = 0f;
    }

    public void PlayerAddHealth (int health) 
    {
        playerHealth = Math.Min(playerMaxHealth, playerHealth + health);
        // Update display health. 
        healthText.text = "HP: " + playerHealth.ToString() + " / " + playerMaxHealth.ToString();;
    }

    public void PlayerAddAmmo (int ammo) 
    {
        ammoCount += ammo;
        // Update display ammo. 
        ammoText.text = "Ammo: " + ammoCount.ToString();
    }

    public void PlayerWins ()
    {
        youWinText.SetActive(true);
        restartText.SetActive(true);
        SimulateFreezingTheGame();
    }
}
