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

    public static Ability GetAbility(int key) => instance.abilityDict[key];

    public void Build()
    {
        Type type = typeof(Ability);
        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> types = assembly.GetTypes();
 
        types = types.Where(t => t.IsSubclassOf(type));

        foreach (Type t in types)
        {
            var ability = Activator.CreateInstance(t) as Ability;

            if(abilityDict.ContainsKey(ability.ID))
            {
                Debug.LogError($"Could not add {ability.GetType().Name} as ID [{ability.ID}] already exists for {abilityDict[ability.ID].GetType().Name}");
                continue;
            }

            abilityDict.Add(ability.ID, ability);
        }

        DebugDict();
    }

    public void DebugDict()
    {
        foreach(var ability in abilityDict)
        {
            var d = new Debugger();
            d.ID = ability.Value.ID;
            d.Name = ability.Value.GetType().Name;
            debuggerList.Add(d);    
        }
    }


}
