using UnityEngine;

public class MoveMe : MonoBehaviour

{
    // Game objects for six ghost positions which the sphere will move between. 
    // (These are not the same as the cubes that appear visibly on the screen, which 
    // are like placebo-effect cubes helping to immitate bouncing physics.)
    [SerializeField] private GameObject position_1;
    [SerializeField] private GameObject position_2;
    [SerializeField] private GameObject position_3;
    [SerializeField] private GameObject position_4;
    [SerializeField] private GameObject position_5;
    [SerializeField] private GameObject position_6;
    private GameObject nextPosition;

    // Controls how fast the pinball moves.
    [SerializeField] private float moveSpeed = 5.0f; // 1 meter per second. 

    private Renderer sphereRenderer; // Pinball renderer

    // Renderers for each "bounce point" : 
    private Renderer position_1_renderer;
    private Renderer position_2_renderer;
    private Renderer position_3_renderer;
    private Renderer position_4_renderer;
    private Renderer position_5_renderer;
    private Renderer position_6_renderer;

    void Start()
    {

        sphereRenderer = gameObject.GetComponent<Renderer>();

        position_1_renderer = position_1.GetComponent<Renderer>();
        position_1.transform.position = new Vector3(-3.75f, 1.0f, 3.75f);

        position_2_renderer = position_2.GetComponent<Renderer>();
        position_2.transform.position = new Vector3(1.25f, 1.0f, 8.75f);

        position_3_renderer = position_3.GetComponent<Renderer>();
        position_3.transform.position = new Vector3(-8.75f, 1.0f, 18.75f);

        position_4_renderer = position_4.GetComponent<Renderer>();
        position_4.transform.position = new Vector3(-18.75f, 1.0f, 8.75f);

        position_5_renderer = position_5.GetComponent<Renderer>();
        position_5.transform.position = new Vector3(-13.75f, 1.0f, 3.75f); 

        position_6_renderer = position_6.GetComponent<Renderer>();
        position_6.transform.position = new Vector3(-8.75f, 1.0f, 8.75f); 

        nextPosition = position_1;
    }

    void Update()
    {
        // Move the pinball towards the nextPosition. 
        gameObject.transform.position = Vector3.MoveTowards(
            gameObject.transform.position, 
            nextPosition.transform.position, 
            moveSpeed * Time.deltaTime
        );

        // Check once if it has reched "nextPosition" to avoid long conditional check. 
        if (gameObject.transform.position == nextPosition.transform.position) {

            if (gameObject.transform.position == position_1.transform.position) {
                // Change the color to the mesh-hidden sphere we've matched the origin of. 
                sphereRenderer.material.color = position_1_renderer.material.color;
                // Set the next movement target. 
                nextPosition = position_2;

            // Same for the other 5 positions...
            } else if (gameObject.transform.position == position_2.transform.position) {
                sphereRenderer.material.color = position_2_renderer.material.color;
                nextPosition = position_3;

            } else if (gameObject.transform.position == position_3.transform.position) {
                sphereRenderer.material.color = position_3_renderer.material.color;
                nextPosition = position_4;

            } else if (gameObject.transform.position == position_4.transform.position) {
                sphereRenderer.material.color = position_4_renderer.material.color;
                nextPosition = position_5;

            } else if (gameObject.transform.position == position_5.transform.position) {
                sphereRenderer.material.color = position_5_renderer.material.color;
                nextPosition = position_6;

            } else if (gameObject.transform.position == position_6.transform.position) {
                sphereRenderer.material.color = position_6_renderer.material.color;
                nextPosition = position_1;
            }

        }
        
    }
}
