using UnityEngine;
using Mirror;

public class DevGameUI : MonoBehaviour
{
    private PlayerController localPlayer;
    private GameStateManager gameStateManager;

    private float rightMargin = 10f;
    private float topMargin = 10f;
    private float uiWidth = 200f;
    private float uiHeight = 300f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetLocalPlayer(PlayerController player)
    {
        localPlayer = player;
        Debug.Log("Local player set in DevGameUI");
        StartCoroutine(WaitForGameStateManager());
    }

    private System.Collections.IEnumerator WaitForGameStateManager()
    {
        float timeout = 0f;
        while (GameStateManager.Instance == null && timeout < 10f)
        {
            timeout += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }

        if (GameStateManager.Instance != null)
        {
            gameStateManager = GameStateManager.Instance;
            Debug.Log("GameStateManager found in DevGameUI");
        }
        else
        {
            Debug.LogError("Failed to find GameStateManager after 10 seconds in DevGameUI");
        }
    }

    void OnGUI()
    {
        // Debug info always visible in top left
        GUI.Label(new Rect(10, 10, 300, 20), $"Connection Status: {(NetworkClient.isConnected ? "Connected" : "Not Connected")}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Local Player: {(localPlayer != null ? "Found" : "Not Found")}");
        GUI.Label(new Rect(10, 50, 300, 20), $"GameStateManager: {(gameStateManager != null ? "Found" : "Not Found")}");

        // Calculate position for top right corner
        float xPos = Screen.width - uiWidth - rightMargin;

        // Create the UI area in the top right
        GUILayout.BeginArea(new Rect(xPos, topMargin, uiWidth, uiHeight));

        if (localPlayer != null && gameStateManager != null)
        {
            try
            {
                GUILayout.Label($"Round: {gameStateManager.GetCurrentRound()}");
                GUILayout.Label($"Phase: {gameStateManager.GetCurrentPhase()}");
                GUILayout.Label($"Your Player: {localPlayer.NetworkPlayerIndex + 1}");

                // Display current turn information
                int activePlayer = gameStateManager.GetActivePlayerIndex() + 1;
                bool isYourTurn = localPlayer.NetworkPlayerIndex == gameStateManager.GetActivePlayerIndex();

                GUI.color = isYourTurn ? Color.green : Color.white;
                GUILayout.Label($"Current Turn: Player {activePlayer}");
                GUILayout.Label(isYourTurn ? "IT'S YOUR TURN!" : "Waiting for other player...");
                GUI.color = Color.white;

                // Show action buttons only if it's your turn
                if (isYourTurn)
                {
                    GUILayout.Space(10);  // Add some spacing

                    // Show appropriate buttons based on the current phase
                    Phase currentPhase = gameStateManager.GetCurrentPhase();

                    if (currentPhase == Phase.DeclareActions ||
                        (currentPhase == Phase.DeclareAndSpell && localPlayer.NetworkPlayerIndex == 1))
                    {
                        if (GUILayout.Button("Declare Action"))
                        {
                            localPlayer.CmdDeclareAction("Sample Action");
                        }
                    }

                    if (currentPhase == Phase.DeclareAndSpell || currentPhase == Phase.SpellOnly)
                    {
                        if (GUILayout.Button("Cast Spell"))
                        {
                            localPlayer.CmdCastSpell("Sample Spell");
                        }
                    }

                    GUILayout.Space(5);  // Add small spacing before End Turn button

                    if (NetworkClient.isConnected)
                    {
                        // Make End Turn button more noticeable
                        GUI.color = Color.yellow;
                        if (GUILayout.Button("End Turn"))
                        {
                            gameStateManager.CmdEndTurn();
                        }
                        GUI.color = Color.white;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error in DevGameUI: {e.Message}");
            }
        }
        else
        {
            if (!NetworkClient.isConnected)
            {
                GUILayout.Label("Not connected to server");
            }
            else
            {
                GUILayout.Label("Waiting for game to initialize...");
            }
        }

        GUILayout.EndArea();
    }

    void OnDestroy()
    {
        Debug.Log("DevGameUI was destroyed");
    }
}