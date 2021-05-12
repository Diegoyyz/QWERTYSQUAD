using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectedState : CharacterState
{
    public EnemySelectedState(Entity character)
    {
        actor = character;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }
    public override void Tick()
    {
    }
}
