using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [Header("Dependencies")]
    public UnitPathFinding    pathFinding;
    public UnitTurn           turn;
    public UnitInfo           units;

    [Header("Data")]
    public UnitPersistantData persistantData;
    public AbilityData moveData;

    [Header("debug")] 
    public string unitName;

    public string unitClass, currentEnergy, maxEnergy;


    private bool              tileDataUpdated;
    private Coroutine         previewCoroutine;
    public  HashSet<TileData> latestTileData;
    public void __init()
    {
        if (persistantData == null) persistantData = new UnitPersistantData();
        SetPersistantData();
        StartCoroutine(AwaitDependencies());
    }

    private void SetPersistantData()
    {
        persistantData.unitName      = unitName;
        persistantData.unitClass     = unitClass;
        persistantData.currentEnergy = currentEnergy;
        persistantData.maxEnergy     = maxEnergy;
    }

    private IEnumerator AwaitDependencies()
    {
        while (units == null)
        {
            yield return null;
            units = UnitInfo.instance;
        }
        units.RegisterUnitOccupant(this);
        SubscribeToEvents(true);
    }

    private void OnEnable()
    {
        __init();
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

    public void MovePreview()
    {
        tileDataUpdated = false;
        units.RequestTileData(this, moveData);
    }


    public void SetTileData(HashSet<TileData> tileData, AbilityData data)
    { 
        pathFinding.HandleAbilityPreview(data, tileData);
    }
}
