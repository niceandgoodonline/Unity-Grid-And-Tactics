using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geomancy : AbilityBase
{
    [Header("Data")]
    public  GeomancyCollection collection;
    public  GeomancyKnown      known;

    [Header("Debug")] 
    public bool         debug;
    public List<string> knownPeak;
    
    // Dependencies for some functionality
    private SaveLoadData       dataService;
    public void __init(SaveLoadData _ds)
    {
        dataService = _ds;
        if (known is null) known = new GeomancyKnown();
        if (debug) knownPeak = new List<string>();

        foreach (string _k in collection.active.Keys)
        {
            Debug.Log($"geomancy key: {_k}");
            Learn(_k);
        }
    }

    public void Learn(string abilityName)
    {
        if (collection.active.ContainsKey(abilityName))
        {
            known.Add(abilityName);
            if(debug) knownPeak.Add(abilityName);
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
            SaveLoadData.InitData += __init;
        }
        else
        { 
            SaveLoadData.InitData -= __init;
        }
    }
}
