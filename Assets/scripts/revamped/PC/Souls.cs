using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Souls : MonoBehaviour
{
    [SerializeField]
    private float soulTimer;

    public AmmoManager.EquippedAmmoType soulType;

    [SerializeField]
    Slider soulSlider;

    private void Awake()
    {
        soulSlider = GetComponentInChildren<Slider>();
        soulSlider.maxValue = soulTimer;
        soulSlider.value = 0;
    }
    IEnumerator ConsumptionCoroutine()
    {
        soulSlider.value += Time.deltaTime;
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
