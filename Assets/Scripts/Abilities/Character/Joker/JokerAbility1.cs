using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerAbility1 : Ability
{
    public override int ID => 104;

    public override bool isQuickOrSlow => true;

    public override int manaCost => 4;

    public override string AbilityName => "Arsene: Dream Needles";

    public override string Description => "Attacks Forward. Joker's Power becomes 0.";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        //Attack Forward
        Attacker.CurrentPower = 0;
    }
}
