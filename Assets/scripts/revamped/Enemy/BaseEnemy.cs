using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyStates
    {
        WANDER,
        SHIELDED,
        SLOWED,
        POISONED,
        BURNED,
        CHASING,
        ABILITY,
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


    public virtual void  Start()
    {
        currentEnemyStates = EnemyStates.WANDER;
        startingAngle = Quaternion.AngleAxis(-visionConeAngle/2, Vector3.up);
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

    }
    public virtual void TakeDamage()
    {

    }

    public virtual void CheckForPlayer() 
    {
        
        RaycastHit hit;

        Quaternion angle = transform.rotation * startingAngle;

        Vector3 direction = angle * Vector3.forward;

        Vector3 pos = transform.position;

        for (int i = 0; i < (visionConeAngle/5) + 1; i++)
        {
            if(Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                PC pc = hit.collider.GetComponent<PC>();

                if (pc != null)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);
            }

            direction = stepAngle * direction;
        }
    }
}
