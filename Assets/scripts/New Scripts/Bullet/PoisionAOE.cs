using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisionAOE : MonoBehaviour
{
    [SerializeField]
    private float PoisonTime;

    private void Start()
    {
        
    }
    void Update()
    {
        PoisonTime -= Time.deltaTime;
        if (PoisonTime <= 0f )
        {
            Die();
        }
    }

    private void Die()
    {
        if(FindObjectOfType<PC>() != null)
        {
            if (FindObjectOfType<PC>().statusEffects.isSlowed)
            {
                FindObjectOfType<PC>().statusEffects.isSlowed = false;
            }
        }
        Destroy(this.gameObject);
    }
}
