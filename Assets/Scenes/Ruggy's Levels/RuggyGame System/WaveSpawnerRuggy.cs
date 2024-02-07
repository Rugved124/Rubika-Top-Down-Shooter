using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerRuggy : MonoBehaviour
{
	public enum WaveStates
	{
		GenerateEnemies,
		SpawnWave,
		Wait
	}

	public WaveStates currentWaveState;

	public WaveRuggy[] waves;

	private WaveRuggy currentWave;

	[SerializeField] private Transform[] spawnPoints;

	private float timeBetweenSpawns;
	private int i = 0;

	private bool stopSpawning = false;

	[SerializeField] private bool isSpawning = false;

	[SerializeField] int wavePurchasePower;

	[SerializeField] List<GameObject> generatedEnemies = new List<GameObject>();

	private void Awake()
	{
		currentWave = waves[i];
		timeBetweenSpawns = currentWave.timeBeforeThisWave;
		wavePurchasePower = currentWave.waveValue;

	}

	private void Update()
	{
		if (stopSpawning)
		{
			return;
		}

		switch (currentWaveState)
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

				if (Time.time > timeBetweenSpawns)
				{
					ResetGeneratedEnemies();
				}

				break;
		}
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
		int randEnemyID = Random.Range(0, currentWave.enemy.Length);
		int randEnemyCost = currentWave.enemy[randEnemyID].enemyValue;

		if (wavePurchasePower - randEnemyCost >= 0)
		{
			generatedEnemies.Add(currentWave.enemy[randEnemyID].enemiesInWave);
			wavePurchasePower -= randEnemyCost;
		}
		else
		{
			for (int i = 0; i < currentWave.enemy.Length; i++)
			{
				if (wavePurchasePower <= currentWave.enemy[i].enemyValue)
				{
					if (currentWaveState != WaveStates.SpawnWave)
					{
						ChangeWaveState(WaveStates.SpawnWave);
					}
				}
			}
		}
	}
	void ChangeWaveState(WaveStates state)
	{
		currentWaveState = state;
	}

	IEnumerator SpawnWave()
	{
		NextWave();
		timeBetweenSpawns = Time.time + currentWave.timeBeforeThisWave;

		for (int j = 0; j < generatedEnemies.Count; j++)
		{
			int selectedSpawnPoint = Random.Range(0, spawnPoints.Length);

			Instantiate(generatedEnemies[j], spawnPoints[selectedSpawnPoint].position, spawnPoints[selectedSpawnPoint].rotation);

			yield return new WaitForSeconds(0.1f);
		}

		ChangeWaveState(WaveStates.Wait);
		isSpawning = false;
	}

	private void ResetGeneratedEnemies()
	{
		generatedEnemies.Clear();
		wavePurchasePower = currentWave.waveValue;

		if (currentWaveState != WaveStates.GenerateEnemies)
		{
			ChangeWaveState(WaveStates.GenerateEnemies);
		}
	}


}
