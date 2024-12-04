using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Gameplay/Spell", order = 2)]
public class Spell : Card
{
    [Header("Spell Effect")]
    public int AbilityID;
}
