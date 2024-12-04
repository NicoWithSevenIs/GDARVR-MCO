using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleThreat : Ability
{
    public override int ID => 4;

    public override string AbilityName => "Triple Threat";

    public override string Description =>"An Ally gains +3 Power and +3 Health";

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        throw new System.NotImplementedException();
    }
}
