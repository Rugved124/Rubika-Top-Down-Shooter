using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth instance;
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        health = maxHealth;
    }
    private void Update()
    {
        if (health <= 0)
        {
            EnemyDeath();
        }
    }
    public void TakeDamage(int bulletDamage)
    {
        health -= bulletDamage;
    }

    private void EnemyDeath()
    {
        Destroy(gameObject); 
    }
}    
