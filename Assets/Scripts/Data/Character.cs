using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Gameplay/Character", order = 1)]
public class Character : ScriptableObject
{
    public string characterName;
    public int power;
    public int health;
    public GameObject model;
    public Texture2D cardArt;

    [Header("Abilities")]
    public int Ability1ID;
    public int Ability2ID;
    public int Ability3ID;

}
