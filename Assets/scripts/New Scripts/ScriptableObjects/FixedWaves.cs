using UnityEngine;

[CreateAssetMenu(fileName = "WaveList", menuName = "ScriptableObjects/WaveData")]
public class FixedWaves : ScriptableObject
{
    public Wave[] waveList;

    public int wavePos;
}
