using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyController character)
    {
        actor = character;
    }
    public override void Tick()
    {
    }
    public override void OnStateEnter()
    {
        
    }
    public override void OnStateExit()
    {

    }
}
