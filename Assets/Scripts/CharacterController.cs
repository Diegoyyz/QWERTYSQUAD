using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
public class CharacterController : Entity
{
    private void OnEnable()
    {
        ResetStats();
        toggleOkMove();
        toggleController();
        SetState(new CharacterStateIdle(this));
    }
    private CharacterController OnMouseUp()
    {
        SetState(new CharacterStateSelected(this));
        return this;
    }

    public void changeState(int estado)
    {
        switch (estado)
        {
            case 0:
                SetState(new CharacterStateAttack(this));
                break;
            case 1:
                SetState(new CharacterStateMove(this));
                break;
            case 2:
                SetState(new CharacterStateIdle(this));
                break;
            case 3:
                SetState(new CharacterStateSelected(this));
                break;
        }
    }   
}
