using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject _startPosition;
    [SerializeField] private GameObject _endPosition;
    [SerializeField] private float _speed;

    private ObiRope _obiRope;
    private MeshRenderer _meshRenderer;
    private Joint _joint;

    private void Start()
    {
        _obiRope = GetComponent<ObiRope>();
        _joint = FindObjectOfType<Joint>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void LateUpdate()
    {
        _startPosition.transform.position = ChangePosition();
    }

    public void SetPointsPosition(Vector3 startPoint, Vector3 endPoint)
    {
        _startPosition.transform.position = startPoint;
        _endPosition.transform.position = endPoint;
    }

    private Vector3 ChangePosition()
    {
        var x = _startPosition.transform.position.x;
        var z = _startPosition.transform.position.z;
        x = Mathf.MoveTowards(x, _joint.transform.position.x, _speed * Time.deltaTime);
        z = Mathf.MoveTowards(z, _joint.transform.position.z, _speed * Time.deltaTime);
        return new Vector3(x, 0.5f, z);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
