using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurinaAbility3 : Ability
{
    public override int ID => 3;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 7;

    public override string AbilityName => "Let the People Rejoice!";

    public override string Description => "Sets Furina's Power to 1. All Allies gain Power equal to the Power she lost";

    public override ETargetType TargetType => ETargetType.OTHER_ALLIES;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        int sharedPower = Mathf.Max(0, Attacker.CurrentPower-1);
        foreach (var unit in Targets)
            unit.ChangePower(sharedPower);
        Attacker.CurrentPower = 1;
    }
}
