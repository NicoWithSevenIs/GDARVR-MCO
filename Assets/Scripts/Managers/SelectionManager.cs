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
        if(instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    #endregion 

    [SerializeField] private GameObject validSelectionIndicator;
    [SerializeField] private float yOffset = 1;

    private List<Unit> GetValidTargets(int PlayerIndex, Unit Requester, ETargetType TargetingSchema)
    {
        var PlayerUnits = PlayerIndex == 1 ? BoardStateManager.instance.PlayerAUnit : BoardStateManager.instance.PlayerBUnit;
        var EnemyUnits = PlayerIndex == 1 ? BoardStateManager.instance.PlayerBUnit : BoardStateManager.instance.PlayerAUnit;

        switch (TargetingSchema)
        {
            case ETargetType.SELF: return new() { Requester };
            case ETargetType.ALLY:
            case ETargetType.ALL_ALLIES: return PlayerUnits;
            case ETargetType.OTHER_ALLIES: return PlayerUnits.Where(t => t != Requester).ToList();
            case ETargetType.ENEMY_SINGLE:
            case ETargetType.ALL_ENEMIES: return EnemyUnits;
        }

        return null;
    }


    public async Task<List<Unit>> MakeSelection(int PlayerIndex, Unit Requester, ETargetType TargetingSchema, CancellationToken cancel)
    {
        List<Unit> validTargets = GetValidTargets(PlayerIndex, Requester, TargetingSchema);

        foreach (var target in validTargets)
        {
            GameObject indicator = Instantiate(validSelectionIndicator);
            indicator.transform.parent = target.FieldObject.transform;
            indicator.transform.localPosition = Vector3.up * yOffset;
        }

        foreach (var target in validTargets)
            Destroy(target.FieldObject);

            return null;
    }

}
