using UnityEngine;

public class AddAmmo : MonoBehaviour
{
    [SerializeField] private int ammo = 10;  // How much ammo to add

    // Trigger when to add ammo
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.PlayerAddAmmo(ammo);
            }

            Destroy(gameObject);
        }
    }
}
