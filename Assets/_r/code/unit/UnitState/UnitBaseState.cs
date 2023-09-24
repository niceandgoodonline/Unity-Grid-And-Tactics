public abstract class UnitBaseState
{
    public bool doUpdate = false;
    public abstract void EnterState(UnitStateManager       unit);
    public abstract void UpdateState(UnitStateManager      unit);
    public abstract void SubscribeToEvents(UnitStateManager unit, bool state);
}
