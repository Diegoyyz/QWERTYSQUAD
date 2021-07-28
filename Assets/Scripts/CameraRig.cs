using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour 
{
	public float speed = 3f;
	[SerializeField]
	public Transform _follow;
	Transform _transform;

	void Awake ()
	{
		_transform = transform;
	}

	void Update ()
	{
        if (_follow!=null)
        {
			_transform.position = Vector3.Lerp(_transform.position, _follow.position, speed * Time.deltaTime);
		}
	}
}
