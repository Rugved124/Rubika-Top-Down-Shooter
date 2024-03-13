using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClusterBomb : BaseBullet
{
    [SerializeField]
    int splitTimes;

    [SerializeField]
    int splitNumber;

    [SerializeField]
    float splitAngle = 90;

    [SerializeField]
    GameObject splitBullet;

    private Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    private Quaternion stepAngle = Quaternion.AngleAxis(0, Vector3.up);

    bool isHit;
    public bool isSplit;
    float time;
    public override void Start() 
    {
        if (isSplit)
        {
            time = 0;
        }
        else
        {
            time = 0.1f;
        }
        isHit = true;
        startingAngle = Quaternion.AngleAxis(-splitAngle / 2, Vector3.up);
        stepAngle = Quaternion.AngleAxis(splitAngle / (splitNumber - 1), Vector3.up);
        base.Start();
        BulletMovement(transform.forward);
    }
    public override void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0f) 
        {
            isHit = false;
        }
        base.Update();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            if (collision.tag != "Shield" && collision.tag != "Enemies" && collision.tag != "Player")
            {
                Destroy(this.gameObject);
            }
            if (collision.CompareTag("Enemies"))
            {
                if (collision.GetComponent<Enemy>() != null)
                {
                    if (!isHit)
                    {
                        isHit = true;
                        collision.GetComponent<Enemy>().TakeDamage(bulletDamage);
                    }
                }
            }
        }
       
        
    }
    public void OnTriggerExit(Collider collision) 
    {
        if(collision != null) 
        {

            if (collision.CompareTag("Enemies"))
            {
                if (collision.GetComponent<Enemy>() != null)
                {
                    Die();
                }
            }
        }
        
    }
    public override void Die()
    {
        if(splitTimes > 0) 
        {
            for(int i = 0; i < splitNumber; i++)
            {
                GameObject _splitBullet = Instantiate(splitBullet, transform.position,transform.rotation * startingAngle);
                _splitBullet.GetComponent<ClusterBomb>().SetSplitNumber(splitTimes - 1);
                _splitBullet.GetComponent<ClusterBomb>().SetDamage(bulletDamage / 2);
                _splitBullet.GetComponent<ClusterBomb>().isSplit = true;
                startingAngle *= stepAngle;
            }
        }
        base.Die();
    }
    public override void BulletMovement(Vector3 forceDirection)
    {
        base.BulletMovement(forceDirection);
        bulletRB.AddForce(forceDirection * bulletSpeed, ForceMode.Impulse);
    }
    public void SetSplitNumber(int number)
    {
        splitTimes = number;
    }
    public void SetDamage(int damage) 
    {
        bulletDamage = damage;
    }
}
