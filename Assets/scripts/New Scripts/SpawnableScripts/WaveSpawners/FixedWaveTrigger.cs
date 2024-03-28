using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FixedWaveTrigger : MonoBehaviour
{
    [SerializeField]
    List<FixedWaveSpawner> spawns;

    [SerializeField]
    GameObject enabledAfterWaves;

    int waveCount;

    [SerializeField]
    bool makeItLoop;

    bool canChange;
    private void Awake()
    {
        canChange = true;
        if(enabledAfterWaves != null)
        {
            enabledAfterWaves.SetActive(false);
        }
        foreach (var spawner in spawns)
        {
            spawner.gameObject.SetActive(false);
            waveCount++;
        }
    }
    private void Update()
    {
        if(waveCount <= 0)
        {
            if (enabledAfterWaves != null)
            {
                enabledAfterWaves.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var spawner in spawns)
            {
                if (canChange)
                {
                    canChange = false;
                    spawner.gameObject.SetActive(true);
                    spawner.SetToLoop(makeItLoop);
                    spawner.ChangeDoorState(true);
                }

            }
        }
    }

    public List<FixedWaveSpawner> ReturnWaveSpawns()
    {
        return spawns;
    }

    public void RemoveCount()
    {
        waveCount--;
    }
}
