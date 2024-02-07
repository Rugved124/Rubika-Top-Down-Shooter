using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyRuggy : MonoBehaviour
{
	public enum EnemyStates
	{
		WANDER,
		SHIELDED,
		SLOWED,
		POISONED,
		BURNED,
		CHASING,
		RUNAWAY,
		DEAD
	}
	public EnemyStates currentEnemyStates;

	[SerializeField] protected int maxHitPoints;
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected int damageDealtToPlayer;
	protected int currentHitPoints;


	protected Vector3 destination;
	protected Vector3 direction;

	Quaternion startingAngle = Quaternion.AngleAxis(-90, Vector3.up);
	Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

	[SerializeField] protected int visionConeAngle;

	protected NavMeshAgent navMeshAgent;

	public virtual void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		startingAngle = Quaternion.AngleAxis(-visionConeAngle/2, Vector3.up);
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
	}

	private void Die()
	{

	}

	public virtual void TakeDamage()
	{

	}

	public virtual void GetDestination()
	{
		Vector3 testPosition = (transform.position + (transform.forward * 4f)) + new Vector3(Random.Range(-4.5f, 4.5f), 0f, Random.Range(-4.5f, 4.5f));

		destination = new Vector3(testPosition.x, transform.position.y, testPosition.z);

	}

	public bool NeedsDestination()
	{
		if (destination == Vector3.zero)
		{
			return true;
		}
		if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			return true;
		}

		return false;
	}
	public virtual void CheckForPlayer()
	{
		//the length of each line 
		float aggroRadius = 5f;
		//to check if the raycast has hit something
		RaycastHit hit;

		//stores an angle, the computer simulates what will happen if you rotate any angle
		Quaternion angle = transform.rotation * startingAngle;

		Vector3 direction = angle * Vector3.forward;

		Vector3 pos = transform.position;

		//for loop that repeats 38 times as that's the number of vision cone number
		for (int i = 0; i < (visionConeAngle/5)+1; i++)
		{
			if (Physics.Raycast(pos, direction, out hit, aggroRadius))
			{
				//stores PC script, and checks if any object has a PC script
				PC pc = hit.collider.GetComponent<PC>();

				//Checks if the raycast has hit a PC (this line says that if it is, then its red color)
				if (pc != null)
				{
					Debug.DrawRay(pos, direction * hit.distance, Color.red);
				}
				//Any other object then its yellow
				else
				{
					Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
				}
			}
			//White when it does not hit anything
			else
			{
				Debug.DrawRay(pos, direction * aggroRadius, Color.white);
			}

			direction = stepAngle * direction;
		}
	}
}
