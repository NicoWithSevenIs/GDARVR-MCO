using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AbortSelection : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UI_Utilities.SetUIActive(canvasGroup, false);

        EventBroadcaster.AddObserver(EVENT_NAMES.UI_EVENTS.ON_SELECTION_INVOKED, t => UI_Utilities.SetUIActive(canvasGroup, true));
    }

    public void OnAbort()
    {
        var p = new Dictionary<string, object>();
        p["Will Show Panel"] = true;
        EventBroadcaster.InvokeEvent(EVENT_NAMES.UI_EVENTS.ON_SELECTION_ENDED, p);
        UI_Utilities.SetUIActive(canvasGroup, false);
    }



}
