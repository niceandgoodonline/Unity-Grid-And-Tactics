using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurn : MonoBehaviour
{
    public int      unitRecoverySpeed = 10;
    public int      unitTurnRecovery  = 100;
    
    public delegate void             SelfDelegate(UnitTurn _self);
    public static event SelfDelegate RequestTurn;
    private void __RequestTurn(UnitTurn _self)
    {
        if (RequestTurn != null) RequestTurn(_self);
    }

    public delegate void noneDelegate();
    public static event noneDelegate EndTurn;
    private void __EndTurn()
    {
        if (EndTurn != null) EndTurn();
    }
    
    public void __init()
    {
        
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
            TurnInfo.TurnTick += HandleTurnTick;
        }
        else
        { 
            TurnInfo.TurnTick -= HandleTurnTick;
        }
    }

    public void StartTurn()
    {
        
    }
    
    private void HandleTurnTick()
    {
        unitTurnRecovery -= unitRecoverySpeed;
        if (unitTurnRecovery < 1) __RequestTurn(this);
    }

    public void SetTurnRecovery(int _recovery)
    {
        unitTurnRecovery = _recovery;
    }

    public void PassTurn()
    {
        if (unitTurnRecovery < 30) unitTurnRecovery = Mathf.Clamp(30 - unitRecoverySpeed, 1, 50);
        Debug.Log($"{gameObject.name} next turn after {unitTurnRecovery}");
        __EndTurn();
    }
    
    //void Start() {}
    //void Update() {}
}
