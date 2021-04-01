using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    GameObject _canvas;
    [SerializeField]
    protected CharacterState currentState;
    public enum states : byte { Idle, Attack, Move};



    private void OnEnable()
    {
        _canvas.SetActive(false);
    }
    private void Start()
    {
        SetState(new CharacterStateIdle(this));
    }
    private void Update()
    {
        currentState.Tick();
    }
    private CharacterController OnMouseUp()
    {
        _canvas.SetActive(true);       
        return this;
    }
    public void changeState(states estado)
    {
        switch (estado)
        {
            case states.Idle:
                SetState(new CharacterStateIdle(this));
                break;
            case states.Attack:
                SetState(new CharacterStateIdle(this));
                break;
            case states.Move:
                SetState(new CharacterStateIdle(this));
                break;
        }
        
                
    }
    public void SetState(CharacterState state)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = state;
        gameObject.name = "Character fsm" + state.GetType().Name;
        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }
}
