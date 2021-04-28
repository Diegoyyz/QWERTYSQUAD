using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class FloorTile : MonoBehaviour
{
    public BoxCollider center;
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
        _floorNode.OnMakeWalkeable += Walkeable;
        _floorNode.OnResetFloor += unselected;
        _floorNode.OnMakePath += selected;
        _floorNode.OnMakeGoal += goal;
        _floorNode.onMakeAttackable += ocupied;
        _floorNode.onIsOcupied += isCurrent;
    }
    private void OnDisable()
    {
        _floorNode.OnMakeWalkeable -= Walkeable;
        _floorNode.OnResetFloor -= unselected;
        _floorNode.OnMakePath -= selected;
        _floorNode.OnMakeGoal -= goal;
        _floorNode.onMakeAttackable -= ocupied;
        _floorNode.onIsOcupied -= isCurrent;

    }
    void Awake()
    {
        center = GetComponentInChildren<BoxCollider>();
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
            unselected();
            deselect();
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
                isCurrent();
                break;
            case States.Walkeable:
                Walkeable();
                break;
            case States.Ocupied:
                goal();
                break;
            case States.Goal:
                ocupied();
                break;
            case States.Selected:
                selected();
                break;
            case States.Idle:
                Idle();
                break;
        }
    }
    public void Walkeable()
    {
        Indicator.image.color = Color.green;
        currentState = States.Walkeable;
    }
    public void isCurrent()
    {
        Indicator.image.color = Color.yellow;
        IsOcupied = true;
        currentState = States.Current;
    }
    public void unselected()
    {
        Indicator.image.color = Color.white;
        currentState = States.UnSelected;
    }
    public void ocupied()
    {
        Indicator.image.color = Color.red;
        currentState = States.Ocupied;
    }
    public void goal()
    {
        Indicator.image.color = Color.magenta;
        currentState = States.Ocupied;
    }
    public void selected()
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
