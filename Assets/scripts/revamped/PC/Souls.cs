using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souls : MonoBehaviour
{
    [SerializeField]
    private int soulTimer;

    public AmmoManager.EquippedAmmoType soulType;
    IEnumerator ConsumptionCoroutine()
    {
        yield return new WaitForSeconds(soulTimer);
        Die();
    }
    public void Consumption()
    {
        StartCoroutine(ConsumptionCoroutine());
    }
    public AmmoManager.EquippedAmmoType SetBulletType()
    {
        return soulType;
    }
    void Die()
    {
        if(FindObjectOfType<PC>() != null)
        {
            AmmoManager.instance.ChangeEquippedAmmo(SetBulletType());
            FindObjectOfType<PC>().DoneConsuming();
        } 
        Destroy(this.gameObject);
    }
}
