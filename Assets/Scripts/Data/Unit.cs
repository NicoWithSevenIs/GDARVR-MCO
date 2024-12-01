using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    private Character character;
    private int currentPower;
    private int currentHealth;

    public Unit(Character character)
    {
        this.character = character;
        currentHealth = character.health;
        currentPower = character.power;
    }

    public void ReducePower(int amount)
    {
        currentPower -= amount;
    }

    public void ReduceHealth(int amount)
    {
        currentHealth -= amount;
    }
}
