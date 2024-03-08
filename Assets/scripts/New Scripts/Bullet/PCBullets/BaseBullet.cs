using System;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public enum bulletStatus
    {
        NONE,
        FIRE,
        POISON,
        SLOW
    }
    public enum BulletTypes
    {
        DEFAULTAMMO,
        FIRE,
        POISON,
        SLOW,
        FIREFIRE,
        POISONPOISON,
        SLOWSLOW,
        FIREPOISON,
        POISONFIRE,
        FIRESLOW,
        SLOWFIRE,
        SLOWPOISON,
        POISONSLOW
    }
    public bulletStatus status;

    [SerializeField]
    protected float bulletSpeed;

    public float bulletRate;

    [SerializeField]
    protected int bulletDamage;

    [SerializeField] 
    protected float bulletLifeTime;

    [SerializeField]
    public float bulletRange;

    public int ammoCount;
    protected float spawnTime;

    public BulletTypes bulletType;

    [SerializeField]
    protected PC pc;

    [SerializeField]
    protected Rigidbody bulletRB;

    protected bool isEnemy;

    protected bool isPC;

    Vector3 startPos;
    public virtual void Start()
    {
        startPos = transform.position;
        spawnTime = Time.time;
        pc = FindObjectOfType<PC>();
        bulletRB = GetComponent<Rigidbody>();
    }
    public virtual void Update()
    {
        if (Time.time - spawnTime > bulletLifeTime || Vector3.Distance(startPos,transform.position) >= bulletRange)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    public virtual void InitializeBullet()
    { 

    }

    public virtual void BulletMovement(Vector3 forceDirection)
    {

    }
    public virtual void OnTriggerEnter(Collider collision)
    {
        
        if (collision != null)
        {
            if(collision.tag != "Shield" && collision.tag != "Player" && collision.tag != "Enemies" && collision.tag != "Souls")
            {
                Die();
            }
            if (isPC)
            {
                if (collision.CompareTag("Shield"))
                {
                    if (collision.GetComponent<ShieldBehaviour>() != null && !collision.GetComponent<ShieldBehaviour>().isPC)
                    {
                        collision.GetComponent<ShieldBehaviour>().TakeDamage(1);
                        Die();
                    }
                }
                if (collision.CompareTag("Enemies"))
                {
                    if (collision.GetComponent<Enemy>() != null)
                    {
                        collision.GetComponent<Enemy>().TakeDamage(bulletDamage);
                        Die();
                    }
                }
            }
            if (isEnemy)
            {
                if (collision.CompareTag("Shield"))
                {
                    
                    if (collision.GetComponent<ShieldBehaviour>() != null && collision.GetComponent<ShieldBehaviour>().isPC)
                    {
                        collision.GetComponent<ShieldBehaviour>().TakeDamage(1);
                        Die();
                    }
                }
                if (collision.CompareTag("Player"))
                {
                    if (collision.GetComponent<PC>() != null)
                    {
                        collision.GetComponent<PC>().TakeDamage(bulletDamage);
                        Die();
                    }
                }
            }
            
            
        }

    }
    
    public void DidPCShotThis(bool yes)
    {
        isEnemy = !yes;
        isPC = yes;
    }

    public bool IsPCBullet()
    {
        return isPC;
    }
}

