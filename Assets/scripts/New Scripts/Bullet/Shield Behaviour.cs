using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    [SerializeField]
    private int shieldCount;

    private int maxShieldCount;

    private GameObject enemy;

    public bool isPC;// { get; private set; }

    private void Awake()
    {
        maxShieldCount = shieldCount;
    }
    private void Update()
    {
        if (shieldCount <= 0)
        {
            Die();
        }
        if (isPC)
        {
            if ((FindObjectOfType<PC>() == null || FindObjectOfType<PC>().isDead))
            {
                Die();
            }
            
        }
        else
        {
            if(enemy != null)
            {
                if (enemy.GetComponent<Enemy>() == null)
                {
                    Die();
                }
            }

        }
    }
    public void Die()
    {
        AmmoManager.instance.currentShield = null;
        Destroy(this.gameObject);
    }

    public void FollowSpawner(Transform _transform)
    {
        transform.position = _transform.position;
        transform.forward = _transform.forward;
    }

    public void TakeDamage(int damage)
    {
        shieldCount -= damage;
    }

    public void IsPCShield(bool yes)
    {
        isPC = yes;
    }

    public void SetParentToEnemy(GameObject _enemy)
    {
        enemy = _enemy;
    }

    public void ResetShield()
    {
        shieldCount = maxShieldCount;
    }
}
