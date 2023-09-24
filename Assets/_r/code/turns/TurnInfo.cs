using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnInfo : MonoBehaviour
{
    public static TurnInfo    instance;
    public        TurnService service;
    public        bool        tickActive = false;

    private List<WaitForSeconds> tickSpeed = new List<WaitForSeconds>();
    private int                  currentTickSpeed = 0;

    
    public delegate void                 noneDelegate();
    public static event noneDelegate     TurnTick;
    private void                     __TurnTick()
    {
        if (TurnTick != null) TurnTick();
    }

    public delegate void UnitDelegate(UnitTurn _unit);
    public static event UnitDelegate EmitUnitTurn;
    public void __EmitUnitTurn(UnitTurn _unit)
    {
        if (EmitUnitTurn != null) EmitUnitTurn(_unit);
    }

    private Coroutine tickCoroutine;
    
    public void __init()
    {
        InitSingleton();
        InitTickSpeed();
        tickActive = true;
        if (tickCoroutine != null) StopCoroutine(tickCoroutine);
        tickCoroutine = StartCoroutine(TurnTickCoroutine());
    }

    private void InitSingleton()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
    }

    private void InitTickSpeed()
    {
        tickSpeed.Add(new WaitForSeconds(4f));
        tickSpeed.Add(new WaitForSeconds(2f));
        tickSpeed.Add(new WaitForSeconds(1f));
        tickSpeed.Add(new WaitForSeconds(0.5f));
        tickSpeed.Add(new WaitForSeconds(0.25f));
        tickSpeed.Add(new WaitForSeconds(0.1f));
        currentTickSpeed = 0;
    }

    private IEnumerator TurnTickCoroutine()
    {
        while (true)
        {
            yield return tickSpeed[currentTickSpeed];
            if (tickActive)
            {
                Debug.Log("sending tick...");
                __TurnTick();
            }
            else Debug.Log("waiting for tickActive...");
        }
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
            UnitTurn.RequestTurn += HandleRequestTurn;
            UnitTurn.EndTurn     += HandleEndTurn;
        }
        else
        {
            UnitTurn.RequestTurn -= HandleRequestTurn;
            UnitTurn.EndTurn     -= HandleEndTurn;
        }
    }

    private void HandleRequestTurn(UnitTurn _unit)
    {
        tickActive = false;
        service.AddToTurnQueue(_unit);
    }

    private void HandleEndTurn()
    {
        Debug.Log($"unit ended turn, remaining queue'd turns: {service.queueOrder.Count}");
        if (service.queueOrder.Count > 0)
        {
            service.ProcessQueue();
        }
        else
        {
            tickActive = true;
            Debug.Log("setting tickActive to true");
        }
    }
    
    //void Start() {}
    //void Update() {}
}
