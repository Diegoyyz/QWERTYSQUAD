using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : CharacterState
{

    public CharacterStateIdle(CharacterController character)
    {
        actor = character;
    }
    public override void OnStateEnter()
    {
        actor.TurnEnds();
        base.OnStateEnter();
        actor.ResetStats();
    }
    public override void Tick()
    {
    } 
}
