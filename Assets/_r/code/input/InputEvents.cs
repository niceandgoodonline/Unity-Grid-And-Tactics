using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEvents : MonoBehaviour
{
    public static InputEvents instance;
    public string currentMap;
    
    public PlayerControls                                      controls;
    public Dictionary<string, InputActionMap>                  actionMaps;
    public Dictionary<string, Dictionary<string, InputAction>> inputActions;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        instance = this;
        __init();
    }

    public void __init()
    {
        controls     = new PlayerControls();
        actionMaps   = new Dictionary<string, InputActionMap>();
        inputActions = new Dictionary<string, Dictionary<string, InputAction>>();

        IEnumerator<InputAction> x = controls.GetEnumerator();
        while (x.MoveNext())
        {
            if (!inputActions.ContainsKey(x.Current.actionMap.name))
            {
                actionMaps.Add(x.Current.actionMap.name, x.Current.actionMap);
                inputActions.Add(x.Current.actionMap.name, new Dictionary<string, InputAction>());
            }

            inputActions[x.Current.actionMap.name].Add(x.Current.name, x.Current);
        }

        ToggleActionMap(currentMap, true);
    }

    public void ToggleActionMap(string actionMapName, bool state)
    {
        if (inputActions.ContainsKey(actionMapName))
        {
            if (state) actionMaps[actionMapName].Enable();
            else actionMaps[actionMapName].Disable();
        }
    }


// private IEnumerator TestToggle()
    // {
    //     Debug.Log("preparing to test ToggleActionMap true");
    //     yield return new WaitForSeconds(0.5f);
    //     foreach (InputActionMap x in actionMaps.Values)
    //     {
    //         Debug.Log($"{x.name} status is {x.enabled}");
    //         foreach (InputAction y in inputActions[x.name].Values)
    //         {
    //             Debug.Log($"{y.name} is {y.enabled}");
    //         }
    //     }
    //
    //     yield return null;
    //     Debug.Log($"\n\npreparing to test ToggleActionMap false");
    //     ToggleActionMap("FirstPerson", false);
    //     yield return new WaitForSeconds(2f);
    //             foreach (InputActionMap x in actionMaps.Values)
    //     {
    //         Debug.Log($"{x.name} status is {x.enabled}");
    //         foreach (InputAction y in inputActions[x.name].Values)
    //         {
    //             Debug.Log($"{y.name} is {y.enabled}");
    //         }
    //     }
    // }


    
    // private void OnEnable()
    // {
    //     SubscribeToEvents(true);
    // }
    //
    // private void OnDisable()
    // {
    //     SubscribeToEvents(false);
    // }
    //
    // private void SubscribeToEvents(bool state)
    // {
    //     if(state)
    //     {
    //         
    //
    //     }
    //     else
    //     {
    //         
    //     }
    // }
    //void Start() {}
    //void Update() {}
}
