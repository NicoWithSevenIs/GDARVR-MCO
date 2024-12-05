using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [Header("Player 1 Info")]
    [SerializeField]
    private TextMeshProUGUI P1NexusHP;

    [SerializeField]
    private TextMeshProUGUI P1ManaCount;

    [SerializeField]
    private TextMeshProUGUI P1ActionCount;

    [Header("Player 2 Info")]
    [SerializeField]
    private TextMeshProUGUI P2NexusHP;

    [SerializeField]
    private TextMeshProUGUI P2ManaCount;

    [SerializeField]
    private TextMeshProUGUI P2ActionCount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        P1NexusHP.text = "HP: " + GameStateManager.Instance.GetPlayer1NexusHP().ToString();
        P1ManaCount.text = GameStateManager.Instance.GetPlayer1Mana().ToString();
        P1ActionCount.text = GameStateManager.Instance.GetPlayer1ActionBudget().ToString();

        P2NexusHP.text = "HP: " + GameStateManager.Instance.GetPlayer2NexusHP().ToString();
        P2ManaCount.text = GameStateManager.Instance.GetPlayer2Mana().ToString();
        P2ActionCount.text = GameStateManager.Instance.GetPlayer2ActionBudget().ToString();
    }
}
