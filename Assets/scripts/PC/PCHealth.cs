using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCHealth : MonoBehaviour
{
    public static PCHealth instance;
    public int hitPoints;
    public int maxHitPoints = 100;

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
        hitPoints = maxHitPoints;
    }
    void Update()
    {
        if(hitPoints <= 0)
        {
            PCDie.pcDie.isDead = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "EnemyBulletDefault")
        {
            DefaultBulletDamage bulletDamage = collision.gameObject.GetComponent<DefaultBulletDamage>(); ;
            if (bulletDamage != null)
            {
                Debug.Log("tookDamage");
                TakeDamage(bulletDamage.GetBulletDamage());
            }
        }
    }
    void TakeDamage(int attackPoints)
    {
        hitPoints -= attackPoints;  
    }
}
