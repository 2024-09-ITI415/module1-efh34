using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For handling UI

public class PlayerMovement : MonoBehaviour
{
    private bool jump = false;
    private Rigidbody rb;
    private Transform camHolder;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpForce;

    private Vector3 vec;

    [SerializeField] private GameObject obstacle;
    [SerializeField] private float obstacleDistance;
    [SerializeField] private float obstaclePosY;
    [SerializeField] private int numberOfObstacles;

    [SerializeField] private Text gameOverText; // For the "Game Over" message
    [SerializeField] private Text obstaclesPassedText; // For tracking obstacles passed

    private int obstaclesPassed = 0;

    private void BuildObstacles() // Set obstacles in the scene
    {
        vec.z = 56.4f;
        for (int i = 0; i < numberOfObstacles; i++)
        {
            vec.z += obstacleDistance;
            vec.y = Random.Range(-obstaclePosY, obstaclePosY);
            vec.x = 0f;

            Instantiate(obstacle, vec, Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camHolder = Camera.main.transform.parent;

        BuildObstacles();

        // Initially hide the "Game Over" text and set obstacles passed to 0
        gameOverText.gameObject.SetActive(false);
        UpdateObstaclesPassedText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump = true;
        }

        // Check if all obstacles have been passed
        if (obstaclesPassed >= numberOfObstacles)
        {
            GameOver();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * playerSpeed);
        if (jump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }

    private void LateUpdate()
    {
        vec.x = camHolder.transform.position.x;
        vec.y = camHolder.transform.position.y;
        vec.z = transform.position.z;
        camHolder.transform.position = vec;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            obstaclesPassed++;
            UpdateObstaclesPassedText(); // Update the obstacles passed UI text
            Debug.Log("Obstacle Passed: " + obstaclesPassed); // Debug log for scoring
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit an obstacle!"); // Debug log for collision
            GameOver(); // Call GameOver when colliding with an obstacle
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!"); // Debug log for game over state
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        gameOverText.gameObject.SetActive(true); // Show the Game Over text
        Invoke("RestartGame", 2f); // Restart after 2 seconds
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateObstaclesPassedText()
    {
        obstaclesPassedText.text = "Obstacles Passed: " + obstaclesPassed; // Update the text with the current count
        Debug.Log("Score Updated: " + obstaclesPassed); // Debug log for score update
    }
}
