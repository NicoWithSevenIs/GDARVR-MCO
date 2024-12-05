using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class RaycastTester : NetworkBehaviour
{
    [SerializeField] private List<GameObject> playerModels;
    [SerializeField] private List<GameObject> enemyModels;

    private PlayerController localPlayerController;

    private void Start()
    {
        foreach (var model in playerModels)
            BoardStateManager.instance.MakeCharacter(1, model);
        foreach (var model in enemyModels)
            BoardStateManager.instance.MakeCharacter(2, model);

        StartCoroutine(WaitForPlayerController());
    }

    private System.Collections.IEnumerator WaitForPlayerController()
    {
        // Wait until NetworkClient is connected
        while (NetworkClient.connection == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Wait until player identity is assigned
        while (NetworkClient.connection.identity == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Get the player controller
        localPlayerController = NetworkClient.connection.identity.GetComponent<PlayerController>();
        Debug.Log($"Found local player controller. Player Index: {localPlayerController.NetworkPlayerIndex}");
    }

    private void Update()
    {
        // Return if we don't have a valid player controller
        if (localPlayerController == null)
        {
            // Try to get it again if we haven't found it yet
            if (NetworkClient.connection != null && NetworkClient.connection.identity != null)
            {
                localPlayerController = NetworkClient.connection.identity.GetComponent<PlayerController>();
                if (localPlayerController != null)
                {
                    Debug.Log($"Found player controller in Update. Index: {localPlayerController.NetworkPlayerIndex}");
                }
            }
            return;
        }

        // Check if it's this player's turn
        if (!IsMyTurn())
        {
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        RaycastHit hit;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(r, out hit, 1000))
        {
            var p = new Dictionary<string, object>();
            p["Character"] = BoardStateManager.instance.ModelLookUp(hit.collider.gameObject);
            EventBroadcaster.InvokeEvent(EVENT_NAMES.UI_EVENTS.ON_DISPLAY_CHARACTER, p);
        }
    }

    private bool IsMyTurn()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.Log("GameStateManager not found");
            return false;
        }

        int activePlayerIndex = GameStateManager.Instance.GetActivePlayerIndex();
        bool isMyTurn = localPlayerController.NetworkPlayerIndex == activePlayerIndex;

        return isMyTurn;
    }
}