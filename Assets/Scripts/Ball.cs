using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [Header("Ball Properties")]
    [SerializeField] private float ball_speed = 15f;
    
    private float initial_speed;
    private float increase = 1f;
    private float max_speed = 30f;
    
    private Vector3 ball_direction;
    
    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            return;
        }

        initial_speed = ball_speed;

        LaunchBall(Random.value > 0.5f ? 1 : -1);
    }

    private void FixedUpdate()
    {
        if(!IsServer)
        {
            return;
        }

        transform.position += ball_direction * ball_speed * Time.fixedDeltaTime;
    }

    private void LaunchBall(int direction)
    {
        ball_direction = new Vector3(direction, 0f, 0f).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!IsServer)
        {
            return;
        }

        if(other.CompareTag("Player"))
        {
            float hit = (transform.position.y - other.bounds.center.y) / other.bounds.extents.y;
            hit = Mathf.Clamp(hit, -1f, 1f);
            
            float x_direction;
            
            if(transform.position.x > other.bounds.center.x)
            {
                x_direction = 1f;
            }
            else
            {
                x_direction = -1f;
            }
            
            ball_direction = new Vector3(x_direction, hit * 1.5f, 0f).normalized;

            ball_speed = Mathf.Min(ball_speed + increase, max_speed);
        }
        
        if(other.CompareTag("Wall"))
        {
            ball_direction *= -1f;
        }
    }
}