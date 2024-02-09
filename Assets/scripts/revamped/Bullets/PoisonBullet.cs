using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletLifeTime;

    [SerializeField]
    private int bulletDamage;

    [SerializeField]
    private GameObject PoisonAOE;   
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
        if (other != null && other.tag != "Poison")
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<PC>() != null)
                {
                    other.GetComponent<PC>().TakeDamage(bulletDamage);
                }
            }
            else if (other.tag != "Player" && other.tag != "Ground")
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit))
                {
                    if (hit.collider != null)
                    {
                        Instantiate(PoisonAOE, hit.point, Quaternion.identity);
                    }
                }
                
            }
            Die();
        }
    }
}

