using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerAbility3 : Ability
{
    public override int ID => 106;

    public override bool isQuickOrSlow => false;

    public override int manaCost => 1;

    public override string AbilityName => "Alice: Survival Trick";

    public override string Description => "Joker gains 3 Health. He then swaps his Power and Health. If the swap would leave him with 0 health, it becomes 1 instead.";

    public override ETargetType TargetType => ETargetType.SELF;

    public override void ExecuteAbility(Unit Attacker, List<Unit> Targets)
    {
        Attacker.ChangeHealth(3);

        int temp = Attacker.CurrentPower;
        Attacker.CurrentPower = Attacker.CurrentHealth;
        Attacker.CurrentHealth = temp;

        Attacker.CurrentHealth = Mathf.Max(Attacker.CurrentHealth, 0);

    }
}
