using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    PCController playerPos;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerPos = FindObjectOfType<PCController>();
    }

    private void Update()
    {
        navMeshAgent.destination = playerPos.transform.position;
    }

    void MoveToPlayer()
    {

    }
    void GetToCover()
    {

    }


}
