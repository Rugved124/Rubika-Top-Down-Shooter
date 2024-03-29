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

        if (Time.time - startTime >= 0.2 && !exploded)
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
                RaycastHit hit;
                if(Physics.Raycast(transform.position, c.transform.position - transform.position, out hit))
                {
                    Debug.DrawRay(transform.position, c.transform.position - transform.position, Color.black);
                    if (hit.collider != null)
                    {
                        
                        if(hit.collider.CompareTag("Player"))
                        {
                            if(c.GetComponent<PC>() != null)
                            {
                                if (c.GetComponent<PC>().isDead && c.GetComponent<PC>() != null)
                                {
                                    c.GetComponent<PC>().TakeDamage(explosionDamage / 2);
                                }
                            }
                        }
                        if (hit.collider.CompareTag("Enemies"))
                        {
                            if(c.GetComponent<Enemy>() != null)
                            {
                                c.GetComponent<Enemy>().TakeDamage(explosionDamage);
                            }
                        }
                    }
                }

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
