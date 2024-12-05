using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerAbility2 : Ability
{
    public override int ID => 105;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 2;

    public override string AbilityName => "Wild Card";

    public override string Description => "Moves to a Random Empty Tile";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        
    }
}
