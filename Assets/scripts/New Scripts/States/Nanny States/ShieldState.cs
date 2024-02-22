using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldState : BaseState
{
	Enemy _enemy;
	float waitbeforeShield;
	float shieldCoolDown;
	public ShieldState(Enemy enemy) : base(enemy.gameObject)
	{
		_enemy = enemy;

	}
	public override void EnterState()
	{
        _enemy.enemyAnim.SetTrigger("ShieldState");
        Debug.Log("Shielding");
        waitbeforeShield = 2.5f;
		_enemy.agent.isStopped = true;
		_enemy.agent.updateRotation = false;
	}

	public override Type ExecuteState()
	{
        
        if (_enemy.lowHpEnemy.Count > 0 && _enemy.canShield)
        {
			if (_enemy.lowHpEnemy[0] == null)
			{
				return typeof(NannyIdleState);
			}

            Quaternion lookOnLook = Quaternion.LookRotation(_enemy.lowHpEnemy[0].transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 5);
			waitbeforeShield -= Time.deltaTime;
            if (!_enemy.lowHpEnemy[0].isShielded)
			{
                
                if (waitbeforeShield <= 0f)
				{ 
                    _enemy.lowHpEnemy[0].AddShield();
                    _enemy.canShield = false;
					_enemy.ResetShield();
                }
				return null;
				
            }
        }
		return typeof(NannyIdleState);

    }
}
