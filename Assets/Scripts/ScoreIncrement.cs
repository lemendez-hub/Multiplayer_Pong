using UnityEngine;

public class ScoreIncrement : MonoBehaviour
{
    // Variables
    public GameObject cubePrefab;
    
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
        timer += Time.deltaTime;
        
        if (timer >= seconds)
        {
            SpawnCube();
            
            timer = 0f;
        }
    }

    // Spawns a new object at a random position within the specified range and destroys it after maxSeconds
    void SpawnCube()
    {
        float randomX = Random.Range(-40.0f, 40.0f);
        float randomZ = Random.Range(-20.0f, 20.0f);
        
        Vector3 spawn = new Vector3(randomX, -0.5f, randomZ);
        
        GameObject cube = Instantiate(cubePrefab, spawn, Quaternion.identity);
        
        Destroy(cube, maxSeconds);
    }

    // Checks if the other collider has a BallScript component, and if so, it increments the score of the player based on the direction of the ball and destroys this object
    void OnTriggerEnter(Collider other)
    {
        BallScript ballScript = other.gameObject.GetComponent<BallScript>();
        
        if (ballScript != null)
        {
            if (ballScript.direction.x > 0)
            {
                ballScript.player1Score++;
                
                Destroy(gameObject);
                
                Debug.Log("Player 1 has gained a point!");
            }
            else if (ballScript.direction.x < 0)
            {
                ballScript.player2Score++;

                Destroy(gameObject);
                
                Debug.Log("Player 2 has gained a point!");
            }
        }
    }
}