using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beelzebub : Ability
{
    public override int ID => 101;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 7;

    public override string AbilityName => "Beelzebub: Megidolaon";

    public override string Description => "Deal 5 damage to all foes";

    public override ETargetType TargetType => ETargetType.ALL_ENEMIES;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        foreach (var t in Targets)
            t.ChangeHealth(-5);
    }
}
