using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NannyBullet : MonoBehaviour
{
    [SerializeField]
    float bulletAngularSpeed;

    [SerializeField]
    float bulletTime;

    [SerializeField]
    int bulletDamage;


    [SerializeField]
    float bulletSpeed;

    Rigidbody rb;

    PC pc;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(FindObjectOfType<PC>() != null)
        {
            pc = FindObjectOfType<PC>();
        }
        
    }
    void Update()
    {
        Quaternion lookOnLook = Quaternion.LookRotation(pc.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * bulletAngularSpeed);
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        bulletTime -=Time.deltaTime;
        if(bulletTime <= 0f)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "Player")
            {
                pc.TakeDamage(bulletDamage);
                Die();
            }
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
