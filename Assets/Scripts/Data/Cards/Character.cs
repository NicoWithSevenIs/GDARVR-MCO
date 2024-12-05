using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Gameplay/Character", order = 1)]
public class Character : Card
{

    [Header("Character Graphics")]
    public Texture2D keyArt;
    public GameObject model;

    [Header("Character Stats")]
    public int power;
    public int health;

    [Header("Abilities")]
    public int Ability_1_ID;
    public int Ability_2_ID;
    public int Ability_3_ID;

}
