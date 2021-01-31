using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class RotateAround : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Vector2 _centerPos, _lastPos, _newPos;
    private bool _down = false, _hold = false;

    private float _angle = 0f;

    private void Start()
    {
        _centerPos = (transform as RectTransform).position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _down = true;
        _newPos = eventData.position;
        _lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _hold = true;
        _newPos = eventData.position;
        Vector2 dirLast = _centerPos - _lastPos;
        Vector2 dirNew = _centerPos - _newPos;
        _angle += Vector2.SignedAngle(dirLast, dirNew);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _down = false;
        _hold = false;
    }

    private void Update()
    {
        if (_angle > 0.2f || _angle < -0.2f)
        {
            transform.Rotate(Vector3.forward, _angle * Time.deltaTime);
            if (_angle < 0f)
            {
                _angle += Time.deltaTime * 20f;
            }
            else
            {
                _angle -= Time.deltaTime * 20f;
            }
        }
    }
}