using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrickAbility2 : Ability
{
    public override int ID => 12;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 3;

    public override string AbilityName => "Wumbo";

    public override string Description => "Patrick Restores 5 Health";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangeHealth(5);
    }
}
