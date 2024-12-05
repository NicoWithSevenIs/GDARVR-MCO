using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurinaAbility1 : Ability
{
    public override int ID => 1;
    public override ETargetType TargetType => ETargetType.SELF;


    public override string AbilityName => "Salon Solitaire";

    public override string Description => "Reduces Health by 2 to Increase Power by 2";


    public override int manaCost => 3;
    public override bool isQuickOrSlow => true;


    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangeHealth(-2, false);
        Attacker.ChangePower(2);

    }
}
