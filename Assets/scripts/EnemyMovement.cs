using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float detectionRange;
    private float shootingRange;
    public bool canShoot;
    PCController playerPos;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerPos = FindObjectOfType<PCController>();
        shootingRange = navMeshAgent.stoppingDistance;
    }

    public void Update()
    {

        var lookPos = playerPos.transform.position - transform.position;
        Debug.Log(lookPos.magnitude);
        Debug.DrawRay(transform.position,lookPos.normalized * detectionRange, Color.red);
        if (lookPos.magnitude <= detectionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, lookPos, out hit, detectionRange))
            {
                if (hit.collider.tag == "Player" && lookPos.magnitude >= shootingRange)
                {
                    MoveToPlayer();
                }
                else if (lookPos.magnitude <= shootingRange)
                {
                    StopMoving();
                }
            }
        }
        if (lookPos.magnitude <= shootingRange)
        {
            canShoot = true;
           // MoveBack(lookPos);
        }
    }

    void MoveToPlayer()
    {
        navMeshAgent.destination = playerPos.transform.position;
    }
    void MoveBack(Vector3 lookPos)
    {
        Debug.DrawLine(transform.position, lookPos.normalized * -shootingRange);
        navMeshAgent.destination = lookPos.normalized * -shootingRange;
    }
    void StopMoving()
    {
        navMeshAgent.destination = transform.position;
    }
}
