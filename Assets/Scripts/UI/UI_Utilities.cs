using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UI_Utilities
{
    public static void SetUIActive(CanvasGroup group, bool active)
    {
        group.alpha = active ? 1 : 0;
        group.interactable = active;
        group.blocksRaycasts = active;
    }

}
