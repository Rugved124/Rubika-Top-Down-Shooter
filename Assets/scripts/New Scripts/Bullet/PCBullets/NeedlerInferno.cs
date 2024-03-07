using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeedlerInferno : BaseBullet
{
    [SerializeField]
    private float visionConeAngle = 90f;

    [SerializeField]
    float detectionRange;

    [SerializeField]
    int rayCastNumber;

    [SerializeField]
    GameObject enemy;

    [SerializeField]
    GameObject explosion;

    [SerializeField]
    float explosionTime;

    Vector3 _startPos;

    [SerializeField]
    LayerMask enemies;
    private Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    private Quaternion stepAngle = Quaternion.AngleAxis(0, Vector3.up);

    Vector3 enemyLocation;

    bool hitEnemy;
    bool tracking;
    bool hitThings;

    float x, y, z;
    public override void Start()
    {
        hitEnemy = false;
        hitThings = false;
        tracking = false;
        _startPos = transform.position;
        startingAngle = Quaternion.AngleAxis(-visionConeAngle / 2, Vector3.up);
        stepAngle = Quaternion.AngleAxis(visionConeAngle / rayCastNumber, Vector3.up);
        base.Start();
    }
    public override void Update()
    {
        if (Vector3.Distance(_startPos, transform.position) >= bulletRange)
        {
            Debug.Log("Too Far");
            if (!hitEnemy && enemy == null)
            {
                Die();
            } 
        }
        if (!hitEnemy) 
        {
            //Quaternion angle = transform.rotation * startingAngle;
            //Vector3 direction = angle * Vector3.forward;
            //if (enemy == null)
            //{

            //    for (int i = 0; i < rayCastNumber; i++)
            //    {
            //        RaycastHit hit;
            //        if (Physics.Raycast(transform.position, direction, out hit, detectionRange, enemies))
            //        {
            //            enemy = hit.transform.gameObject;
            //        }
            //        direction = stepAngle * direction;

            //    }
            //}
            //if (enemy != null)
            //{
            //    enemyLocation = enemy.transform.position;
            //    enemyLocation.y = transform.position.y;
            //    transform.forward = (enemyLocation - transform.position).normalized;
            //}
            BulletMovement(transform.forward);
        }
        else if(!hitThings)
        {
            hitThings=true;
            if (enemy != null) 
            {
                bulletRB.velocity = enemy.transform.position - transform.position;
            }
            
        }
        else
        {
            if (enemy != null) 
            {
                DamageAfterSticking();
                Debug.Log("Tracking");
                bulletRB.velocity = Vector3.zero;
                enemyLocation = enemy.transform.position;
                if (!tracking)
                {
                    tracking = true;
                    x = (enemyLocation - transform.position).x;
                    y = (enemyLocation - transform.position).y;
                    z = (enemyLocation - transform.position).z;
                }
                Vector3 followLoaction = new Vector3(enemyLocation.x - x, enemyLocation.y - y, enemyLocation.z - z);
                transform.position = followLoaction;
            }
            else
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Die();
            }

        }
       
        
    }
    public override void BulletMovement(Vector3 forceDirection)
    {
        base.BulletMovement(forceDirection);
        bulletRB.AddForce(forceDirection * bulletSpeed, ForceMode.Force);

    }
    public override void OnTriggerEnter(Collider collision)
    {
        //base.OnTriggerEnter(collision);

        if (collision.CompareTag("Enemies"))
        {
            Debug.Log("Hit");
            hitEnemy = true;
            enemy = collision.gameObject;
            //transform.position = collision.transform.position;
        }
    }

    public void HitEnemy()
    {
        hitThings = true;
    }

    public void DamageAfterSticking()
    {
        explosionTime -= Time.deltaTime;

        if(explosionTime <= 0)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().TakeDamage(bulletDamage);
                if(enemy.GetComponent<Enemy>().currentHP <= bulletDamage)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                }
            }
            Die();
        }
    }
}
