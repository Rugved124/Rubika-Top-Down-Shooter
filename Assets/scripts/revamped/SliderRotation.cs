using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderRotation : MonoBehaviour
{
    Quaternion startingRotation;

    private void Start()
    {
        startingRotation = transform.localRotation;
    }

    private void Update()
    {
        if(transform.rotation != startingRotation)
        {
            transform.rotation = startingRotation;
        }
    }
}
