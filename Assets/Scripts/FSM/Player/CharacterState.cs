using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState  
{
    protected Entity actor;

    public abstract void Tick();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
   
}
