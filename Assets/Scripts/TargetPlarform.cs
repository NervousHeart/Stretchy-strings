using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetPlarform : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _speedWinBar;
    [SerializeField] private float _speedCheckBar;
    [SerializeField] private Image _crossBar;
    [SerializeField] private Image _winBar;

    private float _targetValue;
    private float _time;
    private Coroutine _fillBar;

    private void Start()
    {
        _targetValue = 0;
        _time = 0;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out Joint ball)) 
        {
            _targetValue = 1;
            _time += Time.deltaTime;
            StartFillBar(_crossBar, _speedCheckBar);

            if (_time >= _delay)
                StartFillBar(_winBar, _speedWinBar);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _time = 0;
        if (other.TryGetComponent(out Joint ball))
        {
            _targetValue = 0;
            StartFillBar(_crossBar, _speedCheckBar);
        }
    }

    private void StartFillBar(Image bar, float speed)
    {
        if (_fillBar != null)
            StopCoroutine(_fillBar);
        _fillBar = (StartCoroutine(FillBar(bar, speed)));
    }

    private IEnumerator FillBar(Image bar, float speed)
    {
        while (bar.fillAmount != _targetValue)
        {
            bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, _targetValue, speed * Time.deltaTime);
            yield return null;
        }
    }
}
