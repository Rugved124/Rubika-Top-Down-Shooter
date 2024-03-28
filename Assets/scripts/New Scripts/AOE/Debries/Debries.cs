using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Debries : MonoBehaviour
{
    [SerializeField]
    private bool isPC;

    [SerializeField]
    private bool isSet;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float fallingSpeed;

    [SerializeField]
    private bool canBeDestroyed;

    [SerializeField]
    private float fallingTime;

    [SerializeField]
    private int spikeDamage;


    [SerializeField]
    bool isHit;

    [SerializeField]
    bool canSpikeHit;
    [SerializeField]
    bool spiked;

    [SerializeField]
    GameObject cross;

    [SerializeField]
    float dieTime;

    [SerializeField]
    GameObject debryBlast;
    void Start()
    {
        isHit = false;
        StartCoroutine(FallDown());
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Enemies") && !other.CompareTag("Shield") && !other.CompareTag("Player"))
        {
            if(cross != null)
            {
                cross.SetActive(false);
            }
            //this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = true;
            if(other != null && !isHit)
            {
                isHit = true;
                Invoke("Die", dieTime);
            }
            if (spiked)
            {   
                this.GetComponent<Collider>().isTrigger = false;
            }
        }
        if (isSet)
        {
            if (isPC)
            {
                if (other.CompareTag("Enemies") && !isHit)
                {
                    Debug.Log("HitEnemies");
                    isHit = true;
                    other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                    Invoke("Die", dieTime);
                }
                if (other.CompareTag("Shield") && !isHit)
                {
                    isHit = true;
                    if (!other.gameObject.GetComponent<ShieldBehaviour>().isPC)
                    {
                        other.gameObject.GetComponent<ShieldBehaviour>().TakeDamage(1);
                        Invoke("Die", dieTime);
                    }
                }
            }
            else
            {
                if (other.CompareTag("Shield") && !isHit)
                {
                    isHit = true;
                    if (other.gameObject.GetComponent<ShieldBehaviour>().isPC)
                    {
                        other.gameObject.GetComponent<ShieldBehaviour>().TakeDamage(1);
                        Invoke("Die", dieTime);
                    }
                }
                if (other.CompareTag("Player") && !isHit)
                {
                    isHit = true;
                    other.gameObject.GetComponent<PC>().TakeDamage(damage);
                    Invoke("Die", dieTime);
                }
            }
        }
        else
        {
            if(other.CompareTag("Shield") && !isHit)
            {
                isHit = true;
                other.gameObject.GetComponent<ShieldBehaviour>().TakeDamage(1);
                Invoke("Die", dieTime);
            }
            if (other.CompareTag("Player") && !isHit)
            {
                isHit = true;
                other.gameObject.GetComponent<PC>().TakeDamage(damage);
                Invoke("Die", dieTime);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && canSpikeHit)
        {
            canSpikeHit = false;
            collision.collider.GetComponent<PC>().TakeDamage(spikeDamage);
            Invoke("ResetSpikes", 1f);
        }
    }
    public void SetIfPC(bool _true)
    {
        isPC = _true;
        isSet = true;
    }

    private void Die()
    {
        if(debryBlast != null)
        {
            GameObject go = Instantiate(debryBlast, transform.position, Quaternion.identity);
            Destroy(go, 1f);
        }
        if (canBeDestroyed)
        {
            Destroy(this.gameObject);
        }
    }
    public void GetDamage(int _damage)
    {
        damage = _damage;
    }

    public IEnumerator FallDown()
    {
        yield return new WaitForSeconds(fallingTime);
        this.GetComponent<Rigidbody>().AddForce(Vector3.down * fallingSpeed, ForceMode.Impulse);
    }

    public void ResetSpikes()
    {
        canSpikeHit = true;
    }
}
