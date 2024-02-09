using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletLifeTime;

    [SerializeField]
    private int bulletDamage;
    public void Start()
    {

    }
    public void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0f)
        {
            Die();
        }

    }
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag != "Bullets" && other.tag != "Enemies")
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<PC>() != null)
                {
                     other.GetComponent<PC>().TakeDamage(bulletDamage);
                }
            }
            Die();
        }
    }
}
