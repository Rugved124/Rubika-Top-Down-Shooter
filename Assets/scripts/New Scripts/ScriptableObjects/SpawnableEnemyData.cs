using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawnables", menuName = "ScriptableObjects/SpawnableData")]
public class SpawnableEnemyData : ScriptableObject
{
    public GameObject enemiesInWave;

    public int enemyValue;
}
