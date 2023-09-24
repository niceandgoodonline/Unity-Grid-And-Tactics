using UnityEngine;
using UnityEngine.Pool;

public class UnitPathTilePool : MonoBehaviour
{
    public GameObject pathTilePrefab;
    public ObjectPool<GameObject> pool;
    private void OnEnable()
    {

        
        if (pool == null)
        {
            pool = new ObjectPool<GameObject>(() =>
            {
                return Instantiate(pathTilePrefab);
            },
            pathTile =>
            {
                pathTile.gameObject.SetActive(true);
            },
            pathTile =>
            {
                pathTile.gameObject.SetActive(false);
            },
            pathTile =>
            {
                Destroy(pathTile.gameObject);
            }, false, 10, 50);
        }
    }
}