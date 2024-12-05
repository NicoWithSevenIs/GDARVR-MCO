using UnityEngine;
using Mirror;

public class DevGameUI : MonoBehaviour
{
    private PlayerController localPlayer;
    private GameStateManager gameStateManager;

    private float rightMargin = 10f;
    private float topMargin = 10f;
    private float uiWidth = 200f;
    private float uiHeight = 500f;

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
        GUI.Label(new Rect(10, 10, 300, 20), $"Connection Status: {(NetworkClient.isConnected ? "Connected" : "Not Connected")}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Local Player: {(localPlayer != null ? "Found" : "Not Found")}");
        GUI.Label(new Rect(10, 50, 300, 20), $"GameStateManager: {(gameStateManager != null ? "Found" : "Not Found")}");

        float xPos = Screen.width - uiWidth - rightMargin;

        GUILayout.BeginArea(new Rect(xPos, topMargin, uiWidth, uiHeight));

        if (localPlayer != null && gameStateManager != null)
        {
            try
            {
                // Game State Information
                GUILayout.Label("Game State", GUI.skin.box);
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

                GUILayout.Space(10);

                // Player 1 Stats
                GUILayout.Label("Player 1 Stats", GUI.skin.box);
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Nexus HP: {gameStateManager.GetPlayer1NexusHP()}");
                if (GUILayout.Button("+", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer1NexusHP(1);
                if (GUILayout.Button("-", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer1NexusHP(-1);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"Action Budget: {gameStateManager.GetPlayer1ActionBudget()}");
                if (GUILayout.Button("+", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer1ActionBudget(1);
                if (GUILayout.Button("-", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer1ActionBudget(-1);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"Mana: {gameStateManager.GetPlayer1Mana()}");
                if (GUILayout.Button("+", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer1Mana(1);
                if (GUILayout.Button("-", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer1Mana(-1);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                // Player 2 Stats
                GUILayout.Label("Player 2 Stats", GUI.skin.box);
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Nexus HP: {gameStateManager.GetPlayer2NexusHP()}");
                if (GUILayout.Button("+", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer2NexusHP(1);
                if (GUILayout.Button("-", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer2NexusHP(-1);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"Action Budget: {gameStateManager.GetPlayer2ActionBudget()}");
                if (GUILayout.Button("+", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer2ActionBudget(1);
                if (GUILayout.Button("-", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer2ActionBudget(-1);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"Mana: {gameStateManager.GetPlayer2Mana()}");
                if (GUILayout.Button("+", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer2Mana(1);
                if (GUILayout.Button("-", GUILayout.Width(30))) gameStateManager.CmdModifyPlayer2Mana(-1);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                // Game Over Display
                if (gameStateManager.IsGameOver())
                {
                    GUILayout.Space(20);
                    GUI.color = Color.yellow;
                    GUILayout.Label("GAME OVER!", GUI.skin.box);

                    int winner = gameStateManager.GetWinnerIndex();
                    if (winner == localPlayer.NetworkPlayerIndex)
                    {
                        GUI.color = Color.green;
                        GUILayout.Label("YOU WIN!", GUI.skin.box);
                    }
                    else
                    {
                        GUI.color = Color.red;
                        GUILayout.Label("YOU LOSE!", GUI.skin.box);
                    }
                    GUI.color = Color.white;
                }

                // Action Buttons (only shown if it's your turn and game isn't over)
                if (isYourTurn && !gameStateManager.IsGameOver())
                {
                    Phase currentPhase = gameStateManager.GetCurrentPhase();
                    int playerIndex = localPlayer.NetworkPlayerIndex;
                    bool canDeclareAction = playerIndex == 0 ?
                        gameStateManager.GetPlayer1ActionBudget() > 0 :
                        gameStateManager.GetPlayer2ActionBudget() > 0;
                    bool canCastSpell = playerIndex == 0 ?
                        gameStateManager.GetPlayer1Mana() > 0 :
                        gameStateManager.GetPlayer2Mana() > 0;

                    // Show Declare Action button with appropriate state
                    if (currentPhase == Phase.DeclareActions ||
                        (currentPhase == Phase.DeclareAndSpell && localPlayer.NetworkPlayerIndex == 1))
                    {
                        GUI.enabled = canDeclareAction;
                        if (GUILayout.Button("Declare Action"))
                        {
                            gameStateManager.CmdTryDeclareAction(playerIndex, "Sample Action");
                        }
                        GUI.enabled = true;
                    }

                    // Show Cast Spell button with appropriate state
                    if (currentPhase == Phase.DeclareAndSpell || currentPhase == Phase.SpellOnly)
                    {
                        GUI.enabled = canCastSpell;
                        if (GUILayout.Button("Cast Spell"))
                        {
                            gameStateManager.CmdTryCastSpell(playerIndex, "Sample Spell");
                        }
                        GUI.enabled = true;
                    }

                    GUILayout.Space(5);

                    // End Turn button
                    if (NetworkClient.isConnected)
                    {
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