using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // Identidy the tag for the player 

    [SerializeField] private GameObject player; // What are we looking at?
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start(){}

    // Update is called once per frame
    void Update()
    {
        GameObject playerLocation = GameObject.FindWithTag(playerTag); // Optimize? 

        // Look at stuff
        gameObject.transform.LookAt(playerLocation.transform);
    }
}
