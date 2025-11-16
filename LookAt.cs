using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform enemyLocation; // Tower/Enemy Location
    
    [SerializeField] private string playerTag = "Player"; // Identidy the tag for the player 

    [SerializeField] private float transitionSpeed = 3f; // Transition Speed
    [SerializeField] private float enemyLookDistance = 35f; // Look Distance Threshold
    [SerializeField] private float enemyShootDistance = 25f; // "Wait till you see the whites of their eyes"
    [SerializeField] private float projectileSpeed = 25f;         // Projectile Speed
    [SerializeField] private GameObject spawnPoint;         // Specify the spawn point
    [SerializeField] private GameObject enemyProjectile;   // Specify the projectile. 


    private float timeToNextShot;  // Shot timer cooldown. 
    private float fireRate = 8f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerLocation = GameObject.FindWithTag(playerTag); // Optimize? 
        // Check the distance to the player (from tower / enemy)   
        float distanceToPlayer = Vector3.Distance(playerLocation.transform.position, enemyLocation.position);

        // Look at the player if close enough
        if (distanceToPlayer <= enemyLookDistance) 
        {
            // Call the look at player method. 
            LookAtPlayer();
                    // Look at the player if close enough

            if (
                Time.time >= timeToNextShot 
                && distanceToPlayer <= enemyShootDistance
            ) {
                // Call the ShootingStuff method. 
                ShootingStuff();
            }
        }
        else 
        {
            // Reset the enemy Rotation
            ResetRotation();
        }

        // Reset rotation if not
    }

    // Create a method for looking at the player 
    void LookAtPlayer() 
    {
        GameObject playerLocation = GameObject.FindWithTag(playerTag); // Optimize? 
        // Steps to look at player
        // Determin the direction to the player 
        Vector3 directionToPlayer = playerLocation.transform.position - enemyLocation.position;
        // Calculate the rotation to the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        // Smoothly rotate the turret towards the player. 
        enemyLocation.rotation = Quaternion.Slerp(
            enemyLocation.rotation,             // Start position
            lookRotation,                       // Goal position
            transitionSpeed * Time.deltaTime    // Time it should take to move there
        );
    }

    // Reset rotation
    void ResetRotation() 
    {
        // Steps to reset enemy rotation
        // Identify what the starting rotation is. 
        Quaternion lookRotation = Quaternion.identity;
        // Smoothly rotate turret to default location. 
        enemyLocation.rotation = Quaternion.Slerp(
            enemyLocation.rotation,             // Start position
            lookRotation,                       // Goal position
            transitionSpeed * Time.deltaTime    // Time it should take to move there
        );
    }

    // Create a method to shoot stuff
    void ShootingStuff() 
    {
        // Reset the shot cooldown timer
        timeToNextShot = Time.time + 1 / fireRate;

        // Instatiate a Projectile
        GameObject projectile = Instantiate(
            enemyProjectile, 
            spawnPoint.transform.position, // At this point
            spawnPoint.transform.rotation // In this direction
        );

        // Get the Rigidbody from the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Apply the velocity to the projectily Rigidbody
        rb.linearVelocity = projectile.transform.forward * projectileSpeed;

        // Destroy itself after x seconds
        Destroy(projectile, 1.5f);
    }
}
