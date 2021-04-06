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
    Floor currentTile; 
    public enum states : byte { Attack, Move, Idle };
    private void OnEnable()
    {
        _canvas.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Floor")
        {
            currentTile = collision.gameObject.GetComponent<Floor>();
            currentTile.isCurrent();
        }
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
        }
    }
    public void SetState(CharacterState state)
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
}
