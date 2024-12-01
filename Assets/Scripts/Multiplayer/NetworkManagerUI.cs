using UnityEngine;
using Mirror;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private NetworkManager netManager;

    void Start()
    {
        // Ensure we have reference to the NetworkManager
        if (netManager == null)
            netManager = FindObjectOfType<NetworkManager>();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            // Show buttons only if we're not connected/hosting
            if (GUILayout.Button("Host"))
            {
                netManager.StartHost();
                Debug.Log("Started Host");
            }

            if (GUILayout.Button("Connect"))
            {
                netManager.StartClient();
                Debug.Log("Started Client");
            }
        }
        else
        {
            // Show connection status and disconnect button
            GUILayout.Label($"Transport: {Transport.active}");
            GUILayout.Label($"Mode: {(NetworkServer.active ? "Host" : "Client")}");
            GUILayout.Label($"Players Connected: {NetworkServer.connections.Count}");

            if (GUILayout.Button("Disconnect"))
            {
                if (NetworkServer.active && NetworkClient.isConnected)
                    netManager.StopHost();
                else if (NetworkClient.isConnected)
                    netManager.StopClient();
            }
        }

        GUILayout.EndArea();
    }
}