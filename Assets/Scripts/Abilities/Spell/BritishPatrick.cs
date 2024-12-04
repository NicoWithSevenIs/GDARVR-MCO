using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BritishPatrick : Ability
{
    public override int ID => 15;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 6;

    public override string AbilityName => "British Patrick";

    public override string Description => "Restores an ally's health to full. That ally then Attacks Forward for the amount healed";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        int HealAmount = Attacker.HighestHealth - Attacker.CurrentHealth;
        Attacker.ChangeHealth(HealAmount);
        //Attack Forward
    }
}
