using DG.Tweening;
using Obi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Joint : MonoBehaviour
{
    [SerializeField] private float _spring;
    [SerializeField] private float _damping;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private GameObject ropePrefab;
    [SerializeField] private ObiSolver _obiSolver;
    [SerializeField] private float _accelerationSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private int _countChangePositions;

    private List<SpringJoint> _springJoints = new List<SpringJoint>();
    private SpringJoint _joint;
    private List<Rope> _ropeList = new List<Rope>();
    private Rope _rope;
    private Vector3 _position;
    private Coroutine _acceleration;
    private float _accelerationValue;

    public void AddJoint(GameObject targetPoint)
    {
        AddSpringJoint(targetPoint);
        AddRope(_position);
    }

    public void RemoveJoint(GameObject targetPoint)
    {
        RemoveSpringJoint(targetPoint);
    }

    private void AddSpringJoint(GameObject targetPoint)
    {
        _position = targetPoint.transform.position;
        _joint = gameObject.AddComponent<SpringJoint>();
        _joint.spring = _spring;
        StartAcceleration();
        _joint.damper = _damping;
        _joint.minDistance = _minDistance;
        _joint.maxDistance = _maxDistance;
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = _position;

        _springJoints.Add(_joint);
    }

    private void RemoveSpringJoint(GameObject targetPoint)
    {
        for (int i = 0; i < _springJoints.Count; i++)
        {
            if (_springJoints[i].connectedAnchor == targetPoint.transform.position)
            {
                StopAllCoroutines();
                _joint = _springJoints[i];
                RemoveRope(i);
                Destroy(_joint);
                _springJoints.RemoveAt(i);
            }
        }
    }

    private void AddRope(Vector3 targetPosition)
    {
        _rope = Instantiate(ropePrefab, _obiSolver.transform).GetComponent<Rope>();
        _rope.SetPointsPosition(gameObject.transform.position, _position);
        _ropeList.Add(_rope);
    }

    private void RemoveRope(int index)
    {
        for (int i = 0; i < _ropeList.Count; i++)
        {
            if (i == index)
            {
                _ropeList[i].Destroy();
                _ropeList.RemoveAt(i);
            }
        }
    }

    private void StartAcceleration()
    {
        StartCoroutine(Acceleration());
    }

    private IEnumerator Acceleration()
    {
        _accelerationValue = _accelerationSpeed;
        for (int i = 0; i < _countChangePositions; i++)
        {
            Debug.Log(i);
            while (_joint.spring != _spring - _accelerationValue)
            {
                _joint.spring = ChangeValue(_spring - _accelerationValue);
                yield return null; 
            }
            while (_joint.spring != _spring + _accelerationValue)
            {
                _joint.spring = ChangeValue(_spring + _accelerationValue);
                yield return null;
            }
            _accelerationValue -= 100;
        }
        yield break;
    }

    private float ChangeValue(float targetValue)
    {
        return Mathf.MoveTowards(_joint.spring, targetValue, _accelerationSpeed * Time.deltaTime * _speed);
    }
}

