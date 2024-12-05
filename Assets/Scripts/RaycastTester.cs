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

        // Get the local player controller
        if (NetworkClient.connection != null && NetworkClient.connection.identity != null)
        {
            localPlayerController = NetworkClient.connection.identity.GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        // Return if we don't have a valid player controller
        if (localPlayerController == null)
            return;

 
        if (!Input.GetMouseButtonDown(0))
            return;

        RaycastHit hit;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(r, out hit, 1000))
        {
            var p = new Dictionary<string, object>();
            p["Character"] = BoardStateManager.instance.ModelLookUp(hit.collider.gameObject);
            EventBroadcaster.InvokeEvent(EVENT_NAMES.UI_EVENTS.ON_DISPLAY_CHARACTER, p);
        }
    }
}