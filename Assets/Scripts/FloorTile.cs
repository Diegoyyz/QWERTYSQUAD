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
        _floorNode.OnResetFloor += unselected;
        _floorNode.OnMakePath += selected;
        _floorNode.OnMakeGoal += goal;
    }
    private void OnDisable()
    {
        _floorNode.OnResetFloor -= unselected;
        _floorNode.OnMakePath -= selected;
        _floorNode.OnMakeGoal -= goal;
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
            OnTogleWalkeable();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Entity")
        {
            unselected();
            deselect();
            OnTogleWalkeable();
            Debug.Log("sale");
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
            case States.Ocupied:
                goal();
                break;
            case States.Selected:
                selected();
                break;
            case States.Idle:
                Idle();
                break;
        }
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
