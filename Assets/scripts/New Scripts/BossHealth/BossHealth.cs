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
    WaveSpawner waveSpawner;

    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    Image barColor;
    private void Awake()
    {
        ResetHealth(0);
        healthSlider.maxValue = currentHealth;
        healthSlider.value = currentHealth;
    }
    private void Update()
    {
        switch (currentBulletType)
        {
            case "SLOW":
                barColor.color = Color.blue;
                break;
            case "POISON":
                barColor.color = Color.green;
                break;
            case "FIRE":
                barColor.color = Color.red;
                break;
        }
            
        healthSlider.value = currentHealth;
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
        healthSlider.maxValue = currentHealth;
    }

    private void Die()
    {
        waveSpawner.KillExistingEnemies();
        Destroy(waveSpawner.gameObject);
        Destroy(gameObject);
    }
}
