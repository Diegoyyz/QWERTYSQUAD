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
    }
    // Start is called before the first frame update
    public override void Tick()
    {
        
    }
    public override void OnStateExit()
    {
       actor.ToggleController();

    }
    public override void OnStateEnter()
    {
        actor.ToggleController();
    }
}
