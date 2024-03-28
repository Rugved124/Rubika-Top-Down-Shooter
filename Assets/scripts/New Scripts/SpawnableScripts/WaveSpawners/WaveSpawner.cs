using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WaveSpawner : MonoBehaviour
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

    [SerializeField]
    List<GameObject> existingEnemies = new List<GameObject>();

    [SerializeField]
    List<GameObject> doors = new List<GameObject>();

    [SerializeField]
    List<GameObject> closingDoors = new List<GameObject>();

    [SerializeField]
    List<GameObject> openOnTrigger = new List<GameObject>();

    [SerializeField]
    GameObject sceneLoader;

    public bool canSpawm;

    public bool isFinished {  get; private set; }

    bool bellStarted;

    [SerializeField]
    AudioSource bell;

    [SerializeField]
    AudioSource doorSound;

    bool isCollided;
    private void Awake()
    {
        bellStarted = false;
        if(sceneLoader != null)
        {
            sceneLoader.SetActive(false);
        }
        isFinished = false;
        currentWave = waves[i];
        timebetweenSpawns = currentWave.timeBeforeThisWave;
        wavePurchasePower = currentWave.waveValue;
        isCollided = false;
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
                    if (Time.time >= timebetweenSpawns - 3 && Time.time < timebetweenSpawns && !bellStarted)
                    {
                        bellStarted = true;
                        if (bell != null)
                        {
                            bell.Play();
                        }
                    }
                    if (Time.time >= timebetweenSpawns || existingEnemies.Count == 0)
                    {
                        bellStarted = false;
                        if (bell != null)
                        {
                            bell.Stop();
                        }
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
                    if(existingEnemies.Count <= 0)
                    {
                        if(sceneLoader != null)
                        {
                            sceneLoader.SetActive(true);
                        }
                        if (!isFinished)
                        {
                            isFinished = true;
                            ChangeDoorState(false);
                            doorSound.Play();
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

        for(int j =0; j < generateEnemies.Count; j++)
        {
            int selectedSpawnPoint = Random.Range(0, spawnPoints.Length);

            existingEnemies.Add(Instantiate(generateEnemies[j], spawnPoints[selectedSpawnPoint].position, spawnPoints[selectedSpawnPoint].rotation));

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

    void ChangeDoorState(bool value)
    {
        foreach(GameObject door in doors)
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
            if(other.tag == "Player" && !isCollided)
            {
                isCollided = true;
                canSpawm = true;
                ChangeDoorState(true);
                foreach(GameObject go in openOnTrigger)
                {   
                    go.SetActive(false);
                }
            }
        }
    }
    
    public void ResetWaveSpawner()
    {
        canSpawm = false;
        foreach (GameObject enemies in existingEnemies)
        {
            if(enemies != null)
            {
                Destroy(enemies.gameObject);
            }
        }
        i = 0;
        if(sceneLoader != null)
        {
            sceneLoader.SetActive(false);
        }
        isFinished = false;
        currentWave = waves[i];
        timebetweenSpawns = currentWave.timeBeforeThisWave;
        wavePurchasePower = currentWave.waveValue;
        if(generateEnemies.Count > 0) 
        {
            generateEnemies.Clear();
        }
        if(existingEnemies.Count > 0)
        {
            existingEnemies.Clear();
        }
        foreach (GameObject door in closingDoors)
        {
            door.SetActive(false);
        }
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
        currentWaveStates = WaveStates.GenerateEnemies;
    }

    public void KillExistingEnemies()
    {
        foreach(GameObject i in existingEnemies)
        {
            Destroy(i);
        }
    }
}
