using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        DRUNKENSEPOY,
        BUTCHER
    }

    public EnemyType enemyType;

    protected FiniteStateMachine fsm;

    public EnemyData enemyData;
    public PC pc { get; private set; }

    [SerializeField]
    protected float firedTime;

    [SerializeField]
    private GameObject enemySoul;
    public NavMeshAgent agent { get; private set; }

    public bool isWeaponFiringDone {  get; protected set; }


    public float angularSpeedMulitplier;

    public float MaxHP;
    public float currentHP;

    private void Awake()
    {
        firedTime = enemyData.timeBetweenBullets;
        fsm = GetComponent<FiniteStateMachine>();
        pc = FindObjectOfType<PC>();
        agent = GetComponent<NavMeshAgent>();
        //NavMesh Stuff
        agent.speed = enemyData.enemyMoveSpeed;
        agent.angularSpeed = enemyData.enemyRotationSpeed;

        angularSpeedMulitplier = enemyData.enemyAttackingAngularSpeed;
        MaxHP = enemyData.HitPoints;
        currentHP = MaxHP;
        
        isWeaponFiringDone = false;

        InitializeStateMachine();
    }

    public virtual void Start()
    {
        
    }

    public virtual void InitializeStateMachine()
    {
       
    }

    public virtual void FireWeapon()
    {
        
    }

    public virtual void Update() 
    {
        if(currentHP <= 0)
        {
             Die();
        }
    }

    public string GetCurrentState()
    {
        if (fsm != null)
        {
            if (fsm.currentState != null)
            {
                return fsm.currentState.ToString();
            }
            else
            {
                return "No State Detected";
            }
        }
        
        return "Not Playing";
        
    }
    public void Die()
    {
        DropSoul(enemySoul);
        Destroy(this.gameObject);
    }

    public virtual void  ResetAttack()
    {
    }
    public virtual void SetFiringToTrue()
    {
        isWeaponFiringDone = false;
        Debug.Log(isWeaponFiringDone);
    }
    public virtual void LookAtPlayer()
    {
        
    }

    public void TakeDamage(int damage) 
    {
        currentHP -= damage;
    }
    
    void DropSoul(GameObject enemySoul)
    {
        Instantiate(enemySoul, transform.position, Quaternion.identity);
    }


}
