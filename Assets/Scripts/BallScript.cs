using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BallScript : MonoBehaviour{
    // Variables
    public float speed = 10f;
    
    public Vector3 direction;
    
    public int player1Score;
    public int player2Score;
    
    public Text player1Text;
    public Text player2Text;

    // Called before the first frame and initializes the scores and direction of the ball at the start of the game
    void Start()
    {
        player1Score = 0;
        player2Score = 0;
        
        direction = new Vector3(1f, 0f, 1f);
    }

    // Called once per frame and updates the position of the ball based on its direction and speed, updates the score text for both players, changes the color of the score text based on the score, and checks for game over condition to reset the game if space key is pressed
    void Update()
    {
        transform.position = transform.position + direction * speed * Time.deltaTime; // Move the ball based on its direction and speed

        player1Text.text = player1Score.ToString();
        player1Text.color = Color.red;
        
        player2Text.text = player2Score.ToString();
        player2Text.color = Color.red;
        
        if (player1Score >= 4 && player1Score < 8)
        {
            player1Text.color = Color.yellow;
        }
        if (player1Score >= 8)
        {
            player1Text.color = Color.green;
        }
        if (player2Score >= 4 && player2Score < 8)
        {
            player2Text.color = Color.yellow;
        }
        if (player2Score >= 8)
        {
            player2Text.color = Color.green;
        }
        if (transform.position.x > 55 && transform.position.x < -55)
        {
            ResetBall(1);
        }
        if (transform.position.y > 5)
        {
            ResetBall(1);
        }
        if (transform.position.z > 30 && transform.position.z < -30)
        {
            ResetBall(1);
        }
        if (BallCollision.FindFirstObjectByType<BallCollision>().isGameOver)
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                BallCollision.FindFirstObjectByType<BallCollision>().gameOver.gameObject.SetActive(false);
                BallCollision.FindFirstObjectByType<BallCollision>().isGameOver = false;
                BallCollision.FindFirstObjectByType<BallCollision>().retry.gameObject.SetActive(false);
                
                ResetGame();
            }
            return;
        }
    }

    // Called when the ball collides with another object and reflects the ball's direction based on the normal of the collision
    void OnCollisionEnter(Collision collision)
    {
        Vector3 normalHit = collision.contacts[0].normal; // Get the normal of the collision at the point of contact

        direction = Vector3.Reflect(direction, normalHit); // Reflect the ball's direction based on the normal of the collision to create a bouncing effect
    }

    // Resets the position, speed, and direction of the ball to the initial state for a new round, and resets the pitch of the hit sound if it exists
    public void ResetBall(int dir)
    {
        transform.position = new Vector3(0f, 1.8f, 0f);
        speed = 10f;
        
        if (BallCollision.FindFirstObjectByType<BallCollision>().hitSound != null)
        {
            BallCollision.FindFirstObjectByType<BallCollision>().hitSound.pitch = 1f;
        }
        
        float random = Random.Range(-1f, 1f);
        
        direction = new Vector3(dir, 0f, random);
    }

    // Resets the scores of both players to 0 and calls ResetBall() to reset the ball for a new game
    public void ResetGame()
    {
        player1Score = 0;
        player2Score = 0;
        
        ResetBall(1);
    }
}