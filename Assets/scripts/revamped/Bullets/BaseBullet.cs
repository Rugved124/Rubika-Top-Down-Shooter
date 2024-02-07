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
    protected int bulletDamage;

    [SerializeField] 
    protected float bulletLifeTime;

    
    protected float spawnTime;

    public BulletTypes bulletType;

    [SerializeField]
    protected PC pc;

    [SerializeField]
    protected Rigidbody bulletRB;
    public virtual void Start()
    {
        spawnTime = Time.time;
        pc = FindObjectOfType<PC>();
        bulletRB = GetComponent<Rigidbody>();

        BulletMovement();

    }
    public virtual void Update()
    {
        if (Time.time - spawnTime > bulletLifeTime)
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

    public virtual void BulletMovement()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.collider.CompareTag("Enemies"))
            {
                if(collision.collider.GetComponent<Enemy>() != null)
                {
                    collision.collider.GetComponent<Enemy>().TakeDamage(bulletDamage);
                }
            }
            Die();
        }
    }
    
}

