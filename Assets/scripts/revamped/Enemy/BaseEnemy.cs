using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyStates
    {
        WANDER,
        CHASING,
        CHARGING,
        ATTACKING,
        RUNAWAY,
        DEAD
    }
    public EnemyStates currentEnemyStates;

    [SerializeField]
    protected int maxHitPoints;

    [SerializeField]
    protected int moveSpeed;

    [SerializeField]
    protected int damageDealtTopPlayer;


    protected int currentHitPoints;

    [SerializeField]
    protected float visionConeAngle = -90;
    [SerializeField]
    float aggroRadius;

    Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    protected Vector3 destination;

    protected NavMeshAgent navMeshAgent;

    protected bool isPCDetected;

    PC pc;

    [SerializeField]
    protected Transform hitPointBar;

    public virtual void Awake()
    {
        currentHitPoints = maxHitPoints;
        navMeshAgent = GetComponent<NavMeshAgent>();
        startingAngle = Quaternion.AngleAxis(-visionConeAngle / 2, Vector3.up);
    }
    public virtual void Start()
    {
        currentEnemyStates = EnemyStates.WANDER;
    }

    public virtual void Update()
    {
        CheckForPlayer();
    }

    public virtual void ChangeState(EnemyStates state)
    {
        if (currentEnemyStates != state)
        {
            currentEnemyStates = state;
        }
        else return;
    }
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    public virtual void TakeDamage()
    {

    }

    public void GetDestination()
    {
        Vector3 testPosition = transform.position + (transform.forward * 4f) + new Vector3(Random.Range(-4.5f, 4.5f), 0, Random.Range(-4.5f, 4.5f));

        destination = new Vector3(testPosition.x, transform.position.y, testPosition.z);

    }

    public bool NeedsDestination()
    {
        if (destination == Vector3.zero)
        {
            return true;
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
    public virtual void CheckForPlayer()
    {

        RaycastHit hit;

        Quaternion angle = transform.rotation * startingAngle;

        Vector3 direction = angle * Vector3.forward;

        Vector3 pos = transform.position;

        for (int i = 0; i < (visionConeAngle / 5) + 1; i++)
        {

            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                pc = hit.collider.GetComponent<PC>();

                if (pc != null)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    isPCDetected = true;
                    return;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                    isPCDetected = false;

                }
            }
            else
            {
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);
                isPCDetected = false;

            }


            direction = stepAngle * direction;
        }
    }
    public Transform GetPlayerTransform()
    {
        if (pc != null)
        {
            return pc.transform;
        }
        else { return null; }


        
    }
    protected float HPPercentage()
    {
        float currentHPPercent = (currentHitPoints / maxHitPoints) ;
        return currentHPPercent;
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.collider.tag == "Bullets")
            {
                Die();
            }
        }
    }
}
