using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    protected int _actionsLeft;
    [SerializeField]
    protected GameObject _canvas;
    [SerializeField]
    protected CharacterState currentState;
    protected bool controllerActive = true;
    public bool isDead = false;
    FloorTile _currentTile;
    [SerializeField]
    protected Floor _currentNode;
    [SerializeField]
    protected Floor _targetNode;
    [SerializeField]
    protected int _attackRange;
    public Button okMove;
    protected bool okMoveActive = true;
    public Button okAttack;
    protected bool okAttackActive = true;
    protected bool _isAttackable = false;
    public Animator anim;
    public GameObject body;
    [SerializeField]
    protected Image HealtBar;
    public enum Teams {Red,Blue,green};
    public Teams team;
    protected Entity attackTarget;
    private void OnEnable()
    {
        ResetStats();
        toggleOkMove();
        toggleOkAttack();
        controllerOff();
        SetState(new CharacterStateIdle(this));
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
    public void MoveToTarget(List<Floor> path)
    {
        anim.SetBool("Walk Forward", true);
        StartCoroutine(moveTo(transform, path));
    }   
    public void Attack()
    {
        if (attackTarget!=null)
        {
            anim.SetTrigger("PunchTrigger");
            attackTarget.TakeDmg(50);
        }
        ActionsLeft--;
    }
    public void TakeDmg(int dmg)
    {
        StartCoroutine("DelayAttackFeedback");
        _currentHP-=dmg;        
        if (HealtBar != null)
        {
            HealtBar.fillAmount = HealtPorcentage();
        }
        if (_currentHP<=0)
        {
            anim.SetBool("isDead", true);
            isDead = true;
        }
    }
    IEnumerator DelayAttackFeedback()
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
                _currentTile.isCurrent();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {                     
            _currentTile.unselected();
            _currentTile.IsOcupied = false;
        }
    }
    public void ResetStats()
    {
       _actionsLeft = _maxActions;
        _currentHP = _maxHP;

    }
    private void Update()
    {
        if (currentState !=null)
        {
            currentState.Tick();
        }       
    }
    public float HealtPorcentage()
    {
        return _currentHP /_maxHP;
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
    public void controllerOn()
    {
        if (_canvas!=null)
        {
            controllerActive = true;
            _canvas.SetActive(true);
        }       
    }
    public void controllerOff()
    {
        if (_canvas != null)
        {
            controllerActive = false;
            _canvas.SetActive(false);
        }
    }
    public void toggleOkMove()
    {
        if (okMove != null)
        {
            okMoveActive = !okMoveActive;
            okMove.gameObject.SetActive(okMoveActive);
        }
    }
    public void toggleOkAttack()
    {
        if (okAttack != null)
        {
            okAttackActive = !okAttackActive;
            okAttack.gameObject.SetActive(okAttackActive);
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
    private IEnumerator moveTo(Transform transform, List<Floor> vectors)
    {
        if (vectors.Count == 0)
            yield break;
        Vector3 start = new Vector3(vectors[0].transform.position.x,
                                    transform.position.y,
                                    vectors[0].transform.position.z);
        for (int i = 1; i < vectors.Count; i++)
        {
            Vector3 end = new Vector3(vectors[i].transform.position.x,
                                    transform.position.y,
                                    vectors[i].transform.position.z);
            float t = 0f;
            body.transform.LookAt(new Vector3(vectors[i].transform.position.x,
                                    transform.position.y,
                                    vectors[i].transform.position.z));
            while (t < 1f)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
                yield return null;
            }
            ActionsLeft--;
            start = end;
        }
        anim.SetBool("Walk Forward", false);
        if (ActionsLeft > 0)
        {
            controllerOn();
        }
    }
}
