using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Debries : MonoBehaviour
{
    private bool isPC;

    private bool isSet;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float fallingSpeed;

    [SerializeField]
    private bool canBeDestroyed;

    [SerializeField]
    private float fallingTime;

    bool isHit;
    bool canSpikeHit;
    [SerializeField]
    bool spiked;

    [SerializeField]
    GameObject cross;
    void Start()
    {
        isHit = false;
        StartCoroutine(FallDown());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Enemies") && !other.CompareTag("Shield") && !other.CompareTag("Player"))
        {
            cross.SetActive(false);
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = true;
            isHit = true;
            if (spiked)
            {
                ResetSpikes();
                this.GetComponent<Collider>().isTrigger = false;
            }
        }
        if (isSet)
        {
            if (!isPC)
            {
                if (other.CompareTag("Enemies") && !isHit)
                {
                    isHit = true;
                    other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            else
            {
                if (other.CompareTag("Shield") && !isHit)
                {
                    isHit = true;
                    other.gameObject.GetComponent<ShieldBehaviour>().TakeDamage(1);
                }
                if (other.CompareTag("Player") && !isHit)
                {
                    isHit = true;
                    other.gameObject.GetComponent<PC>().TakeDamage(damage);
                }
            }
        }
        else
        {
            if(other.CompareTag("Shield") && !isHit)
            {
                isHit = true;
                other.gameObject.GetComponent<ShieldBehaviour>().TakeDamage(1);
            }
            if (other.CompareTag("Player") && !isHit)
            {
                isHit = true;
                other.gameObject.GetComponent<PC>().TakeDamage(damage);
            }
        }
        Die();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && canSpikeHit)
        {
            canSpikeHit = false;
            collision.collider.GetComponent<PC>().TakeDamage(damage/3);
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
