using UnityEngine;

public class PCCollisionHandler : MonoBehaviour
{
    public static PCCollisionHandler instance;
    public bool isSlowedCounting, isPoisonCounting, isSilenceCounting;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

    }
    private void Update()
    {
        //Debug.Log(isPoisonCounting);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "SlowPC")
            {
                isSlowedCounting = true;
                PCStatusEffects.instance.isSlowed = true;

            }
            if (other.tag == "Poison")
            {
                isPoisonCounting = true;
                PCStatusEffects.instance.isPoisoned = true;

            }
            if (other.tag == "SilenceAbility")
            {
                PCStatusEffects.instance.hasLostAbility = true;

            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "SlowPC")
            {
                isSlowedCounting = false;
            }
            if (other.tag == "Poison")
            {
                isPoisonCounting = false;
            }
            if (other.tag == "SilenceAbility")
            {
                PCStatusEffects.instance.hasLostAbility = false;
            }
        }
    }
}
