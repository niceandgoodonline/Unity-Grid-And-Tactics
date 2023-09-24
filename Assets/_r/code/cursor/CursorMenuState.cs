using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMenuState : CursorBaseState
{
    public override void EnterState(CursorStateManager cursor)
    {
        Debug.Log("cursor has entered: CursorMenuState");
    }

    public override void UpdateState(CursorStateManager cursor)
    {
        Debug.Log("cursor has updated: CursorMenuState");
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
