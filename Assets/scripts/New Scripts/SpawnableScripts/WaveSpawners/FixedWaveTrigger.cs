using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FixedWaveTrigger : MonoBehaviour
{
    [SerializeField]
    List<FixedWaveSpawner> spawns;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLayer"))
        {
            foreach (var spawner in spawns)
            {
                spawner.gameObject.SetActive(true);
                spawner.ChangeDoorState(true);
            }
        }
    }
}
