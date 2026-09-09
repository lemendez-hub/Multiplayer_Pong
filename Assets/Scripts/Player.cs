using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private InputSystemActions input_system_actions;
    
    [Header("Player Properties")]
    [SerializeField] private float player_speed = 10f;
    
    private Rigidbody player_rb;
    
    private Vector2 player_input;
    
    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            return;
        }
        
        input_system_actions.Enable();
        
        input_system_actions.Player.Move.performed += OnMove;
        input_system_actions.Player.Move.canceled += OnMove;
    }
    
    public override void OnNetworkDespawn()
    {
        if(!IsOwner)
        {
            return;
        }
        
        input_system_actions.Player.Move.performed -= OnMove;
        
        input_system_actions.Player.Move.canceled -= OnMove;
        input_system_actions.Disable();
    }
    
    private void Awake()
    {
        input_system_actions = new InputSystemActions();
        
        player_rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if(!IsOwner)
        {
            return;
        }
        
        if(!MultiplayerGameManager.instance.MatchStarted)
        {
            player_input = Vector2.zero;
            
            return;
        }

        if(MultiplayerGameManager.instance.MatchEnd)
        {
            return;
        }
        
        MoveRPC(player_input);
    }
    
    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 input)
    {
        if(!MultiplayerGameManager.instance.MatchStarted)
        {
            return;
        }

        if(MultiplayerGameManager.instance.MatchEnd)
        {
            return;
        }
        
        Vector3 movement = new Vector3(0f, input.y, 0f) * player_speed * Time.fixedDeltaTime;
        
        player_rb.MovePosition(player_rb.position + movement);
    }
    
    private void OnMove(InputAction.CallbackContext ctx)
    {
        player_input = ctx.ReadValue<Vector2>();
    }
}