using UnityEngine;
using UnityEngine.AI;

public class PaddleAI : MonoBehaviour
{
    // Variables
    public Transform ball;
    
    public float speed = 20f;

    // Called once per frame and moves the paddle towards the ball's position on the z-axis to try to intercept it
    void Update()
    {
        if (ball == null)
        {
            return;
        }
        
        float direction = 0f;
        
        if (ball.position.z > transform.position.z) // If the ball is above the paddle, set direction to 1 to move up 
        {
            direction = 1f;
        }
        else if (ball.position.z < transform.position.z) // If the ball is below the paddle, set direction to -1 to move down
        {
            direction = -1f;
        }
        
        transform.position += new Vector3(0f, 0f, direction * speed * Time.deltaTime); // Paddle moves in the direction of the ball at the specified speed
    }
}