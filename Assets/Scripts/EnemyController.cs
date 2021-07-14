using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Entity
{
    public void SetState(EnemyState state)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = state;
        gameObject.name = "Enemy FSM " + state.GetType().Name;
        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }
    public override void turnEnds()
    {
        SetState(new EnemyIdleState(this));
    }
    public override void turnStarts()
    {
        SetState(new EnemySelectedState(this));
        ResetStats();
    }
    public override void Attack()
    {
        isAttacking = true;
        base.Attack();
    }
    public void changeState(int estado)
    {
        switch (estado)
        {
            case 0:
                SetState(new EnemyMoveState(this));
                break;
            case 1:
                SetState(new EnemyAttackState(this));
                break;
            case 2:
                SetState(new EnemySelectedState(this));
                break;            
        }
    }
}

