using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleScript : MonoBehaviour{
    // Variables
    public ControlType controls;
    
    public float speed = 25f;
    
    private Rigidbody rb;

    // Enum to determine the control type for the paddle, either WS keys or Arrow keys
    public enum ControlType{
        WS, Arrows
    }

    // Called before the first frame and gets the Rigidbody component of the paddle to control its movement
    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Called at a fixed time interval and checks for keyboard input to move the paddle up or down based on the assigned control type
    void FixedUpdate(){
        if (Keyboard.current == null) {
            return;
        }
        
        float movementZ = 0f;
        
        if (controls == ControlType.WS) {
            if (Keyboard.current.wKey.isPressed) {
                movementZ = movementZ + 1f;
            }
            if (Keyboard.current.sKey.isPressed) {
                movementZ = movementZ - 1f;
            }
        }else if (controls == ControlType.Arrows) {
            if (Keyboard.current.upArrowKey.isPressed) {
                movementZ = movementZ + 1f;
            }
            if (Keyboard.current.downArrowKey.isPressed) {
                movementZ = movementZ - 1f;
            }
        }

        rb.linearVelocity = new Vector3(0f, 0f, movementZ * speed); // Set the velocity of the paddle based on the input and speed
    }
}