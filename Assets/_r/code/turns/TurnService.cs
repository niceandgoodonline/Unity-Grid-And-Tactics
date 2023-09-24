using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TurnService : MonoBehaviour
{
    public  TurnInfo                        info;
    public  bool                            noQueue = true;
    private Dictionary<UnitTurn, int> turnQueue;

    private Coroutine         turnQueueCoroutine;
    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    public List<UnitTurn> queueOrder;
    
    public void __init()
    {
        noQueue   = true;
        turnQueue = new Dictionary<UnitTurn, int>();
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

    public void AddToTurnQueue(UnitTurn _unit)
    {
        if (noQueue)
        {
            noQueue = false;
            turnQueue.Clear();
            Debug.Log($"NO QUEUE HIT!!!!!! turnQueue count: {turnQueue.Count}");
            turnQueueCoroutine = StartCoroutine(ProcessTurnQueueAtEndOfFrame());
        }
        turnQueue.Add(_unit, _unit.unitTurnRecovery);
    }

    public void ProcessQueue()
    {
        if (queueOrder.Count > 0)
        {
            info.__EmitUnitTurn(queueOrder[0]);
            queueOrder[0].StartTurn();
            queueOrder.RemoveAt(0);
        }
        if (queueOrder.Count == 0)
        {
            noQueue = true;
        }
    }

    private IEnumerator ProcessTurnQueueAtEndOfFrame()
    {
        yield return _waitForEndOfFrame;
        OrderQueue();
        ProcessQueue();
    }

    public void OrderQueue()
    {
        
        List<int> keyList = new List<int>(Enumerable.ToList(LinqUtility.ToHashSet(turnQueue.Values)));
        keyList.Sort();
        queueOrder = new List<UnitTurn>();
        foreach (int _key in keyList)
        {
            foreach (KeyValuePair<UnitTurn, int> item in turnQueue)
            {
                if (item.Value == _key)
                {
                    queueOrder.Add(item.Key);
                }
            }
        }
    }
    //void Start() {}
    //void Update() {}
}
