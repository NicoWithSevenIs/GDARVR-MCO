using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMayonnaiseAnInstrument : Ability
{
    public override int ID => 14;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 1;

    public override string AbilityName => "Is Mayonnaise an Instrument";

    public override string Description => "An Enemy gets -3 Power";

    public override ETargetType TargetType => ETargetType.ENEMY_SINGLE;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Targets[0].ChangePower(-3);
    }
}
