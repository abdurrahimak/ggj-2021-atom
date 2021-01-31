using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class JoystickController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private RectTransform _handleRectTransform;
    [SerializeField] private float _maxRadiusPixel = 80f;

    private Vector2 _centerPos;
    private Vector2 _inputAxis = Vector2.zero;
    private PointerEventData _eventData;
    private bool _down = false;
    private bool _hold = false;

    public event Action<Vector2> AxisChanged;

    private void Start()
    {
        Vector2 lastSizeDelta = new Vector2(_backgroundRectTransform.rect.width, _backgroundRectTransform.rect.height);
        Vector2 lastPos = _backgroundRectTransform.position;
        _centerPos = lastPos + (lastSizeDelta / 2);
        Debug.Log(lastPos);
        _backgroundRectTransform.anchorMin = Vector2.zero;
        _backgroundRectTransform.anchorMax = Vector2.zero;
        _backgroundRectTransform.sizeDelta = lastSizeDelta;
        _backgroundRectTransform.position = lastPos;

        Vector2 lastSizeDelta2 = new Vector2(_handleRectTransform.rect.width, _handleRectTransform.rect.height);
        Vector2 lastPos2 = _handleRectTransform.position;
        _handleRectTransform.anchorMin = Vector2.one * 0.5f;
        _handleRectTransform.anchorMax = Vector2.one * 0.5f;
        _handleRectTransform.sizeDelta = lastSizeDelta2;
        _handleRectTransform.position = lastPos2;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _down = true;
        _eventData = eventData;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _hold = true;
        _eventData = eventData;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _down = false;
        _hold = false;
        _handleRectTransform.anchoredPosition = Vector2.zero;
        _inputAxis = Vector2.zero;
        AxisChanged?.Invoke(_inputAxis);
    }

    private void Update()
    {
        if (_down || _hold)
        {
            Vector2 pos = (_eventData.position - _centerPos);
            if (pos.magnitude > _maxRadiusPixel)
            {
                pos = pos.normalized * _maxRadiusPixel;
            }
            _handleRectTransform.anchoredPosition = pos;
            _inputAxis = pos / _maxRadiusPixel;
            AxisChanged?.Invoke(_inputAxis);
        }
    }
}
