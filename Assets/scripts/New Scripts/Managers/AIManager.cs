using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;

    public float radiusAroundTarget;
    PC pc;
    public List<Enemy> spawnedEnemies = new List<Enemy>();
    private void Awake()
    {
        if(instance  == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        foreach(Enemy e in)
        {

        }
    }

    public void AddToList(Enemy enemy)
    {
        spawnedEnemies.Add(enemy);
    }

    public Vector3 GetPosition(Transform pc)
    {
        for(int i = 0; i < spawnedEnemies.Count; i++)
        {
            float x = pc.position.x + radiusAroundTarget * Mathf.Cos(2* Mathf.PI * i/spawnedEnemies.Count);
            float y = spawnedEnemies[i].gameObject.transform.position.y;
            float z = pc.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / spawnedEnemies.Count);

            Vector3 movePosition = new Vector3(x, y, z);
            return movePosition;
        }
        return Vector3.zero;
    }
}
