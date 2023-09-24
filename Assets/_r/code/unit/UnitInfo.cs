using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    public static UnitInfo instance;

    public UnitService service;

    public delegate void unitDelegate(UnitBase unit);
    public delegate void tileDataRequestDelegate(UnitBase unit, AbilityData data);
    
    public static event unitDelegate EmitUnitOccupant;
    public static event tileDataRequestDelegate EmitTileDataRequest;
    
    public void __EmitTileDataRequest(UnitBase unit, AbilityData data)
    {
        if (EmitTileDataRequest != null) EmitTileDataRequest(unit, data);
    }

    public void __EmitUnitOccupant(UnitBase unit)
    {
        if (EmitUnitOccupant != null) EmitUnitOccupant(unit);
    }
    
    public void __init()
    {
        InitSingleton();
    }
    
    private void InitSingleton()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
    }

    private void OnEnable()
    {
        __init();
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
            
        }
        else
        {
            
        }
    }

    public void RegisterUnitOccupant(UnitBase unit)
    {
        __EmitUnitOccupant(unit);
    }

    public void RequestTileData(UnitBase unit, AbilityData data)
    {
        __EmitTileDataRequest(unit, data);
    }
}
