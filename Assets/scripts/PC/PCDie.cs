using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCDie : MonoBehaviour
{
    public static PCDie pcDie;
    public bool isDead;
    private void Awake()
    {
        if (pcDie == null)
        {
            pcDie = this;

        }
        else
        {
            Destroy(this);
        }

    }
  
    void Update()
    {
        if (isDead)
        {
            Die();
        }
    }

    void Die() 
    { 
        this.gameObject.SetActive(false);
    }
}
