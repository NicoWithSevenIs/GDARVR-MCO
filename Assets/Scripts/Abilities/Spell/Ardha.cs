using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ardha : Ability
{
    public override int ID => 102;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 3;

    public override string AbilityName => "Ardha: Salvation";

    public override string Description => "All allies gain +2 Power";

    public override ETargetType TargetType => ETargetType.ALL_ALLIES;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        foreach (var t in Targets)
            t.ChangePower(2);
    }
}
