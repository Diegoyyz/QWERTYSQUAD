using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public BoxCollider center;
    [SerializeField]
    private int _cost;
    private States currentState;
    public Floor _floorNode;
    MeshRenderer Indicator;
    enum States
    {
        Current,
        Walkeable,
        Selected,
        UnSelected,
        Ocupied
    }
    private void OnEnable()
    {
        _floorNode.OnMakeWalkeable += Walkeable;
    }
    private void OnDisable()
    {
        _floorNode.OnMakeWalkeable -= Walkeable;
    }
    void Awake()
    {
        center = GetComponentInChildren<BoxCollider>();
        _floorNode = GetComponentInChildren<Floor>();
        Indicator = GetComponent<MeshRenderer>();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            unselected();
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
    private void OnMouseOver()
    {
        if (currentState == States.Walkeable)
        {
            selected();
        }
    }
    private void OnMouseDown()
    {
        if (currentState == States.Selected)
        {
            ocupied();
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
                ocupied();
                break;
            case States.Selected:
                selected();
                break;
        }
    }
    public void Walkeable()
    {
        Indicator.material.color = Color.green;
        currentState = States.Walkeable;
    }
    public void isCurrent()
    {
        Indicator.material.color = Color.yellow;
        currentState = States.Current;

    }
    private void unselected()
    {
        Indicator.material.color = Color.white;
        currentState = States.UnSelected;

    }
    private void ocupied()
    {
        Indicator.material.color = Color.red;
        currentState = States.Ocupied;

    }
    private void selected()
    {
        Indicator.material.color = Color.blue;
        currentState = States.Selected;

    }   
}
