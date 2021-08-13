using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
public class CharacterController : Entity
{
    [SerializeField]
    protected GameObject _canvas;   
    public Button okMove;
    protected bool okMoveActive = true;
    public Button okAttack;
    protected bool okAttackActive = true;
     void OnEnable()
    {
        toggleOkMove();
        toggleOkAttack();
        controllerOff();        
        SetState(new CharacterStateIdle(this));
    }
    public override void TurnStart()
    {
        base.TurnStart();
        changeState(1);
        controllerOn();
    }
    public override void TurnEnds()
    {
        controllerOff();
    }

    public override void SetState(CharacterState state)
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
    public override void changeState(int estado)
    {
        switch (estado)
        {
            //0 idle
            //1 selected
            //2 move
            //3 attack
            case 0:
                SetState(new CharacterStateIdle(this));
                break;
            case 1:
                SetState(new CharacterStateSelected(this));
                break;
            case 2:
                SetState(new CharacterStateMove(this));
                break;
            case 3:
                SetState(new CharacterStateAttack(this));
                break;  
        }
    }
    public override void MoveToTarget(List<Floor> path)
    {
        base.MoveToTarget(path);      
    }
    public void controllerOn()
    {
        if (_canvas != null)
        {
            controllerActive = true;
            _canvas.SetActive(true);
        }
    }
    public void controllerOff()
    {
        if (_canvas != null)
        {
            controllerActive = false;
            _canvas.SetActive(false);
        }
    }
    public void toggleOkMove()
    {
        if (okMove != null)
        {
            okMoveActive = !okMoveActive;
            okMove.gameObject.SetActive(okMoveActive);
        }
    }
    public void toggleOkAttack()
    {
        if (okAttack != null)
        {
            okAttackActive = !okAttackActive;
            okAttack.gameObject.SetActive(okAttackActive);
        }
    }     
}