using System;
using System.Collections.Generic;
using UnityEngine;

public class ButcherIdleState : BaseState
{
    private Enemy _enemy;

    Quaternion startAngle = Quaternion.AngleAxis(0, Vector3.up);

    public ButcherIdleState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;

        startAngle = Quaternion.AngleAxis(-_enemy.enemyData.visionConeAngle / 2f, Vector3.up);
    }
    public override void EnterState()
    {

    }

    public override Type ExecuteState()
    {

        float distanceFromPC = CalculateDistance(_enemy.pc.transform);

        if (distanceFromPC >= _enemy.enemyData.attackRange)
        {
            return typeof(ButcherRunToPCState);
        }
        else if (distanceFromPC < _enemy.enemyData.attackRange)
        {
            return typeof(ButcherAttackState);
        }

        return null;
    }

    public void CheckForAllies()
    {

        RaycastHit hit;

        Quaternion angle = _enemy.transform.rotation * startAngle;

        Vector3 direction = angle * Vector3.forward;

        Vector3 pos = _enemy.transform.position;

        for (int i = 0; i < (_enemy.enemyData.visionConeAngle / 5) + 1; i++)
        {

            if (Physics.Raycast(pos, direction, out hit, _enemy.enemyData.aggroRadius))
            {

                if (_enemy.pc != null)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);

                }
            }
            else
            {
                Debug.DrawRay(pos, direction * _enemy.enemyData.aggroRadius, Color.white);
            }


            direction = _enemy.enemyData.stepAngle * direction;
        }
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }
}
