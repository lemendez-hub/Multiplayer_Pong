using UnityEngine;

public class SpeedRandomizer : MonoBehaviour
{
    // Variables
    public GameObject ballPrefab;
    
    public float maxSpeed = 40f;
    public float minSpeed = 1f;
    public float seconds = 5f;
    public float maxSeconds = 5f;
    
    private float timer;

    // Called before the first frame and calls SpawnCube() to spawn the object at the start of the game
    void Start()
    {
        SpawnCube();
    }

    // Called once per frame and checks if the timer has reached the specified seconds to spawn a new object and reset the timer
    void Update()
    {
        timer += Time.deltaTime; // Increments the timer by the time elapsed since the last frame

        if (timer >= seconds)
        {
            SpawnCube();
            
            timer = 0f; // Resets the timer to 0 after spawning a new object
        }
    }

    // Spawns a new object at a random position within the specified range and destroys it after maxSeconds
    void SpawnCube()
    {
        float randomX = Random.Range(-40.0f, 40.0f); // Generates a random X position between -40 and 40 at the x-axis
        float randomZ = Random.Range(-20.0f, 20.0f); // Generates a random Z position between -20 and 20 at the z-axis

        Vector3 spawn = new Vector3(randomX, -0.5f, randomZ); // Spawns the object at the generated random position with a y-axis of -0.5f

        GameObject ball = Instantiate(ballPrefab, spawn, Quaternion.identity); // Instantiates the ballPrefab at the generated random position with no rotation

        Destroy(ball, maxSeconds); // Destroys the instantiated ball after maxSeconds to prevent clutter in the scene
    }

    // Checks if the other collider has a BallScript component, and if so, it randomizes the ball's speed and destroys this object
    void OnTriggerEnter(Collider other)
    {
        BallScript ballScript = other.gameObject.GetComponent<BallScript>(); // Checks if the other collider has a BallScript component

        if (ballScript != null)
        {
            float randomSpeed = Random.Range(minSpeed, maxSpeed); // Generates a random speed for the ball between minSpeed and maxSpeed

            ballScript.speed = randomSpeed; // Sets the ball's speed to the generated random speed

            Destroy(gameObject); // Destroys this object after randomizing the ball's speed to prevent multiple collisions and speed changes

            Debug.Log("Ball speed has been randomized to: " + randomSpeed);
        }
    }
}