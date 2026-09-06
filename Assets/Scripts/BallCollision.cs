using UnityEngine;
using TMPro;

public class BallCollision : MonoBehaviour{
    // Variables
    public AudioSource hitSound;

    public TMP_Text start;
    public TMP_Text swappingScores;
    public TMP_Text randomSpeed;
    public TMP_Text ballReflected;
    public TMP_Text point;
    public TMP_Text player1PT;
    public TMP_Text player2PT;
    public TMP_Text gameOver;
    public TMP_Text retry;
    
    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("Start");
        
        start.gameObject.SetActive(true);
        start.canvasRenderer.SetAlpha(1f); // Make it fully visible
        start.CrossFadeAlpha(0f, 1.5f, false); // Fade out over 1.5 seconds
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("IncrementPoint"))
        {
            AudioManager.instance.Play("Point");
     
            point.gameObject.SetActive(true);
            point.canvasRenderer.SetAlpha(1f);
            point.CrossFadeAlpha(0f, 1.5f, false);
        }
        if (collider.CompareTag("SwapScores"))
        {
            if(BallScript.FindFirstObjectByType<BallScript>().player1Score == BallScript.FindFirstObjectByType<BallScript>().player2Score)
            {
                Debug.Log("Scores are the same, no need to swap!");
                return;
            }
            else
            {
                AudioManager.instance.Play("SwappingScores");
            
                swappingScores.gameObject.SetActive(true);
                swappingScores.canvasRenderer.SetAlpha(1f);
                swappingScores.CrossFadeAlpha(0f, 2f, false);
            }
        }
        if (collider.CompareTag("RandomSpeed"))
        {
            AudioManager.instance.Play("RandomSpeed");
            
            randomSpeed.gameObject.SetActive(true);
            randomSpeed.canvasRenderer.SetAlpha(1f);
            randomSpeed.CrossFadeAlpha(0f, 2f, false);
        }
        if (collider.CompareTag("ReflectBall"))
        {
            AudioManager.instance.Play("BallReflection");
            
            ballReflected.gameObject.SetActive(true);
            ballReflected.canvasRenderer.SetAlpha(1f);
            ballReflected.CrossFadeAlpha(0f, 2f, false);
        }
    }

    // Handle collisions with paddles and walls
    void OnCollisionEnter(Collision collision){
        if(collision.collider.name == "PaddleL" || collision.collider.name == "PaddleR"){
            hitSound = AudioManager.instance.GetAudioSource("Ball_Paddle");
            if (hitSound != null)
            {
                hitSound.pitch += 0.2f;

                if (hitSound.pitch > 5f)
                {
                    hitSound.pitch = 5f;
                }

                hitSound.Play();
            }
            
            BallScript.FindFirstObjectByType<BallScript>().speed += 1f;
            AudioManager.instance.Play("Ball_Paddle");
        }
        if(collision.collider.name == "North" || collision.collider.name == "South"){
            BallScript.FindFirstObjectByType<BallScript>().speed += 1f;
        }
        if (collision.collider.name == "West"){
            BallScript.FindFirstObjectByType<BallScript>().player2Score++;
            
            Debug.Log("Player 2 has scored!");
            
            if(BallScript.FindFirstObjectByType<BallScript>().player2Score <= 10)
            {
            AudioManager.instance.Play("PT_Player2");
            }
            
            player2PT.gameObject.SetActive(true);
            player2PT.canvasRenderer.SetAlpha(1f);
            player2PT.CrossFadeAlpha(0f, 1.5f, false);
            
            BallScript.FindFirstObjectByType<BallScript>().ResetBall(-1);
        }
        if (collision.collider.name == "East"){
            BallScript.FindFirstObjectByType<BallScript>().player1Score++;
            
            Debug.Log("Player 1 has scored!");
            
            if (BallScript.FindFirstObjectByType<BallScript>().player1Score <= 10)
            {
                AudioManager.instance.Play("PT_Player1");
            }
            
            player1PT.gameObject.SetActive(true);
            player1PT.canvasRenderer.SetAlpha(1f);
            player1PT.CrossFadeAlpha(0f, 1.5f, false);

            BallScript.FindFirstObjectByType<BallScript>().ResetBall(1);
        }
        if(BallScript.FindFirstObjectByType<BallScript>().player1Score >= 11){
            Debug.Log("Player 1 has won!");
            
            AudioManager.instance.Play("GO_Player1Wins");
            
            gameOver.gameObject.SetActive(true);
            
            DisplayGameOver();
        }
        if (BallScript.FindFirstObjectByType<BallScript>().player2Score >= 11){
            Debug.Log("Player 2 has won!");
            
            AudioManager.instance.Play("GO_Player2Wins");
            
            gameOver.gameObject.SetActive(true);

            DisplayGameOver();
        }
    }

    // Display the game over message and show the retry button
    void DisplayGameOver()
    {
        BallScript.FindFirstObjectByType<BallScript>().speed = 0f;

        isGameOver = true;

        retry.gameObject.SetActive(true);
    }
}