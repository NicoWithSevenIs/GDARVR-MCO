using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Image characterKeyArt;

    [SerializeField] private TextMeshProUGUI power;
    [SerializeField] private TextMeshProUGUI health;


    private CanvasGroup canvasGroup;

    private Unit Requester;
    private List<int> l;

    private bool canInvoke = true;


    private PlayerController localPlayerController;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UI_Utilities.SetUIActive(canvasGroup, false);
        EventBroadcaster.AddObserver(EVENT_NAMES.UI_EVENTS.ON_DISPLAY_CHARACTER, LoadCharacter);

        EventBroadcaster.AddObserver(EVENT_NAMES.UI_EVENTS.ON_SELECTION_INVOKED, t => {
            UI_Utilities.SetUIActive(canvasGroup, false);
            canInvoke = false;
        });

        EventBroadcaster.AddObserver(EVENT_NAMES.UI_EVENTS.ON_SELECTION_ENDED, t => { 
            canInvoke = true;

            if ((bool)t["Will Show Panel"])
                UI_Utilities.SetUIActive(canvasGroup, true);
        });


    }

    private void Start()
    {
        if (NetworkClient.connection != null && NetworkClient.connection.identity != null)
        {
            localPlayerController = NetworkClient.connection.identity.GetComponent<PlayerController>();
        }

    }

    public void Back()
    {
        UI_Utilities.SetUIActive(canvasGroup, false);
    }

    private void LoadCharacter(Dictionary<string, object> p )
    {
        if (!canInvoke)
            return;

        var u = p["Character"] as Unit;



        if (localPlayerController.NetworkPlayerIndex != u.OwnerID)
            return;

            var c = u.Character;
        characterName.text = c.name;

        UI_Utilities.SetUIActive (canvasGroup, true);

        characterKeyArt.sprite = Sprite.Create(c.keyArt, new Rect(0,0,c.keyArt.width, c.keyArt.height), Vector2.one * 0.5f);

        Requester = u;
        l = new List<int>() { c.Ability_1_ID, c.Ability_2_ID, c.Ability_3_ID };

        Debug.Log("new:" + u.CurrentHealth);

        health.text = $"{u.CurrentHealth}";
        power.text = $"{u.CurrentPower}";

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(l[i] != 0);

            if (l[i] == 0)
                continue;

            var buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            Ability a = AbilityManager.GetAbility(l[i]);
            string quickIndicator = a.isQuickOrSlow ? "<b>[Quick]</b>" : "";
            buttonText.text = $"<b>({a.manaCost})</b> {quickIndicator} {a.AbilityName}";
        }
    }

    public void InvokeAbility(int index)
    {
        if (l == null)
            return;

        index = Mathf.Clamp(index, 0, l.Count-1);

        var p = new Dictionary<string ,object>();
        p["Ability"] = AbilityManager.GetAbility(l[index]);
        p["Requester"] = Requester;
        EventBroadcaster.InvokeEvent(EVENT_NAMES.UI_EVENTS.ON_SELECTION_INVOKED, p);
    }
}
