using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> aoe;

    [SerializeField]
    List<float> timer;

    //[SerializeField]
    float currentTimer;

    //[SerializeField]
    GameObject currentAOE;
    float rngDespawnTime;

    bool canDespawn;

    [SerializeField]
    float minDespawnTime, maxDespawnTime;
    private void Start()
    {
        rngDespawnTime = 0;
        foreach (GameObject a in aoe)
        {
            a.SetActive(false);
        }
        SetCurrentTimer();
    }
    private void Update()
    {
        if (currentAOE == null)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0)
            {
                ChooseRandomAOE();
                SetCurrentTimer();
            }
        }
        if (currentAOE != null)
        {
            if (!canDespawn)
            {
                rngDespawnTime = Random.Range(minDespawnTime, maxDespawnTime);
                canDespawn = true;
            }
            else
            {
                rngDespawnTime -= Time.deltaTime;
            }
            if(rngDespawnTime <= 0f)
            {
                currentAOE.SetActive(false);
                currentAOE = null;
            }
        }
    }
    private void ChooseRandomAOE()
    {
        Debug.Log("Spawning");
        int i = Random.Range(0, aoe.Count - 1);
        aoe[i].SetActive(true);
        currentAOE = aoe[i];
        canDespawn = false;
    }
    private void SetCurrentTimer()
    {
        currentTimer = timer[Random.Range(0, timer.Count - 1)];
    }

}
