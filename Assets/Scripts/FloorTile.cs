using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class FloorTile : MonoBehaviour
{
    public Transform center;
    [SerializeField]
    private int _cost;
    [SerializeField]
    private States currentState;
    public Floor _floorNode;
    public Button Indicator;
    [SerializeField]
    private Button okButton;
    [SerializeField]
    Entity _ocupant;
    public bool _ocupied;
    public delegate void TogleWalkeable();
    public event TogleWalkeable OnTogleWalkeable;
    void deselect()
    {
        _ocupant = null;
    }
    public Entity Ocupant
    {
        get { return _ocupant; }
        set
        {
            if (_ocupant != value)
            {
                _ocupant = value;
            };
        }
    }
    public bool IsOcupied
    {
        get { return _ocupied; }
        set
        {
            if (_ocupied != value)
            {
                _ocupied = value;
            };
        }
    }

    enum States
    {
        Current,
        Walkeable,
        Selected,
        Goal,
        Idle,
        UnSelected,
        Ocupied
    }   
    private void OnEnable()
    {
        _floorNode.OnResetFloor += Unselected;
        _floorNode.OnMakePath += Selected;
        _floorNode.OnMakeGoal += Goal;
        _floorNode.onMakeAttackable += Attackable;

    }
    private void OnDisable()
    {
        _floorNode.OnResetFloor -= Unselected;
        _floorNode.OnMakePath -= Selected;
        _floorNode.OnMakeGoal -= Goal;
        _floorNode.onMakeAttackable -= Attackable;

    }
    void Awake()
    {
        center = GetComponentInChildren<Transform>();
        _floorNode = GetComponentInChildren<Floor>();
        Indicator = GetComponentInChildren<Button>();
    }  
     private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Entity")
        {
            Ocupant = collision.collider.GetComponent<Entity>();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Entity")
        {
            Unselected();
            deselect();
            OnTogleWalkeable();
        }
    }
    public Floor FloorNode
    {
        get { return _floorNode; }
        set
        {
            if (_floorNode != value)
            {
                _floorNode = value;
            };
        }
    }
    public int Cost
    {
        get { return _cost; }
        set
        {
            if (_cost != value)
            {
                _cost = value;
            };
        }
    }     
    private void seetVisaul(States currentState)
    {
        switch (currentState)
        {
            case States.Current:
                IsCurrent();
                break;            
            case States.Ocupied:
                Goal();
                break;
            case States.Selected:
                Selected();
                break;
            case States.Idle:
                Idle();
                break;
        }
    }
   
    public void IsCurrent()
    {
        Indicator.image.color = Color.yellow;
        IsOcupied = true;
        currentState = States.Current;
        OnTogleWalkeable();
    }
    public void Attackable()
    {
        Indicator.image.color = Color.red;
        currentState = States.Goal;
    }
    public void Unselected()
    {
        Indicator.image.color = Color.white;
        currentState = States.UnSelected;
    }   
    public void Goal()
    {
        Indicator.image.color = Color.magenta;
        currentState = States.Ocupied;
    }
    public void Selected()
    {
        Indicator.image.color = Color.blue;
        currentState = States.Selected;
    }
    public void Idle()
    {
        Indicator.image.color = Color.white;
        currentState = States.Selected;
    }
}
