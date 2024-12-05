using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewDebugger : MonoBehaviour
{
    [SerializeField] private Character c;
    private void Start()
    {
        var p = new Dictionary<string, object>();
        p["Character"] = new Unit(c, null);
        EventBroadcaster.InvokeEvent(EVENT_NAMES.UI_EVENTS.ON_DISPLAY_CHARACTER, p);
    }

   
}
