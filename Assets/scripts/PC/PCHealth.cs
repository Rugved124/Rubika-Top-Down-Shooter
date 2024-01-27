using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCHealth : PCStats
{
    public static PCHealth instance;
    [SerializeField]
    private int maxShieldPoints;
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
        maxHitPoints = 100;
        hitPoints = maxHitPoints;
    }
    void Update()
    {
        if (hitPoints <= 0)
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
    public void TakeDamage(int attackPoints)
    {
        if (shieldPoints <= 0)
        {
            hitPoints -= attackPoints;
        }
        else
        {
            shieldPoints -= 1;
        }
    }

    public void RespawnHealth()
    {
        hitPoints = maxHitPoints;
    }
    public int GetHitPoints()
    {
        return hitPoints;
    }

    public void SetShieldPoints(int shieldAdded)
    {
        if (shieldPoints < maxShieldPoints)
        {
            shieldPoints += shieldAdded;
        }
        if (shieldPoints > maxShieldPoints)
        {
            shieldPoints = maxShieldPoints;
        }
    }

}
