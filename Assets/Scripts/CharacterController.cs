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
    bool controllerActive;
     Floor _currentTile;
    [SerializeField]
    private int _speed;
    private void OnEnable()
    {
        controllerActive = true;
        toggleController();
    }
    public int Speed
    {
        get { return _speed; }
        set
        {
            if (_speed != value)
            {
                _speed = value;
            };
        }
    }
    public Floor CurrentTile
    {
        get { return _currentTile; }
        set
        {
            if (_currentTile != value)
            {
                _currentTile = value;
            };
        }
    }
    public void toggleController()
    {
        controllerActive = !controllerActive;
        _canvas.SetActive(controllerActive);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Floor")
        {
            _currentTile = collision.gameObject.GetComponent<Floor>();
            _currentTile.isCurrent();
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
        toggleController();
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
