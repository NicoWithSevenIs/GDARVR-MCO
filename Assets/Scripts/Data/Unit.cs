using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    private Character character;

    private int currentPower;
    public int CurrentPower { get => currentPower; set => currentPower = value; }

    private int currentHealth;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    private int hightestHealth;
    public int HighestHealth { get => hightestHealth; }
    public Unit(Character character)
    {
        this.character = character;
        currentHealth = character.health;
        currentPower = character.power;

        hightestHealth = character.health;
    }

    public void ChangePower(int amount)
    {
        currentHealth = Mathf.Max(0, currentPower + amount);
    }

    public void ChangeHealth(int amount, bool willKill = true)
    {

        currentHealth = Mathf.Max(0, currentHealth + amount);
        
        if(currentHealth > hightestHealth)
            hightestHealth = currentHealth;

        if (currentHealth > 0)
            return;

        if (CurrentHealth <= 0)
        {
            if (willKill)
                Debug.Log("Dead");
            else currentHealth = 1;
        }
        
    }
}
