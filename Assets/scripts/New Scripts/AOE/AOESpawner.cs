using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> aoe;

    [SerializeField]
    List<float> timer;

    float currentTimer;

    private void Start()
    {
        foreach (GameObject a in aoe)
        {
            a.SetActive(false);
        }
        currentTimer = timer[Random.Range(0, timer.Count - 1)];
    }
    private void Update()
    {
        
    }
    private void ChooseRandomAOE()
    {
        int i = Random.Range(0, aoe.Count - 1);
    }

}
