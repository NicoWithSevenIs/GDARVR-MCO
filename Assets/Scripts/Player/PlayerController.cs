using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPlayerIndexChanged))]
    private int networkPlayerIndex = -1;

    private GameStateManager gameStateManager;
    private DevGameUI gameUI;

    public int NetworkPlayerIndex
    {
        get => networkPlayerIndex;
        [Server]
        set
        {
            if (!isServer) return;
            networkPlayerIndex = value;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            Debug.Log($"Local player {networkPlayerIndex} initialized");
            gameUI = FindFirstObjectByType<DevGameUI>();
            if (gameUI != null)
            {
                gameUI.SetLocalPlayer(this);
                Debug.Log("DevGameUI found and local player set");
            }
            else
            {
                Debug.LogError("DevGameUI not found in scene!");
            }
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeGameStateManager();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        InitializeGameStateManager();
    }

    private void InitializeGameStateManager()
    {
        if (gameStateManager == null)
        {
            StartCoroutine(WaitForGameStateManager());
        }
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
            Debug.Log($"Player {networkPlayerIndex}: GameStateManager found");
        }
        else
        {
            Debug.LogError($"Player {networkPlayerIndex}: Failed to find GameStateManager after 10 seconds");
        }
    }

    void OnPlayerIndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"Player index changed from {oldIndex} to {newIndex}");
    }

    [Server]
    private bool IsMyTurn()
    {
        if (!isServer) return false;
        if (gameStateManager == null)
        {
            gameStateManager = GameStateManager.Instance;
            if (gameStateManager == null) return false;
        }

        int activePlayer = gameStateManager.GetActivePlayerIndex();
        Phase currentPhase = gameStateManager.GetCurrentPhase();
        bool isFirstPlayerPriority = gameStateManager.GetIsFirstPlayerPriority();

        Debug.Log($"Turn Check - Player: {networkPlayerIndex}, Active: {activePlayer}, Phase: {currentPhase}, FirstPriority: {isFirstPlayerPriority}");

        return networkPlayerIndex == activePlayer;
    }

    [Server]
    private bool CanDeclareActions()
    {
        if (!isServer) return false;
        if (gameStateManager == null)
        {
            gameStateManager = GameStateManager.Instance;
            if (gameStateManager == null) return false;
        }

        Phase currentPhase = gameStateManager.GetCurrentPhase();
        return (currentPhase == Phase.DeclareActions && networkPlayerIndex == 0) ||
               (currentPhase == Phase.DeclareAndSpell && networkPlayerIndex == 1);
    }

    [Server]
    private bool CanCastSpells()
    {
        if (!isServer) return false;
        if (gameStateManager == null)
        {
            gameStateManager = GameStateManager.Instance;
            if (gameStateManager == null) return false;
        }

        Phase currentPhase = gameStateManager.GetCurrentPhase();
        return (currentPhase == Phase.DeclareAndSpell && networkPlayerIndex == 1) ||
               (currentPhase == Phase.SpellOnly && networkPlayerIndex == 0);
    }

    [Command]
    public void CmdDeclareAction(string action)
    {
        // Try to get GameStateManager if it's null
        if (gameStateManager == null)
        {
            gameStateManager = GameStateManager.Instance;
            if (gameStateManager == null)
            {
                Debug.LogError($"Player {networkPlayerIndex}: Cannot declare action - GameStateManager not found");
                return;
            }
        }

        if (!CanDeclareActions())
        {
            Debug.LogWarning($"Player {networkPlayerIndex}: Cannot declare actions in phase {gameStateManager.GetCurrentPhase()}");
            return;
        }

        Debug.Log($"Player {networkPlayerIndex + 1} declared action: {action}");
        // Implement your action logic here
    }

    [Command]
    public void CmdCastSpell(string spell)
    {
        // Try to get GameStateManager if it's null
        if (gameStateManager == null)
        {
            gameStateManager = GameStateManager.Instance;
            if (gameStateManager == null)
            {
                Debug.LogError($"Player {networkPlayerIndex}: Cannot cast spell - GameStateManager not found");
                return;
            }
        }

        if (!CanCastSpells())
        {
            Debug.LogWarning($"Player {networkPlayerIndex}: Cannot cast spells in phase {gameStateManager.GetCurrentPhase()}");
            return;
        }

        Debug.Log($"Player {networkPlayerIndex + 1} cast spell: {spell}");
        // Implement your spell logic here
    }
}