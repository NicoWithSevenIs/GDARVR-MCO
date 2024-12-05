using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    #region Singleton

    public static SelectionManager instance { get; private set; } = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    #endregion 

    public int PlayerIndex;
    [SerializeField] private GameObject validSelectionIndicator;
    [SerializeField] private float yOffset = 1;


    public bool isSelectorBusy { get => selectionSubject != null; }
    public Ability selectionSubject = null;
    private Unit requester;

    private List<GameObject> indicators = new();
    List<Unit> validSelectables = new();


    public List<Unit> selection;

    private void Start()
    {

        EventBroadcaster.AddObserver(EVENT_NAMES.UI_EVENTS.ON_SELECTION_INVOKED, t => RequestSelection(t["Ability"] as Ability, t["Requester"] as Unit) );
        EventBroadcaster.AddObserver(EVENT_NAMES.UI_EVENTS.ON_SELECTION_ENDED, t => AbortSelection());

    }

    private List<Unit> GetValidTargets(int PlayerIndex, ETargetType TargetingSchema)
    {
        var PlayerUnits = PlayerIndex == 1 ? BoardStateManager.instance.PlayerAUnit : BoardStateManager.instance.PlayerBUnit;
        var EnemyUnits = PlayerIndex == 1 ? BoardStateManager.instance.PlayerBUnit : BoardStateManager.instance.PlayerAUnit;

        switch (TargetingSchema)
        {
            case ETargetType.SELF: return new() { requester };
            case ETargetType.ALLY:
            case ETargetType.ALL_ALLIES: return PlayerUnits;
            case ETargetType.OTHER_ALLIES: 
                    var list = new List<Unit>(PlayerUnits);
                    list.Remove(requester);
                    return list;
            case ETargetType.ENEMY_SINGLE:
            case ETargetType.ALL_ENEMIES: return EnemyUnits;
        }

        return null;
    }

    public void RequestSelection(Ability subject, Unit Requester)
    {
        if(isSelectorBusy) return;

        selectionSubject = subject;
        requester = Requester;

        validSelectables = GetValidTargets(PlayerIndex, subject.TargetType);

        Debug.Log(validSelectables.Count + " " + subject.TargetType.ToString());

        foreach (Unit validSelectable in validSelectables)
        {
            GameObject indicator = Instantiate(validSelectionIndicator);
            indicator.transform.parent = validSelectable.FieldObject.transform;
            indicator.transform.localPosition = Vector3.up * yOffset;
            indicators.Add(indicator);
        }

    }

    private void AbortSelection()
    {
        selectionSubject = null;
        foreach (var indicator in indicators)
            Destroy(indicator);
        indicators.Clear();
    }

    private void Update()
    {
        //
        if(!isSelectorBusy || !Input.GetMouseButtonDown(0)) return;

        RaycastHit hit;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(r, out hit, 1000))
            return;

        Unit unitHit = BoardStateManager.instance.ModelLookUp(hit.collider.gameObject);

        if (!validSelectables.Contains(unitHit))
            return;


        List<Unit> targets = new();

        switch (selectionSubject.TargetType)
        {
            case ETargetType.SELF: targets.Add(requester); break;
            case ETargetType.ENEMY_SINGLE:
            case ETargetType.ALLY: targets.Add(unitHit); break;
            case ETargetType.OTHER_ALLIES: 
            case ETargetType.ALL_ALLIES: 
            case ETargetType.ALL_ENEMIES: targets.AddRange(validSelectables); break;
        }

        selectionSubject.ExecuteAbility(requester, targets);

        var p = new Dictionary<string, object>();
        p["Will Show Panel"] = false;
        EventBroadcaster.InvokeEvent(EVENT_NAMES.UI_EVENTS.ON_SELECTION_ENDED, p);

    }

 
}
