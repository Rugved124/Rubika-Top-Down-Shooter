using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossHealth : MonoBehaviour
{
    int i;
    public VirtualCameraShake camShake;
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
    Slider fullHealthSlider;

    [SerializeField]
    Image barColor;

    [SerializeField]
    TextMeshProUGUI bulletType;

    [SerializeField]
    int totalHealth;

    [SerializeField]
    GameObject explosion;

    [SerializeField]
    Transform explosionPos;
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
        if(fullHealthSlider != null)
        {
            foreach(int a in healthPerBar)
            {
                totalHealth += a;
            }
            
            fullHealthSlider.maxValue = totalHealth;
            fullHealthSlider.value = totalHealth;
        }
    }

    private void OnDestroy()
    {
        if(explosion != null && explosionPos != null)
        {
            GameObject explode = Instantiate(explosion, explosionPos.position, Quaternion.identity);
            Destroy(explode, 1.5f);
        }

    }
    private void OnEnable()
    {
        StartCoroutine(camShake.ShakeCamera(8f, 4f, 3f));
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
                    barColor.color = new Vector4(0.78f, 0.35f, 1, 1f);
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
        if(fullHealthSlider != null)
        {
            fullHealthSlider.value = totalHealth;
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
                        TakeDamage();
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

    public string GetCurrentBulletType()
    {
        return currentBulletType;
    }

    public void TakeDamage()
    {
        currentHealth--;
        totalHealth--;
    }
}
