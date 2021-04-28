using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Entity : MonoBehaviour
{
    [SerializeField]
    protected GameObject _canvas;
    [SerializeField]
    protected CharacterState currentState;
    protected bool controllerActive = true;
    FloorTile _currentTile;
    [SerializeField]
    protected Floor _currentNode;
    [SerializeField]
    protected Floor _targetNode;
    [SerializeField]
    protected int _maxSpeed;
    [SerializeField]
    protected int _attackRange;
    [SerializeField]
    public int attackDmg;
    [SerializeField]
    protected int _speedLeft;
    public Button okMove;
    protected bool okMoveActive = true;
    public Button okAttack;
    protected bool okAttackActive = true;
    protected bool _isAttackable = false;
    public Animator anim;
    public GameObject body;
    [SerializeField]
    protected float  _currentHP;
    [SerializeField]
    protected float _maxtHP;
    [SerializeField]
    private Image HealtBar;
    public int team;
    public Entity attackTarget;

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
    private void Start()
    {
        ResetStats();
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
    public void TakeDmg(int dmg)
    {
        _currentHP -=dmg;
        anim.SetTrigger("GetHitTrigger");
    }
    public int SpeedLeft
    {
        get { return _speedLeft; }
        set
        {
            if (_speedLeft != value)
            {
                _speedLeft = value;
            };
        }
    }
    public void MoveToTarget(List<Floor> path)
    {
        toggleController();
        anim.SetBool("Walk Forward", true);
        StartCoroutine(moveTo(transform, path));
    }
    public void Attack()
    {
        if (attackTarget!=null)
        {
            anim.SetTrigger("PunchTrigger");
            attackTarget.TakeDmg(attackDmg);
        }
      
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
    public void ResetStats()
    {
        _speedLeft = _maxSpeed;
        _currentHP = _maxtHP;
    }
    private void Update()
    {
        if (currentState !=null)
        {
            currentState.Tick();
        }
        if (HealtBar != null)
        {
            HealtBar.fillAmount = HealtPorcentage();
        }

    }
    public float HealtPorcentage()
    {
        return _currentHP /_maxtHP;

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
    public void toggleController()
    {
        if (_canvas!=null)
        {
            controllerActive = !controllerActive;
            _canvas.SetActive(controllerActive);
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
            start = end;
        }
        anim.SetBool("Walk Forward", false);
    }
}
