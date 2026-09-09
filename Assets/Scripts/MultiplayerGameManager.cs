using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MultiplayerGameManager : NetworkBehaviour
{
    public static MultiplayerGameManager instance;
    
    private NetworkVariable<bool> match_started = new NetworkVariable<bool>(false);
    public bool MatchStarted => match_started.Value;

    private NetworkVariable<bool> match_end = new NetworkVariable<bool>(false);
    public bool MatchEnd => match_end.Value;
    
    [Header("Host/Client Menu")]
    [SerializeField] private GameObject host_client_menu;
    [SerializeField] private Button host_button;
    [SerializeField] private Button client_button;
    
    [Header("Spawn Positions")]
    [SerializeField] private Transform host_spawn;
    [SerializeField] private Transform client_spawn;
    
    [Header("Scores")]
    [SerializeField] private TextMeshProUGUI host_score;
    private NetworkVariable<int> hostscore = new NetworkVariable<int>(0);
    [SerializeField] private TextMeshProUGUI client_score;
    private NetworkVariable<int> clientscore = new NetworkVariable<int>(0);

    [Header("Gameover Menu")]
    [SerializeField] private GameObject gameover_menu;
    [SerializeField] private TextMeshProUGUI host_wins;
    [SerializeField] private TextMeshProUGUI client_wins;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            
            return;
        }

        host_client_menu.SetActive(true);
        host_button.onClick.AddListener(StartHost);
        client_button.onClick.AddListener(StartClient);

        gameover_menu.SetActive(false);
        host_wins.gameObject.SetActive(false);
        client_wins.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if(NetworkManager.Singleton.IsListening)
        {
            host_client_menu.SetActive(false);
        }
        
        Winner();
        
        if(!match_end.Value)
        {
            gameover_menu.SetActive(false);
            host_wins.gameObject.SetActive(false);
            client_wins.gameObject.SetActive(false);
        }
        
        if(match_end.Value && Keyboard.current.rKey.wasPressedThisFrame)
        {
            PlayAgainRPC();
        }
    }
    
    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        
        hostscore.OnValueChanged += OnHostScoreChange;
        clientscore.OnValueChanged += OnClientScoreChange;
        
        host_score.text = hostscore.Value.ToString();
        client_score.text = clientscore.Value.ToString();
    }
    
    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        
        hostscore.OnValueChanged -= OnHostScoreChange;
        clientscore.OnValueChanged -= OnClientScoreChange;
    }
    
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    
    private void OnClientConnected(ulong clientID)
    {
        if(!NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        NetworkClient client = NetworkManager.Singleton.ConnectedClients[clientID];
        
        if(client.PlayerObject == null)
        {
            return;
        }
        
        if(clientID == NetworkManager.ServerClientId)
        {
            client.PlayerObject.transform.position = host_spawn.position;
        }
        else
        {
            client.PlayerObject.transform.position = client_spawn.position;
        }
        
        if(NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            match_started.Value = true;
        }
    }
    
    private void OnClientDisconnected(ulong clientID)
    {
        if(!NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        match_started.Value = false;
    }
    
    private void OnHostScoreChange(int oldScore, int newScore)
    {
        host_score.text = newScore.ToString();
    }
    
    private void OnClientScoreChange(int oldScore, int newScore)
    {
        client_score.text = newScore.ToString();
    }
    
    public void UpdateHostScore()
    {
        if(!IsServer)
        {
            return;
        }
        
        hostscore.Value++;
    }
    
    public void UpdateClientScore()
    {
        if(!IsServer)
        {
            return;
        }
        
        clientscore.Value++;
    }
    
    private void Winner()
    {
        if(IsServer)
        {
            if(hostscore.Value == 12 || clientscore.Value == 12)
            {
                match_end.Value = true;
            }
        }
        
        if(match_end.Value)
        {
            gameover_menu.SetActive(true);
            
            host_wins.gameObject.SetActive(hostscore.Value == 12);
            client_wins.gameObject.SetActive(clientscore.Value == 12);
        }
    }

    [Rpc(SendTo.Server)]
    private void PlayAgainRPC()
    {
        if(!IsServer)
        {
            return;
        }
        
        hostscore.Value = 0;
        clientscore.Value = 0;
        
        match_end.Value = false;
        match_started.Value = true;
    }
}