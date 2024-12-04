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

    [SerializeField] private GameObject validSelectionIndicator;
    [SerializeField] private float yOffset = 1;


    public Ability selectionSubject = null;
    private Unit requester;

    private List<GameObject> indicators = new();
    List<Unit> validSelectables = new();


    public List<Unit> selection;

    private List<Unit> GetValidTargets(int PlayerIndex, ETargetType TargetingSchema)
    {
        var PlayerUnits = PlayerIndex == 1 ? BoardStateManager.instance.PlayerAUnit : BoardStateManager.instance.PlayerBUnit;
        var EnemyUnits = PlayerIndex == 1 ? BoardStateManager.instance.PlayerBUnit : BoardStateManager.instance.PlayerAUnit;

        switch (TargetingSchema)
        {
            case ETargetType.SELF: return new() { requester };
            case ETargetType.ALLY:
            case ETargetType.ALL_ALLIES: return PlayerUnits;
            case ETargetType.OTHER_ALLIES: return PlayerUnits.Where(t => t != requester).ToList();
            case ETargetType.ENEMY_SINGLE:
            case ETargetType.ALL_ENEMIES: return EnemyUnits;
        }

        return null;
    }

    public void RequestSelection(Ability subject, int PlayerIndex, Unit Requester)
    {
        if (subject != null)
            return;

        selectionSubject = subject;
        requester = Requester;

        validSelectables = GetValidTargets(PlayerIndex, subject.TargetType);

        foreach (Unit validSelectable in validSelectables)
        {
            GameObject indicator = Instantiate(validSelectionIndicator);
            indicator.transform.parent = validSelectable.FieldObject.transform;
            indicator.transform.localPosition = Vector3.up * yOffset;
            indicators.Add(indicator);
        }

   
        //subscribe event here
    }

    public void OnTap(Dictionary <string, object> p)
    {
        Unit unitSelected = BoardStateManager.instance.ModelLookUp(p["Model"] as GameObject);

        if (unitSelected == null || !validSelectables.Contains(unitSelected))
            return;

        selection = new();

        /*
        switch (selectionSubject.TargetType)
        {
            case ETargetType.SELF: selection.Add(requester); break;
            case ETargetType.ALLY: 
            case ETargetType.ENEMY_SINGLE: selectionSubject.


            case ETargetType.ALL_ALLIES: 
            case ETargetType.OTHER_ALLIES:
  
            case ETargetType.ALL_ENEMIES: return EnemyUnits;
        }


        AbortSelection();

        //fire event here
        */
    }

    private void AbortSelection()
    {
        selectionSubject = null;
        foreach (var indicator in indicators)
            Destroy(indicator);
        indicators.Clear();
        //fire event here
    }
}
