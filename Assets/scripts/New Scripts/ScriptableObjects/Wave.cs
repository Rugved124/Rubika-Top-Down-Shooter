using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/WaveData")]
public class Wave : ScriptableObject
{
    public SpawnableEnemyData[] enemy;

    public float timeBeforeThisWave;

    public int waveValue;

}
