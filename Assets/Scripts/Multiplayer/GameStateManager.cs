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

        currentRound = 1;
        currentPhase = Phase.DeclareActions;
        isFirstPlayerPriority = true;
        activePlayerIndex = 0;
        gameStarted = true;

        RpcUpdateGameState();
    }

    void OnGameStateChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            Debug.Log("Game has started!");
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
    }

    [ClientRpc]
    private void RpcUpdateGameState()
    {
        Debug.Log($"Game State Updated - Round: {currentRound}, Phase: {currentPhase}, Active Player: {activePlayerIndex + 1}, First Priority: {isFirstPlayerPriority}");
    }

    public int GetCurrentRound() => currentRound;
    public Phase GetCurrentPhase() => currentPhase;
    public int GetActivePlayerIndex() => activePlayerIndex;
    public bool GetIsFirstPlayerPriority() => isFirstPlayerPriority;
}