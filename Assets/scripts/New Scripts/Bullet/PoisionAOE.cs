using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PoisionAOE : MonoBehaviour
{
    [SerializeField]
    private float PoisonTime;

    [SerializeField]
    NavMeshObstacle poisonAoe;

    public bool isPC;
    private void Start()
    {
        poisonAoe = GetComponent<NavMeshObstacle>();
        if(isPC )
        {
            poisonAoe.enabled = false;
        }
        else
        {
            poisonAoe.enabled = true;
        }

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
