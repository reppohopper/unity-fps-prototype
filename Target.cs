using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField] private int targetHealth = 50;
    [SerializeField] private Text healhText; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject dropItem;                 

    void Start()
    {
        healhText.text = targetHealth.ToString();
    }

    public void TakingDamage(int damage)
    {
        // Reduce Health of Target
        targetHealth -= damage;
        healhText.text = targetHealth.ToString();
        
        // If dead, destroy object. 
        if (targetHealth <= 0) 
        {
            Vector3 itemSpawnPosition = transform.position;
            Destroy(gameObject);
            if (dropItem != null) {
                Instantiate(dropItem, itemSpawnPosition, dropItem.transform.rotation);
            }
        }
    }

}
