using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowTeleportState : BaseState
{
    private Enemy _enemy;
    float waitBeforeGoingBack;

    [SerializeField]
    protected float visionConeAngle = 90f;

    List<Collider> bullets;
    public List<Collider> newBullets = new List<Collider>();

    float attackCoolDown;
    public ShadowTeleportState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }
    public override void EnterState()
    {
        attackCoolDown = _enemy.firedTime;
        _enemy.ResetAttack();
    }

    public override Type ExecuteState()
    {
        attackCoolDown -= Time.deltaTime;
        if(Physics.OverlapSphere(_enemy.transform.position, 4f,_enemy.all, QueryTriggerInteraction.Collide) != null)
        {

            bullets = Physics.OverlapSphere(_enemy.transform.position, 4f,_enemy.all, QueryTriggerInteraction.Collide).ToList();
            if(bullets.Count > 0)
            {
                if (bullets.Exists(r => r.gameObject.GetComponent<BaseBullet>().IsPCBullet()))
                {
                    _enemy.Teleport();
                }
            }
            
        }
        if (attackCoolDown <= 0f)
        {
            return typeof(IdleState);
        }
        
       
        return null;
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }
}
