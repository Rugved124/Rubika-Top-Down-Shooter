using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject spawnPrefab;

    public Collider[] colliders;
    Vector3 spawnPoint;
    public LayerMask spawnLayerMask;
    [SerializeField]
    float gap;
    void Start()
    {
        
        for (int i = 0; i < 5; i++)
        {
            SpawnSpheres();
        }
    }
    void SpawnSpheres()
    {
        int safetyNet = 0;
        bool canSpawn = false;
        while (!canSpawn)
        {
            float x = Random.Range(5f, -5f);
            float z = Random.Range(5f, -5f);
            spawnPoint = new Vector3(transform.position.x + x, 0,transform.position.z + z);
            canSpawn = PreventOverlapSpawn(spawnPoint);
            if (canSpawn)
            {
                break;
            }
            safetyNet++;
            if(safetyNet > 50)
            {
                Debug.Log("Too Many Attempts");
                break;
            }
        }
        Instantiate(spawnPrefab, spawnPoint, Quaternion.identity);

    }
    private bool PreventOverlapSpawn(Vector3 _spawnPoint)
    {
        colliders = Physics.OverlapSphere(transform.position, 5f, spawnLayerMask);
        for(int i = 0; i< colliders.Length;i++)
        {
            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x + gap;
            float hieght = colliders[i].bounds.extents.z + gap;

            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.z - hieght;
            float upperExtent = centerPoint.z + hieght;

            if (_spawnPoint.x >= leftExtent && _spawnPoint.x <= rightExtent)
            {
                if(_spawnPoint.z >= lowerExtent && _spawnPoint.z <= upperExtent) 
                {
                    return false;
                }
            }
        }
        return true;
    }
}
