using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Movement,
    Passive,
    Active,
    Support
}

[CreateAssetMenu(fileName = "AbilityData", menuName = "Ability/BaseData", order = 1)]
public class AbilityData : ScriptableObject
{
    public GameObject  prefab;
    public AbilityType type;
    public string      key;
    public string      description;
    public int         horizontalRange, verticalRange, horizontalArea, verticalArea;
    
    private void OnEnable()
    {
        SubscribeToEvents(true);
    }
    
    private void OnDisable()
    {
        SubscribeToEvents(false);
    }
    
    public  virtual void SubscribeToEvents(bool state)
    {
        if(state)
        {
            SaveLoadData.InitData  += InitData;
        }
        else
        {
            SaveLoadData.InitData  -= InitData;
        }
    }
    
    public virtual void Use()
    {
        Debug.Log($"{key} was used!w nice!");
    }

    public virtual void InitData(SaveLoadData _notUsed)
    {
        Debug.Log("TODO for this class!");
    }
}
