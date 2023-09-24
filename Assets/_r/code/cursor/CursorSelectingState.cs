using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSelectingState : CursorBaseState
{
    public override void EnterState(CursorStateManager cursor)
    {
        Debug.Log("cursor has entered: CursorSelectingState");
    }

    public override void UpdateState(CursorStateManager cursor)
    {
        Debug.Log("cursor has updated: CursorSelectingState");
    }

    public override void SubscribeToEvents(CursorStateManager cursor, bool state)
    {
        if (state)
        {
            cursor.input.actionMaps[cursor.currentMap]["Secondary"].performed += cursor.SelectingSecondary;
        }
        else
        {
            cursor.input.actionMaps[cursor.currentMap]["Secondary"].performed -= cursor.SelectingSecondary;
        }
    }
}
