using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        StartCoroutine(FallDown());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isSet)
        {
            if (!isPC)
            {
                if (other.CompareTag("Enemies"))
                {
                    other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            else
            {
                if (other.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<PC>().TakeDamage(damage);
                }
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PC>().TakeDamage(damage);
            }
        }
        Die();
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
}
