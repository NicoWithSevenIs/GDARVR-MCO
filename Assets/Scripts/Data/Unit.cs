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

    public void ChangePower(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
    }

    public void ChangeHealth(int amount, bool willKill = true)
    {
        currentHealth -= amount;

        if (willKill)
            Debug.Log("Dead");
        else currentHealth = 1;
        
    }
}
