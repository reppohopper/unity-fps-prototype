using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10; // How much damage does the projectile do?
    
    // Method to determine when to cause damage. 
    private void OnCollisionEnter(Collision collision) 
    {
        // Did we hit the target? 
        if (collision.gameObject.tag == "DamageTakingEnemy") {
            // Access the target class
            Target targetScript = collision.gameObject.GetComponent<Target>();
            // Does the script exist on the hit target?
            if (targetScript != null) 
            {
                // call taking damage method 
                targetScript.TakingDamage(damage);
            }
            // Destroy Projectile
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Player") {
            // Access the player controller class 
            PlayerController playerControllerScript = collision.gameObject.GetComponent<PlayerController>();
            if (playerControllerScript != null) {
                playerControllerScript.PlayerTakesDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
