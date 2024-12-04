using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowYouveDoneIt : Ability
{
    public override int ID => 9;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 5;

    public override string AbilityName => "Now You've Done It!";

    public override string Description => "An Ally gains +2 Power and Attacks Forward";

    public override ETargetType TargetType => ETargetType.ALLY;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangePower(2);
        //Attack Forward
    }
}
