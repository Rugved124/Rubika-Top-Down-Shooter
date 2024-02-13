using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTimer : MonoBehaviour
{
    public float timer;
    void Start()
    {
        timer = 5f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            if (FindObjectOfType<PC>())
            {
                FindObjectOfType<PC>().statusEffects.isSlowed = false;
                FindObjectOfType<PC>().statusEffects.hasLostAbility = false;
                FindObjectOfType<PC>().slowMultiplier = 1;
            }
            Destroy(this.gameObject);
        }
    }
}
