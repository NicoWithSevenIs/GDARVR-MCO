using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicAbility1 : Ability
{
    public override int ID => 16;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 3;

    public override string AbilityName => "Spin Attack";

    public override string Description => "Attacks Forward with his Power plus 2. If the Attack defats an opposing enemy, the excess damage is dealt to the Base Instead.";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        int damage = Attacker.CurrentPower + 2;
        //Attack Forward
    }
}
