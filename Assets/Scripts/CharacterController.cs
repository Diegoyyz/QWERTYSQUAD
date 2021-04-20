using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    GameObject _canvas;
    [SerializeField]
    protected CharacterState currentState;
    bool controllerActive = true;
    FloorTile _currentTile;
    [SerializeField]
    Floor _currentNode;
    [SerializeField]
    Floor _targetNode;
    [SerializeField]
    private int _maxSpeed;
    [SerializeField]
    private int _speedLeft;
    public Button okMove;
    bool okMoveActive= true;
    public void MoveToTarget()
    {
        transform.position = new Vector3(_targetNode.transform.position.x, transform.position.y,_targetNode.transform.position.z);       
    }
    public void ResetStats()
    {
        _speedLeft = _maxSpeed;

    }
    private void OnEnable()
    {
        ResetStats();
        toggleOkMove();
        toggleController();
        SetState(new CharacterStateIdle(this));
    }
    public int SpeedLeft
    {
        get { return _speedLeft; }
        set
        {
            if (_speedLeft != value)
            {
                _speedLeft = value;
            };
        }
    }
    public Floor TargetNode
    {
        get { return _targetNode; }
        set
        {
            if (_targetNode != value)
            {
                _targetNode = value;
            };
        }
    }
    public void setTargetNode()
    {

    }
    public Floor CurrentNode
    {
        get { return _currentNode; }
        set
        {
            if (_currentNode != value)
            {
                _currentNode = value;
            };
        }
    }
    public void toggleController()
    {
        controllerActive = !controllerActive;
        _canvas.SetActive(controllerActive);
    }
    public void toggleOkMove()
    {
        okMoveActive = !okMoveActive;
        okMove.gameObject.SetActive(okMoveActive);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Floor")
        {
            _currentTile = collision.gameObject.GetComponent<FloorTile>();
            if (_currentTile!=null)
            {
                CurrentNode = _currentTile._floorNode;
                _currentTile.isCurrent();
            }                                           
        }
    }   
    private void Update()
    {
        currentState.Tick();
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
