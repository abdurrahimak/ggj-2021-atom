using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Ship : BaseAudioComponent
{
    [SerializeField] private Transform _targetOffset;
    [SerializeField] private SpriteRenderer _lightRenderer;

    Sequence mySequence = DOTween.Sequence();
    Vector3 _offset, _lastPos;

    private void Awake()
    {
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.2f, 0f));
        pos.z = 0f;
        transform.position = pos;
        Debug.Log(transform.position);
        _lightRenderer.enabled = false;
        _offset = transform.position - _targetOffset.position;
        base.InitializeAudioComp();
    }

    private void Start()
    {
        AudioSource.clip = AudioManager.Instance.ShipMovement;
        AudioSource.Play();
    }

    public void MovePosition(Vector3 pos)
    {
        Vector3 targetPos = pos + _offset;
        _lastPos = targetPos;
        mySequence.Append(transform.DOMove(targetPos, 1f));
    }

    public void LightsOpen()
    {
        _lightRenderer.enabled = true;
        Color nColor = _lightRenderer.color;
        Color aColor = _lightRenderer.color;
        aColor.a = 0.3f;
        mySequence.PrependInterval(1);
        mySequence.AppendCallback(() =>
        {
            AudioSource.clip = AudioManager.Instance.ShipTeleport;
            AudioSource.Play();
        });
        mySequence.Append(_lightRenderer.DOColor(aColor, 0.5f));
        mySequence.Append(_lightRenderer.DOColor(nColor, 0.2f));
        mySequence.Append(_lightRenderer.DOColor(aColor, 0.2f));
        mySequence.Append(_lightRenderer.DOColor(nColor, 0.2f));
        mySequence.Append(_lightRenderer.DOColor(aColor, 0.2f));
        mySequence.Append(_lightRenderer.DOColor(nColor, 0.2f));
    }

    public void GetTransformToShip(Transform playerTransform)
    {
        mySequence.Append(playerTransform.DOMove(_lastPos, 2f));
        mySequence.Join(playerTransform.DOScale(new Vector3(0.1f, 0.1f, 0), 2f));
    }

    public void StartSequence(Action callback)
    {
        mySequence.OnComplete(() => { callback.Invoke(); }).Play();
    }
}
