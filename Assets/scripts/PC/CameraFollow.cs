using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PCController playerpos;

    [SerializeField]
    private float cameraOffsetY = 7f;
    [SerializeField]
    private float cameraOffsetZ = -4f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(playerpos.transform.position.x, playerpos.transform.position.y + cameraOffsetY, playerpos.transform.position.z + cameraOffsetZ);

    }
}
