using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETargetType
{
    SELF,
    ALLY,
    OTHER_ALLIES,
    ENEMY_SINGLE,
    ALL_ENEMIES,
    ALL_ALLIES,
}

public abstract class Ability { 
    public abstract int ID { get; }


    public abstract bool isQuickOrSlow {  get; }


    public abstract int manaCost { get; }

    public abstract string AbilityName { get; }
    public abstract string Description { get; }

    public abstract ETargetType TargetType { get; }

    public abstract void ExecuteAbility(Unit Attacker, List<Unit> Targets);

}
