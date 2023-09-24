using System.Collections.Generic;
using UnityEngine;

public enum TileTerrain
{
    Road,
    Dirt,
    Mud,
    Grass,
    Hill,
    Water,
    Blocked
}

public struct TileData
{
    public TileTerrain terrain;
    public Vector3     worldPosition;

    public TileData(TileTerrain _terrain, Vector3 _worldPosition)
    {
        terrain       = _terrain;
        worldPosition = _worldPosition;
    }
}

public class Grid
{
    private int          width;
    private int          height;
    private float        cellSize;
    
    public Dictionary<int, Dictionary<int, TileData>> tileData = new Dictionary<int, Dictionary<int, TileData>>();

    public Grid(int width, int height, float cellSize)
    {
        this.width    = width;
        this.height   = height;
        this.cellSize = cellSize;

        for (int x = 1; x <= width; x++)
        {
            tileData.Add(x, new Dictionary<int, TileData>());
            for (int z = 1; z <= height; z++)
            {
                tileData[x].Add(z, new TileData(TileTerrain.Dirt, GetWorldPosition(x, z)));
            }
        }
    }

    public void UpdateTileHeight(Vector3 tileToUpdate)
    {
        int x = (int)tileToUpdate.x;
        int z = (int)tileToUpdate.z;
        
        tileData[x][z] = new TileData(tileData[x][z].terrain, tileToUpdate);
    }
    
    public void UpdateTileTerrain(Vector3 tileToUpdate, TileTerrain newTerrain)
    {
        int x = (int)tileToUpdate.x;
        int z = (int)tileToUpdate.z;
        tileData[x][z] = new TileData(newTerrain, tileToUpdate);
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize;
    }
}
