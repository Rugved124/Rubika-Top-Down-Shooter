using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainLightning : BaseBullet
{
    [SerializeField]
    GameObject chainLightning;

    [SerializeField]
    GameObject respawnThis;

    [SerializeField]
    int jumps;

    bool canJump = true;

    [SerializeField]
    LayerMask enemies;

    [SerializeField]
    List<Collider> colliders = new List<Collider>(10);

    public GameObject currentEnemy;

    [SerializeField]
    Vector3 _startPos;

    [SerializeField]
    Vector3 lookPos;

    [SerializeField]
    bool hasSetPos;

    GameObject shield;
    public override void Start()
    {
        base.Start();
        if (!hasSetPos)
        {
            transform.forward = transform.forward;
        }
        else 
        {
            transform.forward = lookPos;
        }
        BulletMovement(transform.forward);
        //StartLightning();
        canJump = false;
    }
    public override void Update()
    {
        base.Update();
    }
    //private void StartLightning() 
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, bulletRange) && canJump)
    //    {
    //        GameObject lightning = Instantiate(chainLightning,transform.position +  transform.forward * Vector3.Distance(transform.position, hit.transform.position) / 2, transform.rotation);
    //        lightning.transform.localScale = new Vector3(lightning.transform.localScale.x, lightning.transform.localScale.y, Vector3.Distance(transform.position, hit.transform.position));
    //        Destroy(lightning, 1f);
    //        if (hit.collider.CompareTag("Enemies"))
    //        {
    //            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
    //            _startPos = hit.transform.position;
    //            respawnThis.GetComponent<ChainLightning>().SetStartPos(hit.transform.position, jumps);
    //            currentEnemy = hit.collider.gameObject;
    //        }
    //    }
    //    else
    //    {
    //        GameObject lightning = Instantiate(chainLightning, transform.position + transform.forward * bulletRange / 2, transform.rotation);
    //        lightning.transform.localScale = new Vector3(lightning.transform.localScale.x, lightning.transform.localScale.y, bulletRange);
    //        respawnThis.GetComponent<ChainLightning>().SetStartPos(transform.position, 0);
    //        Destroy(lightning, 0.3f);
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemies") && other.CompareTag("Shield"))
        {
            if(other.gameObject != currentEnemy && other.gameObject != shield)
            {
                if (other.CompareTag("Enemies"))
                {
                    currentEnemy = other.gameObject;
                    other.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
                    jumps--;
                }
                if (other.CompareTag("Shield"))
                {
                    shield = other.gameObject;
                    other.gameObject.GetComponent<ShieldBehaviour>().TakeDamage(1);
                    jumps--;
                }
            }
            SetStartPos(transform.position);
            if (Physics.OverlapSphere(_startPos, bulletRange, enemies) != null)
            {
                colliders = Physics.OverlapSphere(_startPos, bulletRange, enemies).ToList();
                if (currentEnemy != null)
                {
                    colliders.Remove(currentEnemy.GetComponent<Collider>());
                }
                if (colliders.Count > 0)
                {
                    if (colliders[0] != null)
                    {
                        currentEnemy = colliders[0].gameObject;
                        SetLookPos();
                        BulletMovement(transform.forward);
                    }
                    else
                    {
                        Die();
                    }
                }
            }
            else
            {
                Die();
            }
        }
        else
        {
            if(other != null)
            {
                Die();
            }
        }
    }
    public override void BulletMovement(Vector3 forceDirection)
    {
        base.BulletMovement(forceDirection);
        bulletRB.AddForce(forceDirection.normalized *  bulletSpeed, ForceMode.Impulse);
    }
    public override void Die()
    {
        base.Die();
    }

    public void SetStartPos(Vector3 _startPos)
    {
        hasSetPos = true;
        this._startPos = _startPos;
        jumps--;
    }
    public void SetLookPos() 
    {
        hasSetPos = true;
        lookPos = (currentEnemy.transform.position - _startPos).normalized;
    }
}
