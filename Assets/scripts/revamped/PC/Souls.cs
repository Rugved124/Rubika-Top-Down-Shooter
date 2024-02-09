using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souls : MonoBehaviour
{
    [SerializeField]
    private int soulTimer;

    IEnumerator ConsumptionCoroutine()
    {
        yield return new WaitForSeconds(soulTimer);
        Die();
    }
    public void Consumption()
    {
        StartCoroutine(ConsumptionCoroutine());
    }
    void Die()
    {
        if(FindObjectOfType<PC>() != null)
        {
            FindObjectOfType<PC>().DoneConsuming();
        } 
        Destroy(this.gameObject);
    }
}
