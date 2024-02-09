using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    [SerializeField]
    private float shadowTrackingTime;

    [SerializeField]
    private float trailingDistance;

    [SerializeField]
    private int hitDamage;

    private PC pc;

    bool isPlayerHit;

    public GameObject spawnPrefab;

    public Collider[] colliders;

    Vector3 spawnPoint;
    public LayerMask spawnLayerMask;
    [SerializeField]
    float gap;
    bool some;
    private void Start()
    {
        pc = FindObjectOfType<PC>();
        isPlayerHit = false;
    }
    void Update()
    {
        shadowTrackingTime -= Time.deltaTime;
        if (shadowTrackingTime <= 0f)
        {
            StartCoroutine(DieCoRoutine());
            
            if (!some)
            {
                Invoke("Die", 1f);
                some = true;
            }
               

            
        }
        else
        {
            if(pc != null)
            {
                Vector3 followPos = new Vector3(pc.transform.position.x, transform.position.y, pc.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, followPos, trailingDistance);
            }
            
        }

    }


    void Die()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnSpheres();
        }
        Destroy(this.gameObject);
    }
    IEnumerator DieCoRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;
    }

    private void OnTriggerEnter (Collider other)
    {
        if(other.tag == "Player" && !isPlayerHit)
        {
            pc.TakeDamage(hitDamage);
        }
    }
    void SpawnSpheres()
    {
        int safetyNet = 0;
        bool canSpawn = false;
        bool dontSpawn = false;
        while (!canSpawn)
        {
            float x = Random.Range(2.5f, -2.5f);
            float z = Random.Range(2.5f, -2.5f);
            
            spawnPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            canSpawn = PreventOverlapSpawn(spawnPoint);
            if (canSpawn)
            {
                break;
            }
            safetyNet++;
            if (safetyNet > 50)
            {
                dontSpawn = true;
                Debug.Log("Too Many Attempts");
                break;
                
            }
        }
        if (!dontSpawn)
        {
            Instantiate(spawnPrefab, spawnPoint, Quaternion.identity);
        }
       

    }
    private bool PreventOverlapSpawn(Vector3 _spawnPoint)
    {
        colliders = Physics.OverlapSphere(transform.position, 5f, spawnLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x + gap;
            float hieght = colliders[i].bounds.extents.z + gap;

            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.z - hieght;
            float upperExtent = centerPoint.z + hieght;

            if (_spawnPoint.x >= leftExtent && _spawnPoint.x <= rightExtent)
            {
                if (_spawnPoint.z >= lowerExtent && _spawnPoint.z <= upperExtent)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
