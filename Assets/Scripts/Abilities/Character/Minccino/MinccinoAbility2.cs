using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinccinoAbility2 : Ability
{
    public override int ID => 8;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 4;

    public override string AbilityName => "Technician";

    public override string Description => "Increases Mincinno's Power by 4";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangePower(4);
    }
}


