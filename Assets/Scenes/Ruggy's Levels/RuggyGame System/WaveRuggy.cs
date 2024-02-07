using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave" , menuName = "ScriptableObjects/WaveData")]
public class WaveRuggy : ScriptableObject
{
	public SpawnableEnemyRuggy[] enemy;

	public float timeBeforeThisWave;

	public int waveValue;

}
