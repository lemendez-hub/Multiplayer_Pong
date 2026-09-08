using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerGameManager : MonoBehaviour
{
    [Header("Spawn Buttons")]
    [SerializeField] private Button host_button;
    [SerializeField] private Button client_button;

    [Header("Spawn Positions")]
    [SerializeField] private Transform host_spawn;
    [SerializeField] private Transform client_spawn;

    private void Awake()
    {
        host_button.onClick.AddListener(StartHost);
        client_button.onClick.AddListener(StartClient);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void Update()
    {
        if(NetworkManager.Singleton.IsListening)
        {
            host_button.gameObject.SetActive(false);
            client_button.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
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
    }
}