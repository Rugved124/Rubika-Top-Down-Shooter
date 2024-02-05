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
        FIREPOISON,
        FIRESLOW,
        POISONSLOW
    }
    public bulletStatus status;

    [SerializeField]
    protected float bulletSpeed;

    public float bulletRate;

    [SerializeField]
    protected float bulletDamage;

    [SerializeField] 
    protected float bulletLifeTime;

    [SerializeField]
    protected float spawnTime;

    public BulletTypes bulletType;

    protected PC pc;

    protected Rigidbody bulletRB;
    public virtual void Start()
    {
        spawnTime = Time.time;
        pc = FindObjectOfType<PC>();
    }
    public virtual void Update()
    {
        BulletMovement();
    }
    public virtual void Die()
    {
      Destroy(this.gameObject);
    }
    public virtual void InitializeBullet()
    { 

    }

    public virtual void BulletMovement()
    {

    }
    
}

