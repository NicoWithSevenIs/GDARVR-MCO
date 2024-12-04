using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinccinoAbility1 : Ability
{
    public override int ID => 7;

    public override bool isQuickOrSlow => true;

    public override int manaCost => 0;

    public override string AbilityName => "Tail Slap";

    public override string Description => "Reduces Power by 1 to Attack Forward for 1";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangePower(-1);
        //Attack Forward Here
    }
}
