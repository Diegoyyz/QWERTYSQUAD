using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        base.OnStateEnter();
        actor.ResetStats();
    }
    public override void OnStateExit()
    {

    }
}
