using UnityEngine;

public class AddHealth : MonoBehaviour
{
    [SerializeField] private int health = 10;  // How much Health to Add

    // Trigger when to add health
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.PlayerAddHealth(health);
            }

            Destroy(gameObject);
        }
    }
}
