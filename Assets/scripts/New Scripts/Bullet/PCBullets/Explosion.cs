using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    int explosionDamage;

    [SerializeField]
    float explosionRadius;

    [SerializeField]
    LayerMask enemies;

    [SerializeField]
    float maxTime;

    float startTime;
    bool exploded;
    void Start()
    {
        exploded = false;
        startTime = Time.time;
    }
    void Update()
    {
        if(Time.time - startTime >= maxTime)
        {
            Die();
        }

        if (Time.time - startTime >= maxTime/2 && !exploded)
        {
            exploded = true;
            Explode();
        }
    }

    public void ExplosionDamage(int damage)
    {
        explosionDamage = damage;
    }
    public void ExplosionRange(float range)
    {
        explosionRadius = range;
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        if (Physics.OverlapSphere(transform.position, explosionRadius, enemies) != null)
        {
            foreach (Collider c in Physics.OverlapSphere(transform.position, explosionRadius, enemies))
            {
                if (c.CompareTag("Player"))
                {
                    c.GetComponent<PC>().TakeDamage(explosionDamage / 2);
                }
                if (c.CompareTag("Enemies"))
                {
                    c.GetComponent<Enemy>().TakeDamage(explosionDamage);
                }
            }
        }
    }
}
