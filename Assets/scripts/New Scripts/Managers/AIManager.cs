using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    PC pc;
    public List<Enemy> spawnedEnemies = new List<Enemy>();
    public bool hasAssignedPos;

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

        pc = FindObjectOfType<PC>();
    }

    private void Update()
    {
        spawnedEnemies.RemoveAll(enemies => enemies == null);
        if (spawnedEnemies.Count > 0)
        {

            GetPosition(pc.gameObject.transform);
        }
    }

    public void AddToList(Enemy enemy)
    {
        spawnedEnemies.Add(enemy);
    }
    public void RemoveFromList(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

    //public Vector3 GetPosition(Transform pc, float radiusAroundTarget, Enemy enemy)
    //{
    //    if(spawnedEnemies.Count > 0)
    //    {
    //        int i = spawnedEnemies.FindIndex(0, spawnedEnemies.Count, r => enemy);
    //        float x = pc.position.x + radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / spawnedEnemies.Count);
    //        float y = spawnedEnemies[i].gameObject.transform.position.y;
    //        float z = pc.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / spawnedEnemies.Count);
    //        Vector3 movePosition = new Vector3(x, y, z);
    //        return movePosition;
    //    }

    //    return Vector3.zero;
    //}

    public void GetPosition(Transform pc)
    {
        if (spawnedEnemies.Count > 0)
        {
            float radiusAroundTarget = 0f;
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                radiusAroundTarget = spawnedEnemies[i].enemyData.attackRange - 2f;
                float x = pc.position.x + radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / spawnedEnemies.Count);
                float y = spawnedEnemies[i].gameObject.transform.position.y;
                float z = pc.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / spawnedEnemies.Count);
                Vector3 movePosition = new Vector3(x, y, z);

                spawnedEnemies[i].surroundPos = movePosition;
            }
            
        }
    }
}
