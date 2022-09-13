using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class SnapPoint : MonoBehaviour
{
    [SerializeField] private GameObject _activeSnapPoint;
    [SerializeField] private Joint _joint;

    private bool _selectable;


    private void Start()
    {
        _selectable = false;
    }
    private void OnMouseDown()
    {
        if(_selectable == false)
        {
            _activeSnapPoint.SetActive(true);
            _selectable = true;
            _joint.AddJoint(gameObject);
        }
        else
        {
            _activeSnapPoint.SetActive(false);
            _selectable = false;
            _joint.RemoveJoint(gameObject);
        }
    }
}



