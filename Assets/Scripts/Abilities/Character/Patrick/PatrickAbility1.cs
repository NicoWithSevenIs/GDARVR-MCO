using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrickAbility1 : Ability
{
    public override int ID => 11;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 4;

    public override string AbilityName => "FIRMLY GRASP IT!";

    public override string Description => "Moves an Enemy to the Tile Forward if it's empty. They cannot move for the rest of the round.";

    public override ETargetType TargetType => ETargetType.ENEMY_SINGLE;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
    }
}
