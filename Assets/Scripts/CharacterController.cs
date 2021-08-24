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
    public Button okAttack;
    protected bool okAttackActive = true;
     void OnEnable()
    {
        toggleOkAttack();
        ToggleController();
        SetState(new CharacterStateIdle(this));
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
    public void MoveToTargetNode(List<Floor> path)
    {
        this.path = path;
        anim.SetBool("Walk Forward", true);
        StartCoroutine(MoveTo());       
    }
    public void ToggleController()
    {
        if (_canvas != null)
        {
            controllerActive = !controllerActive;
            _canvas.gameObject.SetActive(controllerActive);
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