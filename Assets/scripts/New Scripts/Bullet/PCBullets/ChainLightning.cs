using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public override void Start()
    {
        if (!hasSetPos)
        {
            Debug.Log("wierd");
            transform.forward = transform.forward;
        }
        else 
        {
            Debug.Log("Good");
            transform.forward = lookPos;
        }
        StartLightning();
        canJump = false;
        Invoke("Die", 1f);
    }
    public override void Update()
    {
        base.Update();
        if (!canJump) 
        {
            canJump = true;
            if (Physics.OverlapSphere(_startPos, bulletRange, enemies) != null)
            {
                colliders = Physics.OverlapSphere(_startPos, bulletRange, enemies).ToList(); 
                if(currentEnemy != null) 
                {
                    colliders.Remove(currentEnemy.GetComponent<Collider>());
                }
                if(colliders.Count > 0) 
                {
                    if (colliders[0] != null)
                    {
                        respawnThis.GetComponent<ChainLightning>().currentEnemy = colliders[0].gameObject;
                        respawnThis.GetComponent<ChainLightning>().SetLookPos();
                    }
                }
                

            }
        }
    }
    private void StartLightning() 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletRange) && canJump)
        {
            GameObject lightning = Instantiate(chainLightning,transform.position +  transform.forward * Vector3.Distance(transform.position, hit.transform.position) / 2, transform.rotation);
            lightning.transform.localScale = new Vector3(lightning.transform.localScale.x, lightning.transform.localScale.y, Vector3.Distance(transform.position, hit.transform.position));
            Destroy(lightning, 1f);
            if (hit.collider.CompareTag("Enemies"))
            {
                Debug.Log("Hit Enemies");
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
                _startPos = hit.transform.position;
                respawnThis.GetComponent<ChainLightning>().SetStartPos(hit.transform.position, jumps);
                currentEnemy = hit.collider.gameObject;
            }
        }
        else
        {
            GameObject lightning = Instantiate(chainLightning, transform.position + transform.forward * bulletRange / 2, transform.rotation);
            lightning.transform.localScale = new Vector3(lightning.transform.localScale.x, lightning.transform.localScale.y, bulletRange);
            respawnThis.GetComponent<ChainLightning>().SetStartPos(transform.position, 0);
            Destroy(lightning, 0.3f);
        }
    }

    public override void Die()
    {
        if (jumps > 0)
        {
            Instantiate(respawnThis, _startPos, Quaternion.identity);
        }
        base.Die();
    }

    public void SetStartPos(Vector3 _startPos, int jumpCount)
    {
        hasSetPos = true;
        this._startPos = _startPos;
        jumps = jumpCount -  1;
    }
    public void SetLookPos() 
    {
        Debug.Log("Check this");
        hasSetPos = true;
        lookPos = (currentEnemy.transform.position - _startPos).normalized;
    }
}
