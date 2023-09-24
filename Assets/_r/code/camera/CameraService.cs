using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraService : MonoBehaviour
{
    public bool   debug;

    public float snapToTopThreshold = 0.67f;
    public Camera    mainCam;
    public LayerMask terrainLayer;

    public Transform cursor;

    private Vector3 cursorOffset; 

    private Vector2     lookInput;

    private RaycastHit hit, tile;

    private GridInfo grid;
    
    private Vector3 dummy = new Vector3(-255f, -255f, -255f);
    private Vector3 sideHit, lookOffset, updateVector3;
    
    private Vector3 nudge;
    private Vector3 nudgeNegative = new Vector3(-0.01f, 0f, -0.01f);
    private Vector3 nudgePositive = new Vector3(0.01f, 0f, 0.01f);
    
    private Vector3 northHit = new Vector3(0f, 1f, 0.5f);
    private Vector3 eastHit  = new Vector3(0.5f, 1f, 0f);
    private Vector3 southHit = new Vector3(0f, 1f, -0.5f);
    private Vector3 westHit  = new Vector3(-0.5f, 1f, 0f);

    public UnitBase _unit;
    
    public delegate void noneDelegate();
    public delegate void unitDelegate(UnitBase u);

    public static event noneDelegate EmitNoUnit;
    public void __EmitNoUnit()
    {
        if (EmitNoUnit != null) EmitNoUnit();
    }
    
    public static event noneDelegate EmitNoUnitMenu;
    public void __EmitNoUnitMenu()
    {
        if (EmitNoUnitMenu != null) EmitNoUnitMenu();
    }
    
    public static event unitDelegate EmitUnitPreview;
    public void __EmitUnitPreview(UnitBase _u)
    {
        if (EmitUnitPreview != null) EmitUnitPreview(_u);
    }
    
    public static event unitDelegate EmitUnitSelect;
    public void __EmitUnitSelect(UnitBase _u)
    {
        if (EmitUnitSelect != null) EmitUnitSelect(_u);
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

    public void HandlePrimary(InputAction.CallbackContext cx)
    {
        if (cx.performed)
        {
            if (_unit != null) __EmitUnitSelect(_unit);
        }
    }

    public void __init()
    {
        cursorOffset = new Vector3(0f, cursor.localScale.z, 0f);
        if (grid == null) StartCoroutine(AwaitDependencies());
    }

    private IEnumerator AwaitDependencies()
    {
        while (grid == null)
        {
            yield return null;
            grid = GridInfo.instance;
        }

        UpdateAllTileHeights();
    }

    public void UpdateAllTileHeights()
    {
        int x = (int)grid.gridSize.x;
        int z = (int)grid.gridSize.y;

        for (int i = 1; i <= z; i++)
        {
            for (int j = 1; j <= x ; j++)
            {
                tile          = FindTerrain(new Vector3(i + 0.1f, 100f, j + 0.1f), Vector3.down);
                updateVector3 = new Vector3(i, tile.point.y, j);
                grid.UpdateTileHeight(updateVector3);
            }
        }

        if (debug) DebugGridHeights();
    }

    private void DebugGridHeights()
    {
        int x = (int)grid.gridSize.x;
        int z = (int)grid.gridSize.y;
        for (int i = 1; i <= x; i++)
        {
            for (int j = 1; j <= z; j++)
            {
                Vector3 lookUp = grid.ClosestTile(new Vector3(i, 1, j));
                if (lookUp != dummy) Debug.Log(lookUp.y);
            }
        }
    }

    public void UpdateTileHeights(Vector3[] tilesToUpdate)
    {
        foreach (Vector3 tile in tilesToUpdate)
        {
            grid.UpdateTileHeight(tile);
        }
    }

    public void FreeMoveCursor()
    {
        Ray        ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit = FindTerrain(ray.origin, ray.direction);
        try { QueryGrid(hit); }
        catch 
        {
            if (debug)
            {
                Debug.Log($"did not find terrain, hit: {hit.point}");
            }
        }
    }
    
    public void SelectingMoveCursor()
    {
        Debug.Log("CameraService. SelectingMoveCursor called: TODO?");
    }

    public void SelectingPrimary()
    {
        if (_unit != null) __EmitUnitSelect(_unit);
    }

    private RaycastHit FindTerrain(Vector3 origin, Vector3 direction)
    {
        Physics.Raycast(origin, direction, out hit, Mathf.Infinity, 
            terrainLayer, QueryTriggerInteraction.Collide);
        return hit;
    }

    private void QueryGrid(RaycastHit hit)
    {
        bool    sideWasHit = CheckIfSideHit(hit);
        Vector3 lookUp     = grid.ClosestTile(hit.point + nudge);
        
        if (sideWasHit)
        {
            Vector3    nearTile = hit.point + sideHit;
            RaycastHit hitNear  = FindTerrain(nearTile, Vector3.down);
            Vector3    lookNear = grid.ClosestTile(hitNear.point + nudge);
            
            if (hit.point.y - lookNear.y > (lookUp.y - lookNear.y) * snapToTopThreshold) SetCursorPosition(lookUp + cursorOffset);
            else if (lookNear != dummy)  SetCursorPosition(lookNear + cursorOffset);
            else SetCursorPosition(lookUp + cursorOffset);
        }
        else if (lookUp != dummy) SetCursorPosition(lookUp + cursorOffset);
    }

    
    
    private bool CheckIfSideHit(RaycastHit hit)
    {

        int diffX = (int)((hit.point.x * 10) - (hit.transform.position.x * 10));
        int diffZ = (int)((hit.point.z * 10) - (hit.transform.position.z * 10));
        
        if (Mathf.Abs(diffX) == 5) 
        {
            if (diffX > 0)
            {
                nudge   = nudgeNegative;
                sideHit = eastHit;
            }
            else
            {
                nudge   = nudgePositive;
                sideHit = westHit;
            }
            return true;
        }
        else if (Mathf.Abs(diffZ) == 5)
        {
            if (diffZ > 0)
            {
                nudge    = nudgeNegative;
                sideHit  = northHit;
            }
            else
            {
                nudge    = nudgePositive;
                sideHit  = southHit;
            }
            return true;
        }
        return false;
    }

    private void SetCursorPosition(Vector3 newPosition)
    {
        cursor.position = newPosition;
        _unit           = grid.CheckUnitOccupant(newPosition);
        if (_unit != null)
        {
            __EmitUnitPreview(_unit);
        }
        else __EmitNoUnit();
    }
}
