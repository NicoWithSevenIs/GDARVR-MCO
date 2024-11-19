using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
public class NetworkedPlayerController : NetworkBehaviour
{
    private Camera arCamera;

    void Start()
    {
        // Only execute camera setup for the local player
        if (isLocalPlayer)
        {
            arCamera = Camera.main;
            // Additional local player setup here
        }
    }

    void Update()
    {
        // Only process input for the local player
        if (!isLocalPlayer) return;

        // Add your player movement/interaction code here
    }
}