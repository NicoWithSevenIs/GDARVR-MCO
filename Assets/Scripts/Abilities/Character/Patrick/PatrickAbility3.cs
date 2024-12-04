using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrickAbility3 : Ability
{
    public override int ID => 13;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 7;

    public override string AbilityName => "The Inner Machinations of my Mind are an Enigma";

    public override string Description => "Restores Health to the Base Equal to Patrick's Lost Health";

    public override ETargetType TargetType => throw new System.NotImplementedException();

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        int restore = Attacker.HighestHealth - Attacker.CurrentHealth;
        //Change Base here
    }
}
