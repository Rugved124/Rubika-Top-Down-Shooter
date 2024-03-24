using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public HealthPack currentPot;

    public GameObject pot;

    [SerializeField]
    private float coolDown;

    private float timeOfLastPickUp;

    private bool didPickUp = false;

    private void Start()
    {
        Instantiate(pot, transform.position, Quaternion.identity);
        currentPot = pot.GetComponent<HealthPack>();
        currentPot.SetSpawner(this.GetComponent<HealthPackSpawner>());
    }
    private void Update()
    {

        if (currentPot == null)
        {
            if (!didPickUp)
            {
                didPickUp = true;
                timeOfLastPickUp = Time.time;
            }
            if(Time.time - timeOfLastPickUp >= coolDown)
            {
                Instantiate(pot, transform.position, Quaternion.identity);
                currentPot = pot.GetComponent<HealthPack>();
                currentPot.SetSpawner(this.GetComponent<HealthPackSpawner>());
                didPickUp = false;
            }
        }
    }
    public void RemoveCurrentPot()
    {
        if (currentPot != null)
        {
            currentPot = null;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    if(other.GetComponent<HealthPack>() != null)
    //    {
    //        currentPot = other.GetComponent<HealthPack>();
    //    }
    //}
}
