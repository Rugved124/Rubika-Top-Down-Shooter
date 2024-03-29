using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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

    bool canExplode;
    private void Awake()
    {
        canExplode = true;
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
    private void OnEnable()
    {
        StartCoroutine(camShake.ShakeCamera(8f, 4f, 3f));
    }
    private void Update()
    {
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
        if(canExplode)
        {
            Debug.Log("Dying");
            canExplode = false;
            Invoke("LoadCreditScene", 2f);
            if (waveSpawner != null)
            {
                foreach (FixedWaveSpawner spawner in waveSpawner.ReturnWaveSpawns())
                {
                    if (spawner != null)
                    {
                        spawner.DestroyExistingEnemies();
                        Destroy(spawner.gameObject);
                    }
                }
            }
            GameObject go = Instantiate(explosion, explosionPos.position, Quaternion.identity);
            Destroy(go, 1f);
            Destroy(waveSpawner.gameObject, 2.1f);
        }

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
    private void LoadCreditScene()
    {
        Debug.Log($"Loading Scene: {SceneManager.GetActiveScene().buildIndex + 1}");
        SceneManager.LoadScene(3);
    }
}
