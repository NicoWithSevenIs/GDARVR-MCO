using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurinaAbility2 : Ability
{
    public override int ID => 2;
    public override ETargetType TargetType => ETargetType.SELF;

    public override string AbilityName => "Singer of Many Waters";

    public override string Description => "Restore 3 Health to Furina or a random Ally";

    public override int manaCost => 2;
    public override bool isQuickOrSlow => false;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangeHealth(3);
        Targets[Random.Range(0, Targets.Count)].ChangeHealth(3);
    }
}
