using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedWaveSpawner : MonoBehaviour
{
    public enum WaveStates
    {
        GenerateEnemies,
        SpawnWave,
        Wait,
        StopSpawn
    }

    public WaveStates currentWaveStates;

    public Wave[] waves;

    private Wave currentWave;

    [SerializeField]
    private Transform spawnPoint;

    private float timebetweenSpawns;
    private int i = 0;

    private bool stopSpawning = false;


    [SerializeField]
    private bool isSpawning = false;

    [SerializeField]
    List<GameObject> generateEnemies = new List<GameObject>();

    [SerializeField]
    List<GameObject> existingEnemies = new List<GameObject>();

    [SerializeField]
    List<GameObject> doors = new List<GameObject>();

    [SerializeField]
    List<GameObject> closingDoors = new List<GameObject>();

    public bool canSpawm;

    [SerializeField]
    FixedWaveTrigger trigger;

    bool isLoop;
    public bool isFinished { get; private set; }
    private void Start()
    {
        isFinished = false;
        canSpawm = true;
        if(waves.Length > 0)
        {
            currentWave = waves[i];
            timebetweenSpawns = currentWave.timeBeforeThisWave;
        }
    }

    private void Update()
    {
        existingEnemies.RemoveAll(enemies => enemies == null);
        if (canSpawm)
        {

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

                    if (Time.time >= timebetweenSpawns)
                    {
                        ResetGeneratedEnemies();

                        if (stopSpawning)
                        {
                            if (currentWaveStates != WaveStates.StopSpawn)
                            {
                                ChangeWaveState(WaveStates.StopSpawn);
                            }
                        }
                    }
                    break;
                case WaveStates.StopSpawn:
                    if (existingEnemies.Count <= 0)
                    {
                        if (isLoop)
                        {
                            isFinished = false;
                            canSpawm = true;
                            i = 0;
                            if (waves.Length > 0)
                            {
                                currentWave = waves[i];
                                timebetweenSpawns = currentWave.timeBeforeThisWave;
                            }
                            ChangeWaveState (WaveStates.GenerateEnemies);
                        }
                        else
                        {
                            ChangeDoorState(false);
                            if (!isFinished)
                            {
                                if (trigger != null)
                                {
                                    trigger.RemoveCount();
                                }

                                isFinished = true;
                            }
                        }
                    }

                    break;
            }
        }


    }

    IEnumerator SpawnWave()
    {
        NextWave();
        timebetweenSpawns = Time.time + currentWave.timeBeforeThisWave;

        for (int j = 0; j < generateEnemies.Count; j++)
        {
            existingEnemies.Add(Instantiate(generateEnemies[j], spawnPoint.position + new Vector3(Random.Range(-1,1), 0, Random.Range(-1, 1)), spawnPoint.rotation));
            yield return new WaitForSeconds(0.5f);
        }
        ChangeWaveState(WaveStates.Wait);
        isSpawning = false;
    }
    void NextWave()
    {
        if (i + 1 < waves.Length)
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

        Debug.Log("Generating Enemies");
        for (int i = 0; i < currentWave.enemy.Length; i++)
        {
            generateEnemies.Add(currentWave.enemy[i].enemiesInWave);
        }
        for (int i = 0; i < currentWave.enemy.Length; i++)
        {
            if (currentWaveStates != WaveStates.SpawnWave)
            {
                ChangeWaveState(WaveStates.SpawnWave);
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

        if (currentWaveStates != WaveStates.GenerateEnemies)
        {
            ChangeWaveState(WaveStates.GenerateEnemies);
        }
    }

    public void ChangeDoorState(bool value)
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(value);
        }
        foreach (GameObject door in closingDoors)
        {
            door.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "Player")
            {
                ChangeDoorState(true);
            }
        }
    }

    public void ResetWaveSpawner()
    {
        DestroyExistingEnemies();
        i = 0;
        isFinished = false;
        currentWave = waves[i];
        timebetweenSpawns = currentWave.timeBeforeThisWave;
    }

    public void DestroyExistingEnemies()
    {
        foreach (GameObject enemies in existingEnemies)
        {
            if (enemies != null)
            {
                Destroy(enemies.gameObject);
            }
        }
    }

    public void SetToLoop(bool loop)
    {
        isLoop = loop;
    }
}
