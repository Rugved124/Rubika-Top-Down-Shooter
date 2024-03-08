using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FixedWaveTrigger : MonoBehaviour
{
    [SerializeField]
    List<FixedWaveSpawner> spawns;


    private void Awake()
    {
        foreach (var spawner in spawns)
        {
            spawner.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var spawner in spawns)
            {
                spawner.gameObject.SetActive(true);
                spawner.ChangeDoorState(true);
            }
        }
    }
}
