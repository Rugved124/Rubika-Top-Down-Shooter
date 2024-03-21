using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    [SerializeField]
    float maxTime;
    void Start()
    {
        Destroy(this.gameObject, maxTime);
    }

}
