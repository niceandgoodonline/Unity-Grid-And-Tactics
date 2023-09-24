using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTargetingState : CursorBaseState
{
    public override void EnterState(CursorStateManager cursor)
    {
        Debug.Log("cursor has entered: CursorFreeState");
    }

    public override void UpdateState(CursorStateManager cursor)
    {
        Debug.Log("cursor has updated: CursorFreeState");
    }

    public override void SubscribeToEvents(CursorStateManager cursor, bool state)
    {
        if (state)
        {
            
        }
        else
        {
            
        }
    }
}
