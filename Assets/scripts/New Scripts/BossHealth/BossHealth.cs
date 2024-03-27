using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealth : MonoBehaviour
{
    int i;
    [SerializeField]
    List<string> bulletTypes = new List<string> { "SLOW", "FIRE", "POISON" };

    [SerializeField]
    string currentBulletType;

    [SerializeField]
    int currentHealth;

    [SerializeField]
    List<int> healthPerBar = new List<int>();

    [SerializeField]
    FixedWaveTrigger waveSpawner;

    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    Image barColor;
    private void Awake()
    {
        ResetHealth(0);
        if(healthSlider == null)
        {
            Debug.LogError("HealthBar Slider is not attached");
        }
        if(barColor == null)
        {
            Debug.LogError("HealthBar Image is not attached");
        }
        if(healthSlider != null)
        {
            healthSlider.maxValue = currentHealth;
            healthSlider.value = currentHealth;
        }
    }
    private void Update()
    {
        if(waveSpawner == null)
        {
            Debug.LogError("There is no waveSpawner attached to the boss");
        }
        if(barColor != null) 
        {
            switch (currentBulletType)
            {
                case "SLOW":
                    barColor.color = new Vector4(0.38f, 0.26f, 0.78f, 1f);
                    break;
                case "POISON":
                    barColor.color = new Vector4(0.47f, 0.723f, 0, 1);
                    break;
                case "FIRE":
                    barColor.color = new Vector4(0.35f, 0.7f, 1, 1);
                    break;
            }
        }
        if(healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }    
        if(currentHealth <= 0 )
        {
            if( i + 1 < bulletTypes.Count)
            {
                Debug.Log(i);
                ResetHealth(i + 1);
            }
            else
            {
                Die();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if (other.CompareTag("Bullets"))
            {
                if(other.gameObject.GetComponent<BaseBullet>() != null)
                {
                    if (other.gameObject.GetComponent<BaseBullet>().bulletType.ToString().Contains(currentBulletType))
                    {
                        currentHealth--;
                    }
                }
            }
        }
    }

    private void ResetHealth(int newBullet)
    {
        i = newBullet;
        currentHealth = healthPerBar[i]; 
        currentBulletType = bulletTypes[i];
        if(healthSlider != null)
        {
            healthSlider.maxValue = currentHealth;
        }
    }

    private void Die()
    {
        if(waveSpawner != null)
        {
            foreach(FixedWaveSpawner spawner in waveSpawner.ReturnWaveSpawns())
            {
                spawner.DestroyExistingEnemies();
                Destroy(spawner.gameObject);
            }
            Destroy(waveSpawner.gameObject);
        }
        Destroy(gameObject);
    }
}
