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
    FloorTile _currentTile;
    [SerializeField]
    Floor _currentNode;
    [SerializeField]
    Floor _targetNode;
    [SerializeField]
    private int _speed;
    private void OnEnable()
    {
        controllerActive = true;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.down, out hit))
        //{
        //    _currentTile = hit.collider.GetComponent<FloorTile>();
        //}
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
