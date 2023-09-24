using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridOccupants
{
    public Dictionary<int, Dictionary<int, UnitBase>> unit = new Dictionary<int, Dictionary<int, UnitBase>>();
}

public class GridInfo : MonoBehaviour
{
    public static GridInfo instance;

    public bool       debugGrid, debugTileRequest;
    public GameObject debugPrefab;
    
    public Grid         grid;
    public Vector2      gridSize;
    public GridOccupants occupants = new GridOccupants();

    private Vector3           dummyVector3  = new Vector3(-255f, -255f, -255f);
    private int[]             cardinals     = new int[2]{1, -1};
    private HashSet<TileData> filteredTiles = new HashSet<TileData>();
    public void __init()
    {
        InitSingleton();
        grid = new Grid((int)gridSize.x, (int)gridSize.y, 1f);
        if (debugGrid) DisplayDebugSpheres();
    }

    private void DisplayDebugSpheres()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                SpawnDebugPrefab(new Vector3(x, 1f, y), Vector3.zero);
            }
        }
    }
    
    private void InitSingleton()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
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
        if (state)
        {
            UnitInfo.EmitUnitOccupant    += RegisterUnitOccupant;
            UnitInfo.EmitTileDataRequest += RequestTilesInRange;
        }
        else
        {
            UnitInfo.EmitUnitOccupant    -= RegisterUnitOccupant;
            UnitInfo.EmitTileDataRequest -= RequestTilesInRange;

        }
    }

    public Vector3 ClosestTile(Vector3 point)
    {
        Vector3 lookUp = dummyVector3; 
        try
        {
            lookUp = grid.tileData[(int)Mathf.Round(point.x)][(int)Mathf.Round(point.z)].worldPosition;
        }
        catch { };
        return lookUp;
    }

    public void UpdateTileHeight(Vector3 tileToUpdate)
    {
        grid.UpdateTileHeight(tileToUpdate);
    }
    
    public void UpdateTileTerrain(Vector3 tileToUpdate,TileTerrain newTerrain)
    {
        grid.UpdateTileTerrain(tileToUpdate, newTerrain);
    }

    public UnitBase CheckUnitOccupant(Vector3 tileToCheck)
    {
        int _x = (int)Mathf.Floor(tileToCheck.x);
        int _z = (int)Mathf.Floor(tileToCheck.z);
        if (occupants.unit.ContainsKey(_x) && occupants.unit[_x].ContainsKey(_z))
        {
            return occupants.unit[_x][_z];
        }

        return null;
    }

    public void RegisterUnitOccupant(UnitBase occupantToUpdate)
    {
        int _x = (int)Mathf.Floor(occupantToUpdate.transform.position.x);
        int _z = (int)Mathf.Floor(occupantToUpdate.transform.position.z);
        occupants.unit.Add(_x, new Dictionary<int, UnitBase>());
        occupants.unit[_x][_z] = occupantToUpdate;
    }

    public void UpdateUnitOccupant(UnitBase occupantToUpdate, Vector3 tileToCheck)
    {
        int _x = (int)Mathf.Floor(tileToCheck.x);
        int _z = (int)Mathf.Floor(tileToCheck.z);
        if (occupants.unit[_x][_z] != occupantToUpdate)
        {
            occupants.unit[_x][_z] = occupantToUpdate;
        }
    }

    public void RequestTilesInRange(UnitBase unit, AbilityData data)
    {
        filteredTiles.Clear();
        Vector3  origin = ClosestTile(unit.transform.position);
        TileData tileOverride;

        int[] xRange = Enumerable.Range(data.horizontalRange * -1, data.horizontalRange * 2 + 1).ToArray();
        int[] yRange;
        
        List<int[]> toCheck = new List<int[]>();

        for (int i = 0; i < xRange.Length; i++)
        {
            int _thisX = xRange[i];
            int _thisY = (Mathf.Abs(xRange[i]) * -1) + data.horizontalRange;
            if (_thisY > 0)
            {
                yRange = Enumerable.Range(_thisY * -1, _thisY * 2 + 1).ToArray();
                for (int j = 0; j < yRange.Length; j++) toCheck.Add(new int[]{_thisX, yRange[j]});
            }
            else toCheck.Add(new int[]{_thisX, _thisY});
        }

        foreach (int[] pos in toCheck)
        {
            try
            {
                tileOverride = grid.tileData[pos[0] + (int)origin.x][pos[1] + (int)origin.z];
                if(CheckTileFitness(tileOverride, origin, data)) filteredTiles.Add(tileOverride);
            }
            catch {}
        }

        unit.SetTileData(filteredTiles, data);
    }

    private bool CheckTileFitness(TileData tile, Vector3 origin, AbilityData data)
    {
        if (Mathf.Abs(tile.worldPosition.y - origin.y) < data.verticalRange)
            if (data.type == AbilityType.Movement)
            {
                if (CheckUnitOccupant(tile.worldPosition) == null) return true;
            }
            else return true;
        return false;
    }

    private void SpawnDebugPrefab(Vector3 _p, Vector3 _o)
    {
        Instantiate(debugPrefab, _p + _o, Quaternion.identity);
    }
}
