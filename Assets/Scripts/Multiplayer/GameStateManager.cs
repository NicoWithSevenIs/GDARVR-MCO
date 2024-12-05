using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : NetworkBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [SyncVar]
    private int currentRound = 1;

    [SyncVar]
    private Phase currentPhase = Phase.DeclareActions;

    [SyncVar]
    private int activePlayerIndex = 0;

    [SyncVar]
    private bool isFirstPlayerPriority = true;

    [SyncVar(hook = nameof(OnGameStateChanged))]
    private bool gameStarted = false;

    [SyncVar(hook = nameof(OnGameOverChanged))]
    private bool isGameOver = false;

    [SyncVar(hook = nameof(OnWinnerIndexChanged))]
    private int winnerIndex = -1;  // -1 means no winner yet

    // Player 1 Stats
    [SyncVar(hook = nameof(OnPlayer1NexusHPChanged))]
    private int player1NexusHP = 20;
    [SyncVar(hook = nameof(OnPlayer1ActionBudgetChanged))]
    private int player1ActionBudget = 3;
    [SyncVar(hook = nameof(OnPlayer1ManaChanged))]
    private int player1Mana = 5;

    // Player 2 Stats
    [SyncVar(hook = nameof(OnPlayer2NexusHPChanged))]
    private int player2NexusHP = 20;
    [SyncVar(hook = nameof(OnPlayer2ActionBudgetChanged))]
    private int player2ActionBudget = 3;
    [SyncVar(hook = nameof(OnPlayer2ManaChanged))]
    private int player2Mana = 5;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Server]
    public void StartGame()
    {
        if (!isServer) return;

        // Initialize game state
        currentRound = 1;
        currentPhase = Phase.DeclareActions;
        isFirstPlayerPriority = true;
        activePlayerIndex = 0;
        gameStarted = true;
        isGameOver = false;
        winnerIndex = -1;

        // Initialize player stats
        player1NexusHP = 20;
        player1ActionBudget = 3;
        player1Mana = 1;

        player2NexusHP = 20;
        player2ActionBudget = 3;
        player2Mana = 1;

        RpcUpdateGameState();
    }

    void OnGameStateChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            Debug.Log("Game has started!");
        }
    }

    void OnGameOverChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            Debug.Log("Game has ended!");
        }
    }

    void OnWinnerIndexChanged(int oldValue, int newValue)
    {
        if (newValue >= 0)
        {
            Debug.Log($"Winner is Player {newValue + 1}!");
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdEndTurn()
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("Cannot end turn - server not active");
            return;
        }

        try
        {
            AdvanceTurn();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in CmdEndTurn: {e.Message}");
        }
    }

    [Server]
    private void AdvanceTurn()
    {
        if (!isServer) return;

        try
        {
            switch (currentPhase)
            {
                case Phase.DeclareActions:
                    currentPhase = Phase.DeclareAndSpell;
                    activePlayerIndex = isFirstPlayerPriority ? 1 : 0;
                    Debug.Log($"Advanced to DeclareAndSpell - Active Player: {activePlayerIndex}");
                    break;

                case Phase.DeclareAndSpell:
                    currentPhase = Phase.SpellOnly;
                    activePlayerIndex = isFirstPlayerPriority ? 0 : 1;
                    Debug.Log($"Advanced to SpellOnly - Active Player: {activePlayerIndex}");
                    break;

                case Phase.SpellOnly:
                    StartNewRound();
                    Debug.Log($"Started new round - Active Player: {activePlayerIndex}");
                    break;
            }

            RpcUpdateGameState();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in AdvanceTurn: {e.Message}");
        }
    }

    [Server]
    private void StartNewRound()
    {
        currentRound++;
        currentPhase = Phase.DeclareActions;
        isFirstPlayerPriority = !isFirstPlayerPriority;
        activePlayerIndex = isFirstPlayerPriority ? 0 : 1;

        // Restore resources at the start of each round
        RestorePlayerResources();
    }

    [Server]
    private void RestorePlayerResources()
    {
        // Restore Action Budget to base value
        player1ActionBudget = 3;
        player2ActionBudget = 3;

        // Restore Mana equal to round number
        player1Mana = currentRound;
        player2Mana = currentRound;
    }

    [Command(requiresAuthority = false)]
    public void CmdTryDeclareAction(int playerIndex, string actionDetails)
    {
        int actionBudget = playerIndex == 0 ? player1ActionBudget : player2ActionBudget;

        if (actionBudget > 0)
        {
            // Decrease action budget
            if (playerIndex == 0)
                CmdModifyPlayer1ActionBudget(-1);
            else
                CmdModifyPlayer2ActionBudget(-1);

            Debug.Log($"Player {playerIndex + 1} declared action: {actionDetails}");

            TargetActionResult(GetPlayerConnection(playerIndex), true, "Action declared successfully");
        }
        else
        {
            TargetActionResult(GetPlayerConnection(playerIndex), false, "Not enough Action Budget");
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdTryCastSpell(int playerIndex, string spellDetails)
    {
        int mana = playerIndex == 0 ? player1Mana : player2Mana;

        if (mana > 0)
        {
            // Decrease mana
            if (playerIndex == 0)
                CmdModifyPlayer1Mana(-1);
            else
                CmdModifyPlayer2Mana(-1);

            Debug.Log($"Player {playerIndex + 1} cast spell: {spellDetails}");

            // Return success to client
            TargetActionResult(GetPlayerConnection(playerIndex), true, "Spell cast successfully");
        }
        else
        {
            // Return failure to client
            TargetActionResult(GetPlayerConnection(playerIndex), false, "Not enough Mana");
        }
    }

    [TargetRpc]
    private void TargetActionResult(NetworkConnection target, bool success, string message)
    {
        Debug.Log($"Action Result: {message}");
    }

    private NetworkConnection GetPlayerConnection(int playerIndex)
    {
        if (CustomNetworkManager.singleton != null &&
            playerIndex < CustomNetworkManager.singleton.Players.Count)
        {
            return CustomNetworkManager.singleton.Players[playerIndex].connectionToClient;
        }
        return null;
    }

    [Server]
    private void CheckGameOver()
    {
        if (isGameOver) return;

        if (player1NexusHP <= 0)
        {
            isGameOver = true;
            winnerIndex = 1;  // Player 2 wins
            RpcGameOver(1);
        }
        else if (player2NexusHP <= 0)
        {
            isGameOver = true;
            winnerIndex = 0;  // Player 1 wins
            RpcGameOver(0);
        }
    }

    [ClientRpc]
    private void RpcGameOver(int winner)
    {
        Debug.Log($"Game Over! Player {winner + 1} wins!");
    }

    // Player 1 Command Methods
    [Command(requiresAuthority = false)]
    public void CmdModifyPlayer1NexusHP(int amount)
    {
        player1NexusHP = Mathf.Max(0, player1NexusHP + amount);
        CheckGameOver();
    }

    [Command(requiresAuthority = false)]
    public void CmdModifyPlayer1ActionBudget(int amount)
    {
        player1ActionBudget = Mathf.Max(0, player1ActionBudget + amount);
    }

    [Command(requiresAuthority = false)]
    public void CmdModifyPlayer1Mana(int amount)
    {
        player1Mana = Mathf.Max(0, player1Mana + amount);
    }

    // Player 2 Command Methods
    [Command(requiresAuthority = false)]
    public void CmdModifyPlayer2NexusHP(int amount)
    {
        player2NexusHP = Mathf.Max(0, player2NexusHP + amount);
        CheckGameOver();
    }

    [Command(requiresAuthority = false)]
    public void CmdModifyPlayer2ActionBudget(int amount)
    {
        player2ActionBudget = Mathf.Max(0, player2ActionBudget + amount);
    }

    [Command(requiresAuthority = false)]
    public void CmdModifyPlayer2Mana(int amount)
    {
        player2Mana = Mathf.Max(0, player2Mana + amount);
    }

    // Hook Methods
    void OnPlayer1NexusHPChanged(int oldValue, int newValue)
    {
        Debug.Log($"Player 1 Nexus HP changed from {oldValue} to {newValue}");
    }

    void OnPlayer1ActionBudgetChanged(int oldValue, int newValue)
    {
        Debug.Log($"Player 1 Action Budget changed from {oldValue} to {newValue}");
    }

    void OnPlayer1ManaChanged(int oldValue, int newValue)
    {
        Debug.Log($"Player 1 Mana changed from {oldValue} to {newValue}");
    }

    void OnPlayer2NexusHPChanged(int oldValue, int newValue)
    {
        Debug.Log($"Player 2 Nexus HP changed from {oldValue} to {newValue}");
    }

    void OnPlayer2ActionBudgetChanged(int oldValue, int newValue)
    {
        Debug.Log($"Player 2 Action Budget changed from {oldValue} to {newValue}");
    }

    void OnPlayer2ManaChanged(int oldValue, int newValue)
    {
        Debug.Log($"Player 2 Mana changed from {oldValue} to {newValue}");
    }

    [ClientRpc]
    private void RpcUpdateGameState()
    {
        Debug.Log($"Game State Updated - Round: {currentRound}, Phase: {currentPhase}, Active Player: {activePlayerIndex + 1}, First Priority: {isFirstPlayerPriority}");
    }

    // Getter Methods
    public int GetCurrentRound() => currentRound;
    public Phase GetCurrentPhase() => currentPhase;
    public int GetActivePlayerIndex() => activePlayerIndex;
    public bool GetIsFirstPlayerPriority() => isFirstPlayerPriority;
    public bool IsGameOver() => isGameOver;
    public int GetWinnerIndex() => winnerIndex;
    public int GetPlayer1NexusHP() => player1NexusHP;
    public int GetPlayer1ActionBudget() => player1ActionBudget;
    public int GetPlayer1Mana() => player1Mana;
    public int GetPlayer2NexusHP() => player2NexusHP;
    public int GetPlayer2ActionBudget() => player2ActionBudget;
    public int GetPlayer2Mana() => player2Mana;
}