using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : MonoBehaviour
{
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
            

        }
        else
        {
            
        }
    }
    //void Start() {}
    //void Update() {}
}
