using System.Collections;
using System.Collections.Generic;
using D2D.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FingerCursor : MonoBehaviour
{
    [SerializeField] private Image _hand;
    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private bool _isAlwaysVisible;

    [Header("Animations")] 
    [SerializeField] private float _scaleDuration;
    [SerializeField] private float _scaleTo;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _fadeFrom;

    private Transform _handTransform;
    private Camera _mainCamera;
    private Vector3 _startScale;
    
    private void Start()
    {
        _mainCamera = this.Find<Camera>();
        _handTransform = _hand.transform;
        _startScale = transform.localScale;

        _hand.DOFade(0, 0f);
    }
    
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        _handTransform.position = mousePosition + _followOffset;

        if (Input.GetMouseButtonDown(0))
        {
            //_handTransform.DOScale(Vector3.one * 1f, 0.5f);
            /*if (_isAlwaysVisible)
                _hand.DOFade(1,)*/
            _hand.DOFade(1, _fadeDuration);
            _hand.transform.DOScale(_scaleTo, _scaleDuration);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _hand.DOFade(0, _fadeDuration);
            _hand.transform.DOScale(_startScale, _scaleDuration);
        }
    }
}
