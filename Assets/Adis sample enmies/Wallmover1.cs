using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallmover : MonoBehaviour
{

    [SerializeField]
    Vector3 destinationRelative = new Vector3(0f, 10f, 0f);
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    float pauseDuration = 1f;

    Vector3 startPos;
    bool isPaused;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            Vector3 destination = startPos + destinationRelative;

            // Move towards the destination
            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                yield return null;
            }

            // Pause at the destination
            isPaused = true;
            yield return new WaitForSeconds(pauseDuration);
            isPaused = false;

            // Move back to the start position
            Vector3 startDestination = startPos;
            while (Vector3.Distance(transform.position, startDestination) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startDestination, speed * Time.deltaTime);
                yield return null;
            }

            // Pause at the start position
            isPaused = true;
            yield return new WaitForSeconds(pauseDuration);
            isPaused = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 firstPos = Application.isPlaying ? startPos : transform.position;
        Gizmos.DrawLine(firstPos, firstPos + destinationRelative);
        Gizmos.DrawWireSphere(firstPos + destinationRelative, 0.2f);
    }
}
