using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyLittleRat : Ability
{
    public override int ID => 10;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 3;

    public override string AbilityName => "GreedyLittleRat";

    public override string Description => "Move an ally to another tile. They gain +2 Power";

    public override ETargetType TargetType => ETargetType.ALLY;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangePower(2);
    }
}
