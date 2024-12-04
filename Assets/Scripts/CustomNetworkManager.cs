using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    public static new CustomNetworkManager singleton { get; private set; }

    public List<PlayerController> Players = new List<PlayerController>();

    [SerializeField]
    private GameStateManager gameStateManagerPrefab;

    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // First, spawn the player object
        GameObject playerObj = Instantiate(playerPrefab);

        // Get the PlayerController component
        PlayerController playerController = playerObj.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // Add to players list before spawning
            Players.Add(playerController);

            // Spawn the player on the network
            NetworkServer.AddPlayerForConnection(conn, playerObj);

            // Set the player index after spawning
            playerController.NetworkPlayerIndex = Players.Count - 1;

            Debug.Log($"Player {Players.Count} added to the game");

            // If we have 2 players, start the game
            if (Players.Count == 2)
            {
                StartGame();
            }
        }
        else
        {
            Debug.LogError("PlayerController component not found on player prefab!");
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // Find and remove the disconnected player
        PlayerController player = conn.identity.GetComponent<PlayerController>();
        if (player != null)
        {
            Players.Remove(player);
            Debug.Log($"Player disconnected. Remaining players: {Players.Count}");
        }

        base.OnServerDisconnect(conn);
    }

    private void StartGame()
    {
        if (!NetworkServer.active) return;

        if (GameStateManager.Instance == null)
        {
            GameObject gameStateManagerObj = Instantiate(gameStateManagerPrefab.gameObject);
            NetworkServer.Spawn(gameStateManagerObj);
            GameStateManager.Instance.StartGame();
            Debug.Log("Game started with 2 players");
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Players.Clear();
        Debug.Log("Server stopped");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client started");
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        Debug.Log("Client stopped");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client connected to server");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Client disconnected from server");
    }
}