using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public GameObject currentPot;

    public GameObject pot;

    [SerializeField]
    private float coolDown;

    private float timeOfLastPickUp;

    private bool didPickUp = false;

    private void Start()
    {
        
        currentPot = Instantiate(pot, transform.position, Quaternion.identity);
        currentPot.GetComponent<HealthPack>().SetSpawner(this.GetComponent<HealthPackSpawner>());
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
                currentPot = Instantiate(pot, transform.position, Quaternion.identity);
                currentPot.GetComponent<HealthPack>().SetSpawner(this.GetComponent<HealthPackSpawner>());
                didPickUp = false;
            }
        }
    }
    public void RemoveCurrentPot()
    {
        if (currentPot != null)
        {
            Debug.Log("Ate Health");
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
