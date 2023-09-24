using UnityEngine;

public abstract class CursorBaseState
{
    public bool doUpdate = false;
    public abstract void EnterState(CursorStateManager       cursor);
    public abstract void UpdateState(CursorStateManager      cursor);
    public abstract void SubscribeToEvents(CursorStateManager cursor, bool state);
}
