using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _stars;
    [SerializeField] private SpriteRenderer _planet;

    [SerializeField] private float _line = 3f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _starsSpeed = 1f;
    [SerializeField] private float _planetSpeed = 0.5f;

    private Vector3 _starsTarget, _planetTarget;
    private bool _right = false;

    void Start()
    {
        _starsTarget = _stars.transform.position - Vector3.left * _radius;
        _planetTarget = _planet.transform.position - Vector3.left * _line;
    }

    // Update is called once per frame
    void Update()
    {
        _planet.transform.RotateAround(_planetTarget, Vector3.forward, Time.deltaTime * _planetSpeed);
        _stars.transform.RotateAround(_starsTarget, Vector3.forward, Time.deltaTime * _starsSpeed);
    }
}
