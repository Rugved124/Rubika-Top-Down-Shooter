using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawnerManager : MonoBehaviour
{
    //this is the code of singleton
    public static WaveSpawnerManager instance;

    public List<WaveSpawner> waveSpawners;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        waveSpawners = FindObjectsOfType<WaveSpawner>().ToList();
    }

    public void ResetRemainingWaveSpaners()
    {
        foreach (WaveSpawner waveSpawner in waveSpawners)
        {
            if (waveSpawner != null)
            {
                if (!waveSpawner.isFinished)
                {
                    waveSpawner.ResetWaveSpawner();
                }
            }
        }
    }
}
