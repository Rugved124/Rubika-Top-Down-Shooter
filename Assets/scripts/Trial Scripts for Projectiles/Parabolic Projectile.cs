using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicProjectile : MonoBehaviour
{
    Rigidbody rb;
    Vector3 startPos;
    public Transform endPosObj;
    public Transform centerOffset;
    Vector3 endPos;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        endPos = endPosObj.position;
        
    }

    private void Update()
    {
        startPos = transform.position;
        //rb.MovePosition(Vector3.Slerp(startPos, endPos, Time.deltaTime));   
    }

    private void OnDrawGizmos()
    {
        float duration = 2f; // Adjust as needed
        for (float t = 0; t <= 1; t += Time.deltaTime / duration)
        {
            Gizmos.DrawSphere(GetPosition(t, centerOffset.position.y), 0.1f);
        }
    }

    private Vector3 GetPosition(float t, float centerOffset) 
    {
        Vector3 centerPoint = (transform.position + endPosObj.position) * 0.5f;
        centerPoint += new Vector3(0,centerOffset);
        Vector3 startPoint = (transform.position) - centerPoint;
        Vector3 endPoint = (endPosObj.position) - centerPoint;
        return Vector3.Slerp(startPoint, endPoint, t) + centerPoint ;
    }
}
