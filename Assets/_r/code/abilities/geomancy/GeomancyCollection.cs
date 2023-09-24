using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "GeomancyCollection", menuName = "Ability/Geomancy/GeomancyCollection", order = 1)]
public class GeomancyCollection : ScriptableObject
{
    public Dictionary<string, GeomancyAbility> active;
    public List<string>                        keyPeak = new List<string>();
    public void __init(SaveLoadData _ds)
    {
        active = new Dictionary<string, GeomancyAbility>();
        foreach (string _k in active.Keys)
        {
            keyPeak.Add(_k);
        }
    }
    
    private void OnEnable()
    {
        SubscribeToEvents(true);
    }
    
    private void OnDisable()
    {
        SubscribeToEvents(false);
    }
    
    private void SubscribeToEvents(bool state)
    {
        if(state)
        {
            SaveLoadData.InitData    += __init;
            GeomancyAbility.Register += Register;
        }
        else
        {
            SaveLoadData.InitData    -= __init;
            GeomancyAbility.Register -= Register;
        }
    }

    private void Register(GeomancyAbility data)
    {
        if (!active.ContainsKey(data.key)) active.Add(data.key, data);
    }
}
