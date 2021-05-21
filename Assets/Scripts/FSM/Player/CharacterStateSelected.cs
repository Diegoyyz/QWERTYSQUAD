using System.Collections.Generic;
using util;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class CharacterStateSelected : CharacterState
{

    public CharacterStateSelected(CharacterController character)
    {
        actor = character;
        actor.ResetStats();
    }
    // Start is called before the first frame update
    public override void Tick()
    {
    }
    public override void OnStateExit()
    {

    }
    public override void OnStateEnter()
    {
        if (actor.ActionsLeft>0)
        {
            actor.controllerOn();
        }
    }
}
