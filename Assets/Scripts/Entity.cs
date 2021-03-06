using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
public class Entity : MonoBehaviour
{
    //stats
    [SerializeField]
    protected int _speed;
    [SerializeField]
    public int attackDmg;
    [SerializeField]
    protected float _currentHP;
    [SerializeField]
    protected float _maxHP;
    [SerializeField]
    protected int _maxActions;
    [SerializeField]
    protected int _actionsLeft;
    [SerializeField]
    protected FSMState currentState;
    protected bool controllerActive = true;
    public bool isDead = false;
    FloorTile _currentTile;
    [SerializeField]
    protected Floor _currentNode;
    [SerializeField]
    protected Floor _targetNode;
    [SerializeField]
    protected int _attackRange;
    [SerializeField]
    public float attackRate;
    protected bool _isAttackable = false;
    public Animator anim;
    public GameObject body;
    [SerializeField]
    protected Image HealtBar;
    public bool onTheMoove;
    public enum Teams { Red, Blue, green };
    public Teams team;
    [SerializeField]
    protected Entity attackTarget;
    public bool isAttacking;
    public delegate void OnDeath(Entity body);
    public event OnDeath OnDeathEvent;
    public delegate void OnTurnStarts();
    public event OnTurnStarts OnTurnStartsEvent;
    public delegate void OnTurnEnds();
    public event OnTurnEnds OnTurnEndsEvent;
    public List<Floor> path;
    private void Start() 
    {
        _currentHP = _maxHP;
        OnTurnStartsEvent += () => { };
        OnTurnEndsEvent += () => { };
    }
    public virtual void TurnStart()
    {
        ActionsLeft = _maxActions;
        changeState(1);
    }
    public virtual void TurnEnds()
    {
    }
    public List<Floor> GetAttackableNodes(Floor root, int Range)
    {
        List<Floor> neighbours = new List<Floor>();
        var start = root;
        int AttackRange = Range;
        while (AttackRange > 0)
        {
            AttackRange--;
            foreach (var item in start.getNeighbours())
            {
                if (item.tile.Ocupant != null)
                {
                    item.MakeAttackable();
                    neighbours.Add(item);
                    GetAttackableNodes(item, AttackRange);
                }

            }
        }
        return neighbours;
    }
    public int AttackRange
    {
        get { return _attackRange; }
        set
        {
            if (_attackRange != value)
            {
                _attackRange = value;
            };
        }
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
    public Entity AttackTarget
    {
        get { return attackTarget; }
        set
        {
            if (attackTarget != value)
            {
                attackTarget = value;
            };
        }
    }
    public bool IsAttackable
    {
        get { return _isAttackable; }
        set
        {
            if (_isAttackable != value)
            {
                _isAttackable = value;
            };
        }
    }
    public int ActionsLeft
    {
        get { return _actionsLeft; }
        set
        {
            if (_actionsLeft != value)
            {
                _actionsLeft = value;
            };
        }
    }
    public virtual void MoveToTargetNode()
    {
        anim.SetBool("Walk Forward", true);
        StartCoroutine(MoveTo());
    }
    public virtual void Attack()
    {
        StartCoroutine("DelayAttackFeedback");
    }
    public void TakeDmg(int dmg)
    {
        StartCoroutine("DelayGetAttackedFeedback");
        _currentHP -= dmg;
        if (HealtBar != null)
        {
            HealtBar.fillAmount = HealtPorcentage();
        }
        if (_currentHP <= 0)
        {
            anim.SetBool("isDead", true);
            isDead = true;
            CurrentNode.ResetFloor();
            OnDeathEvent(this);
            Destroy(this.gameObject, 1);
        }
    }
    IEnumerator DelayAttackFeedback()
    {
        isAttacking = true;
        yield return new WaitForSecondsRealtime(1f);
        if (attackTarget != null)
        {
            anim.SetTrigger("PunchTrigger");
            ActionsLeft--;
            attackTarget.TakeDmg(attackDmg);
        }
        isAttacking = false;
        yield return new WaitForSecondsRealtime(1f);
        if (ActionsLeft <= 0)
        {
            OnTurnEndsEvent();
        }
        else changeState(1);
    }
    IEnumerator DelayGetAttackedFeedback()
    {
        yield return new WaitForSecondsRealtime(.5f);
        anim.SetTrigger("GetHitTrigger");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _currentTile = collision.gameObject.GetComponent<FloorTile>();
            _currentNode = collision.gameObject.GetComponent<Floor>();
            if (_currentTile != null)
            {
                CurrentNode = _currentTile._floorNode;
                _currentTile.Ocupied();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _currentTile.Empty();
            _currentTile.IsOcupied = false;
        }
    }
    public void ResetStats()
    {
        _actionsLeft = _maxActions;
    }
    private void Update()
    {
        if (currentState != null)
        {
            currentState.Tick();            
        }
    }
    public float HealtPorcentage()
    {
        return _currentHP / _maxHP;
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
    public virtual void changeState(int estado) { }
    public virtual void SetState(CharacterState state)
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
    protected IEnumerator MoveTo()
    {
        onTheMoove = true;
        if (path.Count == 0)
            yield break;
        Vector3 start = new Vector3(path[0].transform.position.x,
                                    transform.position.y,
                                    path[0].transform.position.z);
        for (int i = 1; i < path.Count; i++)
        {
            if (ActionsLeft > 0)
            {
                Vector3 end = new Vector3(path[i].transform.position.x,
                                        transform.position.y,
                                        path[i].transform.position.z);
                float t = 0f;
                body.transform.LookAt(new Vector3(path[i].transform.position.x,
                                        transform.position.y,
                                        path[i].transform.position.z));
                while (t < 1f)
                {
                    t += Time.deltaTime;
                    transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
                    yield return null;
                }
                ActionsLeft--;                
                start = end;               
            }
        }
        onTheMoove = false;
    }
}
