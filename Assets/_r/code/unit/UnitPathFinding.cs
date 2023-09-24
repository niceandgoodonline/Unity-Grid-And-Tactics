using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathFinding : MonoBehaviour
{
    public GameObject       pathTiles;
    public UnitPathTilePool pool;
    
    public void HandleAbilityPreview(AbilityData ability, HashSet<TileData> tiles)
    {
        if (ability.type == AbilityType.Movement) MovementPreview(ability, tiles);
        else ActivePreview(ability, tiles);
    }

    public void MovementPreview(AbilityData ability, HashSet<TileData> tiles)
    {
        foreach (TileData tile in tiles)
        {
            GameObject _x = pool.pool.Get();
            _x.transform.parent   = pathTiles.transform;
            _x.transform.position = tile.worldPosition;
        }
    }
    
    public void ActivePreview(AbilityData ability,  HashSet<TileData> tiles)
    {
        
    }
}
