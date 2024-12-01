using UnityEngine;
using Mirror;
using UnityEditor;

public class DebugUIController : MonoBehaviour
{
    /*private NetworkManager networkManager;
    private Vector2 scrollPosition;
    private string logMessages = "";
    private bool showGUI = true;
    private Rect windowRect = new Rect(10, 10, 300, 400);
    private float updateInterval = 1.0f;
    private float lastUpdateTime = 0f;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        AddLog("Debug started");
    }

    void Update()
    {
        // Update stats periodically instead of using InvokeRepeating
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateStats();
            lastUpdateTime = Time.time;
        }
    }

    void UpdateStats()
    {
        if (networkManager == null) return;

        if (NetworkClient.active)
        {
            AddLog($"Ping: {NetworkTime.rtt * 1000:0}ms");
        }
    }

    void AddLog(string message)
    {
        logMessages = $"[{Time.time:F1}] {message}\n" + logMessages;
        Debug.Log(message);
    }

    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showGUI = !showGUI;
        }

        if (!showGUI) return;

        windowRect = GUILayout.Window(0, windowRect, DrawDebugWindow, "Network Debug");
    }

    void DrawDebugWindow(int windowID)
    {
        GUILayout.BeginVertical();

        // Status display
        string status = "Disconnected";
        if (NetworkServer.active && NetworkClient.active)
            status = "Host";
        else if (NetworkServer.active)
            status = "Server";
        else if (NetworkClient.active)
            status = "Client";

        GUI.backgroundColor = Color.gray;
        GUILayout.Label($"Status: {status}");
        GUILayout.Label($"Client Address: {networkManager.networkAddress}");
        GUILayout.Label($"Transport: {Transport.active}");
        GUILayout.Label($"Connected Players: {NetworkServer.connections.Count}");

        if (NetworkClient.active)
        {
            GUILayout.Label($"Ping: {NetworkTime.rtt * 1000:0}ms");
        }

        // Connection controls
        GUI.backgroundColor = Color.green;
        if (!NetworkClient.active && !NetworkServer.active)
        {
            if (GUILayout.Button("Start Host"))
            {
                networkManager.StartHost();
                AddLog("Started Host");
            }
            if (GUILayout.Button("Start Client"))
            {
                networkManager.StartClient();
                AddLog("Started Client");
            }
        }
        else
        {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Disconnect"))
            {
                if (NetworkServer.active && NetworkClient.active)
                {
                    networkManager.StopHost();
                    AddLog("Stopped Host");
                }
                else if (NetworkServer.active)
                {
                    networkManager.StopServer();
                    AddLog("Stopped Server");
                }
                else if (NetworkClient.active)
                {
                    networkManager.StopClient();
                    AddLog("Stopped Client");
                }
            }
        }

        // Log display
        GUI.backgroundColor = Color.white;
        GUILayout.Label("Log Messages:", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(250));
        GUILayout.TextArea(logMessages);
        GUILayout.EndScrollView();

        GUILayout.EndVertical();

        GUI.DragWindow();
    }*/
}