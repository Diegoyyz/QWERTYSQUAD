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
        Empty,
        Ocupied,
        Walkeable,
        Attackable,
    }
    private void OnEnable()
    {
      
        _floorNode.onMakeAttackable += Attackable;
        _floorNode.OnMakeUnWalkeable += Walkeable;
        _floorNode.OnMakeOcupied += Ocupied;


    }
    private void OnDisable()
    {
     
        _floorNode.onMakeAttackable -= Attackable;
        _floorNode.OnMakeUnWalkeable -= Walkeable;
        _floorNode.OnMakeOcupied -= Ocupied;
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
            case States.Empty:
                Empty();
                break;
            case States.Ocupied:
                Ocupied();
                break;
            case States.Walkeable:
                Walkeable();
                break;
            case States.Attackable:
                Attackable();
                break;
            
        }
    }
    public void Ocupied()
    {
        Indicator.image.color = Color.yellow;
        currentState = States.Ocupied;
    }
    public void Attackable()
    {
        Indicator.image.color = Color.red;
        currentState = States.Attackable;
    }
    public void Empty()
    {
        Indicator.image.color = Color.white;
        currentState = States.Attackable;
    }
    public void Walkeable()
    {
        Indicator.image.color = Color.blue;
        currentState = States.Walkeable;
    }

}
