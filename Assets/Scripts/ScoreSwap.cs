using UnityEngine;

public class ScoreSwap : MonoBehaviour
{
    // Public/Private
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

    // Checks if the other collider has a BallScript component, and if so, it swaps the scores of player 1 and player 2 and destroys this object
    void OnTriggerEnter(Collider other)
    {
        BallScript ballScript = other.gameObject.GetComponent<BallScript>();
        
        if (ballScript != null)
        {
            int tempScore = ballScript.player1Score;
            
            ballScript.player1Score = ballScript.player2Score;
            ballScript.player2Score = tempScore;
            
            Destroy(gameObject);
            
            Debug.Log("Scores have Swapped!");
        }
    }
}