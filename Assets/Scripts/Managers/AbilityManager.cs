using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AbilityManager: MonoBehaviour
{
    #region Singleton
    public static AbilityManager instance { get; private set; } = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Build();
    }
    #endregion

    private Dictionary<int, Ability> abilityDict = new();

    //Debugging

    [Serializable]
    private class Debugger
    {
        public int ID;
        public string Name;
    }

    [SerializeField] private List<Debugger> debuggerList = new();

    public static Ability GetMove(int key) => instance.abilityDict[key];

    public void Build()
    {
        Type moveType = typeof(Ability);
        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> types = assembly.GetTypes();

        types = types.Where(t => t.IsSubclassOf(moveType));

        foreach (Type type in types)
        {
            var move = Activator.CreateInstance(type) as Ability;

            if(abilityDict.ContainsKey(move.ID))
            {
                Debug.LogError($"Could not add {move.GetType().Name} as ID [{move.ID}] already exists for {abilityDict[move.ID].GetType().Name}");
                continue;
            }

            abilityDict.Add(move.ID, move);
        }

        DebugDict();
    }

    public void DebugDict()
    {
        foreach(var move in abilityDict)
        {
            var d = new Debugger();
            d.ID = move.Value.ID;
            d.Name = move.Value.GetType().Name;
            debuggerList.Add(d);    
        }
    }


}
