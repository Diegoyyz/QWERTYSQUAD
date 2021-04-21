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
    private 
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
        _floorNode.OnResetFloor += unselected;
        _floorNode.onMakeAttackable += ocupied;
    }
    private void OnDisable()
    {
        _floorNode.OnMakeWalkeable -= Walkeable;
        _floorNode.OnResetFloor -= unselected;
        _floorNode.OnMakePath -= selected;
        _floorNode.OnMakeGoal -= goal;
        _floorNode.OnResetFloor -= unselected;
        _floorNode.onMakeAttackable -= ocupied;
    }
    void Awake()
    {
        center = GetComponentInChildren<BoxCollider>();
        _floorNode = GetComponentInChildren<Floor>();
        Indicator = GetComponentInChildren<Button>();
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
