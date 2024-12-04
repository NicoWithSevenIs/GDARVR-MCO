using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BoardStateManager : MonoBehaviour
{
    #region Singleton
    public static BoardStateManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
        //characters = new(Resources.LoadAll("Resources/Characters") as Character[]);
    }
    #endregion

    //private List<Character> characters;

    private Character[] characters;

    private List<Unit> playerAUnits = new();
    private List<Unit> playerBUnits = new();

    public List<Unit> PlayerAUnit { get => playerAUnits; }
    public List<Unit> PlayerBUnit { get => playerBUnits; }


    private void Start()
    {
        characters = Resources.LoadAll("Resources/Character")as Character[];
    }

    public Unit ModelLookUp(GameObject model)
    {
        List<Unit> lookup = new(playerAUnits);
        lookup.AddRange(playerBUnits);
        foreach (var unit in lookup)
            if (model == unit.FieldObject)
                return unit;
        return null;
    }

    public void MakeCharacter(int PlayerIndex, GameObject model)
    {
        Character c = Array.Find<Character>(characters, t => t.model.name == model.name);
        var UnitPool = PlayerIndex == 1 ? playerAUnits : playerBUnits;
        var newUnit = new Unit(c, model);
        UnitPool.Add(newUnit);
    }


}
