using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    private DebugUIController debugController;

    public override void Start()
    {
        base.Start();
        debugController = FindObjectOfType<DebugUIController>();

        // Verify transport exists
        if (transport == null)
        {
            // Try to get KCP Transport
            transport = GetComponent<kcp2k.KcpTransport>();

            // If still null, try Telepathy
            if (transport == null)
            {
                transport = GetComponent<TelepathyTransport>();
            }

            // If still null, add KCP Transport
            if (transport == null)
            {
                Debug.Log("No transport found. Adding KCP Transport.");
                transport = gameObject.AddComponent<kcp2k.KcpTransport>();
            }
        }
    }

    public override void OnStartHost()
    {
        if (transport == null)
        {
            Debug.LogError("Cannot start host: No transport configured!");
            return;
        }
        base.OnStartHost();
        Debug.Log("Host started");
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        Debug.Log("Host stopped");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log($"Client connected. Total players: {NetworkServer.connections.Count}");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log($"Client disconnected. Total players: {NetworkServer.connections.Count}");
    }
}