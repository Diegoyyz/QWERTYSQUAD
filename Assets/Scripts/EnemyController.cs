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
        gameObject.name = "PLAYER FMS " + state.GetType().Name;
        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
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

