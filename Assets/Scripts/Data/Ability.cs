using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability { 
    public abstract int ID { get; }
    public abstract void ExecuteAbility(Unit Attacker, Unit Target);

}
