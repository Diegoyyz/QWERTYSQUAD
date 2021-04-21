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
    protected int _speedLeft;
    public Button okMove;
    protected bool okMoveActive = true;
  
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
        StartCoroutine(moveTo(transform, path));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _currentTile = collision.gameObject.GetComponent<FloorTile>();
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
    }
    private void Update()
    {
        currentState.Tick();
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
        controllerActive = !controllerActive;
        _canvas.SetActive(controllerActive);
    }
    public void toggleOkMove()
    {
        okMoveActive = !okMoveActive;
        okMove.gameObject.SetActive(okMoveActive);
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
    public void setTargetNode()
    {

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
            while (t < 1f)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
                yield return null;
            }
            start = end;
        }
    }
}
