using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicAbility2 : Ability
{
    public override int ID => 17;

    public override bool isQuickOrSlow => true;

    public override int manaCost => 2;

    public override string AbilityName => "Quills Sense";

    public override string Description => "Sonic gains +2 Power";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangePower(2);
    }
}
