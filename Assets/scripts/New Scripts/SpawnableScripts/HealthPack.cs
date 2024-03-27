using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    PC pc;
    [SerializeField]
    private int healNumber = 20;

    [SerializeField]
    private HealthPackSpawner owner;
    private void Start()
    {
        pc = FindObjectOfType<PC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (pc != null)
            {
                if(pc.maxHP - pc.currentHP >= healNumber)
                {
                    pc.Heal(healNumber);
                }
                else
                {
                    pc.Heal((pc.maxHP - pc.currentHP));
                }
                Die();
            }
        }
        
    }

    private void Die()
    {
        if(owner != null)
        {
            owner.RemoveCurrentPot();
        }
        Destroy(this.gameObject);
    }

    public void SetSpawner(HealthPackSpawner spawner)
    {
        owner = spawner;
    }
}
