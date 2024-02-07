using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plattrigger : MonoBehaviour
{
    // List to hold the scripts that will be activated
    public List<MonoBehaviour> scriptsToActivate = new List<MonoBehaviour>();

    // The object that will trigger the activation
    public GameObject triggeringObject;

    // Called when a collider enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the triggering object
        if (other.gameObject == triggeringObject)
        {
            // Activate each script in the list
            foreach (var script in scriptsToActivate)
            {
                script.enabled = true;
            }
        }
    }

    // Called when a collider exits the trigger zone
    void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to the triggering object
        if (other.gameObject == triggeringObject)
        {
            // Deactivate each script in the list
            foreach (var script in scriptsToActivate)
            {
                script.enabled = false;
            }
        }
    }
}
