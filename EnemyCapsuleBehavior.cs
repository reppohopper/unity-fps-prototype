using UnityEngine;
using UnityEngine.UI;

/* A class that does the following things: 
 * - Agro to (repeatedly move toward and shoot) the player only in a specified range
 * - Stop chasing and reset rotation once outside that range 
 */
public class EnemyCapsuleBehavior : MonoBehaviour
{
    [SerializeField] private Transform enemyLocation; // Tower/Enemy Location
    [SerializeField] private Transform playerLocation; // Player Location
    [SerializeField] private GameObject enemyProjectile;   // Specify the projectile. 
    [SerializeField] private GameObject spawnPoint;         // Specify the spawn point
    [SerializeField] private float transitionSpeed = 3f; // Transition Speed
    [SerializeField] private float enemyLookDistance = 15f; // Look Distance Threshold

    [SerializeField] private float enemyShootDistance = 10f; // "Wait till you see the whites of their eyes"
    [SerializeField] private float enemyMoveSpeed = 5.0f;
    [SerializeField] private float projectileSpeed = 25f;         // Projectile Speed
    [SerializeField] private int ammoCount = 30;                 // Ammo Count

    // Patrol position tags
    [SerializeField] private string positionOne = "P1_P1"; // Patrol 1 Position 1
    [SerializeField] private string positionTwo = "P1_P2";
    [SerializeField] private string positionThree = "P1_P3";

    [SerializeField] private string positionFour = "P1_P4";

    private GameObject nextPatrolTouchpoint;
    private float timeToNextShot;  // Shot timer cooldown. 
    private float fireRate = 1.5f; // Storm troopers. 

    private GameObject position_1;
    private GameObject position_2;
    private GameObject position_3;
    private GameObject position_4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position_1 = GameObject.FindWithTag(positionOne);
        position_2 = GameObject.FindWithTag(positionTwo);
        position_3 = GameObject.FindWithTag(positionThree);
        position_4 = GameObject.FindWithTag(positionFour);

        nextPatrolTouchpoint = position_1;
    }

    // Update is called once per frame
    void Update()
    {
        // Check the distance to the player (from tower / enemy)   
        float distanceToPlayer = Vector3.Distance(playerLocation.position, enemyLocation.position);

        // Look at the player if close enough
        if (distanceToPlayer <= enemyLookDistance) 
        {
            // Call the look at player method. 
            SmoothlyLookAtTransform(playerLocation);
            // Don't keep walking toward the player to shoot them at point blank
            if (distanceToPlayer > enemyShootDistance / 2) 
            {
                MoveTowardsPlayer();
            }
            
            if (
                ammoCount > 0 
                && Time.time >= timeToNextShot 
                && distanceToPlayer <= enemyShootDistance
            ) {
                // Call the ShootingStuff method. 
                ShootingStuff();
            }
        }
        // Otherwise continue with patrolling. 
        else 
        {
            // Move the enemy towards the nextPatrolTouchpoint. 
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position, 
                nextPatrolTouchpoint.transform.position, 
                enemyMoveSpeed * Time.deltaTime
            );
            
            if (gameObject.transform.position == position_1.transform.position) {
                nextPatrolTouchpoint = position_2;
            } else if (gameObject.transform.position == position_2.transform.position) {
                nextPatrolTouchpoint = position_3;
            } else if (gameObject.transform.position == position_3.transform.position) {
                nextPatrolTouchpoint = position_4;
            } else if (gameObject.transform.position == position_4.transform.position) {
                nextPatrolTouchpoint = position_1;
            }

            // Enemy also faces in the direction it is walking. 
            SmoothlyLookAtTransform(nextPatrolTouchpoint.transform);
        }

    }

    // Updated lookAtPlayer method to allow the unit to look in 
    // the direction it is walking while on patrol. 
    void SmoothlyLookAtTransform(Transform transformToLookAt) 
    {
        // Steps to look at player
        // Determine the direction to the thing 
        Vector3 directionToThing = transformToLookAt.position - enemyLocation.position;
        // Calculate the rotation to the thing
        Quaternion lookRotation = Quaternion.LookRotation(directionToThing);
        // Smoothly rotate the turret towards the player. 
        enemyLocation.rotation = Quaternion.Slerp(
            enemyLocation.rotation,             // Start position
            lookRotation,                       // Goal position
            transitionSpeed * Time.deltaTime    // Time it should take to move there
        );
    }

    void MoveTowardsPlayer() 
    {
        // Move the enemy towards the player position. 
        gameObject.transform.position = Vector3.MoveTowards(
            gameObject.transform.position, 
            playerLocation.position, 
            enemyMoveSpeed * Time.deltaTime
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
// Reduce ammo count by 1
        ammoCount -= 1;

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
