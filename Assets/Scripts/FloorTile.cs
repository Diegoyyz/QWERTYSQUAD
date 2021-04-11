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
        _floorNode.OnMakePath += selected;
    }
    private void OnDisable()
    {
        _floorNode.OnMakeWalkeable -= Walkeable;
        _floorNode.OnMakePath -= selected;
    }
    void Awake()
    {
        center = GetComponentInChildren<BoxCollider>();
        _floorNode = GetComponentInChildren<Floor>();
        Indicator = GetComponentInChildren<Button>();
    }
    public void addCallbackToButton(CharacterController actor)
    {

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
        Indicator.image.color = Color.green;
        currentState = States.Walkeable;
    }
    public void isCurrent()
    {
        Indicator.image.color = Color.yellow;
        currentState = States.Current;
    }
    private void unselected()
    {
        Indicator.image.color = Color.white;
        currentState = States.UnSelected;
    }
    private void ocupied()
    {
        Indicator.image.color = Color.red;
        currentState = States.Ocupied;
    }
    private void selected()
    {
        Indicator.image.color = Color.blue;
        currentState = States.Selected;
    }   
}
