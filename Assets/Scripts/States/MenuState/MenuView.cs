using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuView : MonoBehaviour
{
    public Button PlaySinglePlayer;
    public Button RandomButton;
    public Button CloseButton;
    public Toggle SoundToggle;

    [Header("Difficult Selector")]
    [SerializeField] private Transform _difficultSelectorTransform;
    [SerializeField] private RectTransform _difficultCircleTransform;
    [SerializeField] private float _difficultCircleTurnSpeed = 0f;


    [Header("Difficult Selector")]
    [SerializeField] private Transform _characterSelectorTransform;
    [SerializeField] private RectTransform _characterCircleTransform;
    [SerializeField] private float _characterCircleTurnSpeed = 0f;

    private LevelDifficult _selectedLevelDifficult = LevelDifficult.Easy;
    private string _selectedCharacterName = "c1";

    public event Action<string, LevelDifficult> PlaySinglePlayerEvent;

    void Start()
    {
        PlaySinglePlayer.onClick.AddListener(OnClick_PlaySinglePlayer);
        RandomButton.onClick.AddListener(OnClick_Online);
        CloseButton.onClick.AddListener(OnClick_Close);
        foreach (var item in _characterCircleTransform.parent.GetComponentsInChildren<RectTransform>())
        {
            SetMiddleAnchor(item);
        }
        foreach (var item in _difficultCircleTransform.parent.GetComponentsInChildren<RectTransform>())
        {
            SetMiddleAnchor(item);
        }

        SoundToggle.onValueChanged.AddListener(SoundToggle_onValueChanged);
        SoundToggle.isOn = AudioManager.Instance.IsActive;
    }

    private void SoundToggle_onValueChanged(bool isOn)
    {
        AudioManager.Instance.IsActive = isOn;
    }

    private void OnClick_Close()
    {
        Application.Quit();
    }

    private void OnClick_Online()
    {
        _difficultCircleTurnSpeed = UnityEngine.Random.Range(90f, 150f);
        _characterCircleTurnSpeed = UnityEngine.Random.Range(100f, 200f);
    }

    private void OnClick_PlaySinglePlayer()
    {
        CheckLevelDifficult();
        CheckCharacterSelector();
        PlaySinglePlayerEvent?.Invoke(_selectedCharacterName, _selectedLevelDifficult);
    }


    private void Update()
    {
        if (_difficultCircleTurnSpeed > 0f)
        {
            _difficultCircleTransform.Rotate(Vector3.forward, Time.deltaTime * _difficultCircleTurnSpeed * 40f);
            _difficultCircleTurnSpeed -= Time.deltaTime * 40f;
        }


        if (_difficultCircleTurnSpeed > 0f)
        {
            _characterCircleTransform.Rotate(Vector3.forward, Time.deltaTime * _characterCircleTurnSpeed * 40f);
            _characterCircleTurnSpeed -= Time.deltaTime * 40f;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            CheckLevelDifficult();
            CheckCharacterSelector();
        }
    }

    private void CheckLevelDifficult()
    {
        float selectedMag = 10000f;
        foreach (var item in _difficultCircleTransform.GetComponentsInChildren<CircleChildDifficult>())
        {
            float magnitude = (_difficultSelectorTransform.position - item.transform.position).magnitude;
            if (magnitude < selectedMag)
            {
                selectedMag = magnitude;
                _selectedLevelDifficult = item.LevelDifficult;
            }
        }
    }

    private void CheckCharacterSelector()
    {
        float selectedMag = 10000f;
        foreach (var item in _characterCircleTransform.GetComponentsInChildren<CircleChildCharacter>())
        {
            float magnitude = (_characterSelectorTransform.position - item.transform.position).magnitude;
            if (magnitude < selectedMag)
            {
                selectedMag = magnitude;
                _selectedCharacterName = item.CharacterName;
            }
        }
    }

    private void SetMiddleAnchor(RectTransform transform)
    {
        Vector2 lastSizeDelta = new Vector2(transform.rect.width, transform.rect.height);
        Vector2 lastPos = transform.position;
        transform.anchorMin = Vector2.one * 0.5f;
        transform.anchorMax = Vector2.one * 0.5f;
        transform.sizeDelta = lastSizeDelta;
        transform.position = lastPos;
    }
}