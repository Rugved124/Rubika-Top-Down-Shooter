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

    float attackCoolDown;
    public ShadowTeleportState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }
    public override void EnterState()
    {
        attackCoolDown = _enemy.firedTime;
    }

    public override Type ExecuteState()
    {
        attackCoolDown -= Time.deltaTime;
        if(Physics.OverlapSphere(_enemy.transform.position, 2f) != null)
        {
            bullets = Physics.OverlapSphere(_enemy.transform.position, 2f).ToList();
            if(bullets.Exists(r => r.CompareTag("PlayerBullets")))
            {
                _enemy.Teleport();
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
