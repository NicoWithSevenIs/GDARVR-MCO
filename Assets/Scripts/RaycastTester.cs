using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTester : MonoBehaviour
{

    [SerializeField] private List<GameObject> models;

    private void Start()
    {
        foreach (var model in models)
        {
            BoardStateManager.instance.MakeCharacter(1, model);
        }
    }

    private void Update()
    {
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
