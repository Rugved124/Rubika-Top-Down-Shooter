using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum WaveStates
    {
        GenerateEnemies,
        SpawnWave,
        Wait
    }

    public WaveStates currentWaveStates;

    public Wave[] waves;

    private Wave currentWave;

    [SerializeField]
    private Transform[] spawnPoints;

    private float timebetweenSpawns;
    private int i = 0;

    private bool stopSpawning = false;


    [SerializeField]
    private bool isSpawning = false;

    [SerializeField]
    int wavePurchasePower;

    [SerializeField]
    List<GameObject> generateEnemies = new List<GameObject>();
    private void Awake()
    {
        currentWave = waves[i];
        timebetweenSpawns = currentWave.timeBeforeThisWave;
        wavePurchasePower = currentWave.waveValue;
    }

    private void Update()
    {
        if (stopSpawning)
        {
            return;
        }

       switch (currentWaveStates)
        {
            case WaveStates.GenerateEnemies:
                GenerateEnemies(); 
                break;

            case WaveStates.SpawnWave:
                if (!isSpawning)
                {
                    isSpawning = true;
                    StartCoroutine(SpawnWave());
                }
                break;
            case WaveStates.Wait:

                if(Time.time >= timebetweenSpawns)
                {
                    ResetGeneratedEnemies();
                }
                break;
        }

    }

    IEnumerator SpawnWave()
    {
        NextWave();
        timebetweenSpawns = Time.time + currentWave.timeBeforeThisWave;

        for(int j =0; j < generateEnemies.Count; j++)
        {
            int selectedSpawnPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(generateEnemies[j], spawnPoints[selectedSpawnPoint].position, spawnPoints[selectedSpawnPoint].rotation);
            yield return new WaitForSeconds(0.1f);
        }
        ChangeWaveState(WaveStates.Wait);
        isSpawning = false;
    }
    void NextWave()
    {
        if(i + 1 < waves.Length)
        {
            i++;
            currentWave = waves[i];
        }
        else
        {
            stopSpawning = true;
        }
    }

    void GenerateEnemies()
    {
        

        int randomEnemyID = Random.Range(0, currentWave.enemy.Length);
        int randomEnemyCost = currentWave.enemy[randomEnemyID].enemyValue;


        if (wavePurchasePower - randomEnemyCost >= 0)
        {
            generateEnemies.Add(currentWave.enemy[randomEnemyID].enemiesInWave);
            wavePurchasePower -= randomEnemyCost;
        }
        else
        {
            for(int i = 0; i < currentWave.enemy.Length; i++)
            {
                if(wavePurchasePower <= currentWave.enemy[i].enemyValue)
                {
                    if(currentWaveStates != WaveStates.SpawnWave)
                    {
                        ChangeWaveState(WaveStates.SpawnWave);
                    }
                }
            }
        }
        
    }
    void ChangeWaveState(WaveStates state)
    {
        currentWaveStates = state;
    }

    void ResetGeneratedEnemies()
    {
        generateEnemies.Clear();
        wavePurchasePower = currentWave.waveValue;

        if(currentWaveStates != WaveStates.GenerateEnemies)
        {
            ChangeWaveState(WaveStates.GenerateEnemies);
        }
    }
}
