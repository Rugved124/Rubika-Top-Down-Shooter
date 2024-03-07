using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotgunPellets : BaseBullet
{
    [SerializeField]
    int explosionDamage = 25;

    [SerializeField]
    GameObject explosionVFX;

    [SerializeField]
    GameObject explosion;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base .Update();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        if (collision.tag != "Player" && collision.tag != "Ground" && collision.tag != "Shield" && collision.tag != "Enemies")
        {
            Die();
        }
    }
    public void SetBaseStats(BulletTypes bullet, float _bulletLifeTime, int _damage)
    {
        bulletLifeTime = _bulletLifeTime;
        bulletDamage = _damage;
        bulletType = bullet;
    }
    public override void Die()
    {
        base.Die();
        explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        explosion.GetComponent<Explosion>().ExplosionDamage(explosionDamage);
    }
}
