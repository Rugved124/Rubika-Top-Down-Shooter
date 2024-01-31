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

    [SerializeField]
    protected float bulletRate;

    [SerializeField]
    protected float bulletDamage;

    [SerializeField] 
    protected float bulletLifeTime;

    protected float spawnTime;

    public BulletTypes bulletType;

    public virtual void Start()
    {
        spawnTime = Time.time;
    }
    void Die()
    {
      Destroy(this.gameObject);
    }
}
