using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceExample : MonoBehaviour
{
    public GameObject prefab;
    public List<Transform> destinations = new List<Transform>();
    public float           speed;

    private int count;

    private bool    step = false;
    private Vector3 direction;

    private Transform currentTarget;


    private void OnEnable()
    {
        currentTarget = destinations[0];
    }

    void Update()
    {

        if (Mathf.Abs(Vector3.Distance(transform.position, currentTarget.position)) < 0.1f)
        {
            count++;
            if (count >= destinations.Count)
            {
                count = 0;
                GameObject _newDestination = Instantiate(prefab, GenerateNewDirection(), Quaternion.identity);
                destinations.Add(_newDestination.transform);
            }
            
            currentTarget = destinations[count];
        }
        
        direction     = currentTarget.position - transform.position ;
        transform.position += direction * speed * Time.deltaTime;
    }

    private Vector3 GenerateNewDirection()
    {
        return new Vector3(currentTarget.position.x + Random.Range(-10, 10), currentTarget.position.y + Random.Range(-10, 10),
            currentTarget.position.z + Random.Range(-10, 10));
    }
}
