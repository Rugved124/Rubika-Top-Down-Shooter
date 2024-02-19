using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NONrotator : MonoBehaviour
{
    [SerializeField] Vector3 rotationAmount = new Vector3(0f, 90f, 0f); // Rotation amount per second
    [SerializeField] float pauseDuration = 1f; // Pause duration after each rotation

    bool isPaused;

    void Start()
    {
        StartCoroutine(RotateObject());
    }

    IEnumerator RotateObject()
    {
        while (true)
        {
            Vector3 startRotation = transform.rotation.eulerAngles;
            Vector3 endRotation = startRotation + rotationAmount;

            // Rotate towards the end rotation
            while (Quaternion.Angle(transform.rotation, Quaternion.Euler(endRotation)) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(endRotation), rotationAmount.magnitude * Time.deltaTime);
                yield return null;
            }

            // Pause after rotation
            isPaused = true;
            yield return new WaitForSeconds(pauseDuration);
            isPaused = false;
        }
    }
}
