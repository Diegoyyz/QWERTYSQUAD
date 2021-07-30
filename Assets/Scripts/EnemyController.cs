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
    public override void TurnStart()
    {
        base.TurnStart();
        changeState(2);
    }
    public override void Attack()
    {
        isAttacking = true;
        base.Attack();
    }
    public void changeState(int estado)
    {
        //0 idle
        //1 selected
        //2 move
        //3 attack
        switch (estado)
        {
            case 0:
                SetState(new EnemyIdleState(this));
                break;
            case 1:
                SetState(new EnemySelectedState(this));
                break;
            case 2:
                SetState(new EnemyMoveState(this));
                break;
            case 3:
                SetState(new EnemyAttackState(this));
                break;

        }
    }
}