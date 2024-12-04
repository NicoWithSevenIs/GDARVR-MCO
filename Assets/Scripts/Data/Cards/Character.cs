using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Gameplay/Character", order = 1)]
public class Character : Card
{

    [Header("Character Model")]
    public GameObject model;

    [Header("Character Stats")]
    public int power;
    public int health;

    [Header("Abilities")]
    public int Ability1ID;
    public int Ability2ID;
    public int Ability3ID;

}
