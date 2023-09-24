using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorStateManager : MonoBehaviour
{
    public InputEvents   input;
    public CameraService camService;
    public string        currentMap = "none";
    
    private CursorBaseState currentState;
    
    private CursorFreeState      freeState      = new CursorFreeState();
    private CursorSelectingState selectingState = new CursorSelectingState();
    private CursorMenuState      menuState      = new CursorMenuState();
    private CursorTargetingState targetingState = new CursorTargetingState();
    
    public void __init()
    {
        if (input != null)
        {
            currentState.SubscribeToEvents(this, false);
            currentMap   = input.currentMap;
            currentState.SubscribeToEvents(this, true);
        }
        else StartCoroutine(AwaitDependencies());
        
        currentState = freeState;
        currentState.EnterState(this);
    }

    private IEnumerator AwaitDependencies()
    {
        while (input == null)
        {
            yield return null;
            input = InputEvents.instance;
        }

        currentMap = input.currentMap;
        currentState.SubscribeToEvents(this, true);
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
    
    // void Update()
    // {
    //     if (currentState.doUpdate)
    //     {
    //         currentState.UpdateState(this);
    //     }
    // }

    public void SwitchState(CursorBaseState state)
    {
        currentState.SubscribeToEvents(this, false);
        currentState = state;
        currentState.SubscribeToEvents(this, true);
        state.EnterState(this);
    }

    public void FreeMoveCursor(InputAction.CallbackContext cx)
    {
        camService.FreeMoveCursor();
    }

    public void FreePrimary(InputAction.CallbackContext cx)
    {
        if (camService._unit != null)
        {
            camService.SelectingPrimary();
            SwitchState(selectingState);
        }
        else
        {
            Debug.Log("no unit to select!");
        }
    }
    
    public void SelectingSecondary(InputAction.CallbackContext cx)
    {
        SwitchState(freeState);
        camService.__EmitNoUnitMenu();
        camService.FreeMoveCursor();
    }
}
