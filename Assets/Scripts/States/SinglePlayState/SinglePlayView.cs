using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SinglePlayView : MonoBehaviour
{
    [SerializeField] private Transform _bagParent;
    [SerializeField] private GameObject _bagChildPrefab;
    [SerializeField] private GameObject _bagChildBetweenPrefab;

    [SerializeField] private Color _trueTextColor;
    [SerializeField] private Color _falseTextColor;

    [SerializeField] private Sprite _trueBackgroundSprite;
    [SerializeField] private Sprite _falseBackgroundSprite;

    [Header("End Game")]
    [SerializeField] private Button _againButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Image _endGameImage;


    private List<BagChildItem> _bagChildItems = new List<BagChildItem>();
    private List<Image> _arrows = new List<Image>();
    private LevelData _levelData;
    public Transform RightBox => _bagChildItems[_bagChildItems.Count - 1].transform;

    public event Action ReturnToMain;
    public event Action PlayAgain;

    private void Start()
    {
        _againButton.onClick.AddListener(OnClick_Again);
        _menuButton.onClick.AddListener(OnClick_Menu);
        _homeButton.onClick.AddListener(OnClick_Menu);
    }

    private void OnClick_Menu()
    {
        ReturnToMain?.Invoke();
    }

    private void OnClick_Again()
    {
        PlayAgain?.Invoke();
    }

    public void Init(LevelData levelData)
    {
        _levelData = levelData;
        for (int i = 0; i < _levelData.BagSize; i++)
        {
            BagChildItem bagChildItem = GameObject.Instantiate(_bagChildPrefab, _bagParent).GetComponentInChildren<BagChildItem>();
            bagChildItem.Text.text = "";
            _bagChildItems.Add(bagChildItem);
            if (i < _levelData.BagSize - 1)
            {
                _arrows.Add(GameObject.Instantiate(_bagChildBetweenPrefab, _bagParent).GetComponentInChildren<Image>());
            }
        }
    }

    public void UpdateBag(List<string> bagItems)
    {
        string text = "";
        int k = _levelData.BagSize - bagItems.Count;
        for (int i = 0; i < _levelData.BagSize; i++)
        {
            if (i >= k)
            {
                _bagChildItems[i].Correct = bagItems[i - k].Equals(_levelData.Keys[i]);
                if (i == 0)
                {
                    FromToOut();
                }
                _bagChildItems[i].Text.text = ItemFactory.Instance.GetValue(bagItems[i - k]);
            }
            else
            {
                _bagChildItems[i].Text.text = "";
                _bagChildItems[i].Correct = false;
            }
        }
        foreach (var item in bagItems)
        {
            text += item + ", ";
        }
    }

    internal void FromToOut()
    {
        Text fromText = _bagChildItems[0].Text;
        Vector3 fromPos = fromText.transform.position;
        Vector3 targetPos = Vector3.zero;

        GameObject go = GameObject.Instantiate(_bagChildPrefab, transform);
        go.transform.position = fromPos;
        Text text = go.GetComponentInChildren<Text>();
        text.text = fromText.text;
        text.color = fromText.color;

        targetPos = fromText.transform.position - new Vector3(200f, 0f, 0f);
        go.transform.DOMove(targetPos, 0.5f).OnUpdate(() =>
        {

        }).OnComplete(() =>
        {
            GameObject.Destroy(go);
        }).Play();
    }

    internal void CollectedItem(CharacterItem item)
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(item.transform.position);
        GameObject go = GameObject.Instantiate(_bagChildPrefab, transform);
        go.transform.position = pos;
        Text text = go.GetComponentInChildren<Text>();
        text.text = item.Character.ToString();
        _bagChildItems[_bagChildItems.Count - 1].Show = false;
        foreach (var arrow in _arrows)
        {
            Color color = arrow.color;
            color.a = 0.1f;
            arrow.DOColor(color, 0.25f).OnComplete(() =>
            {
                color.a = 1f;
                arrow.DOColor(color, 0.25f).Play();
            }).Play();
        }
        go.transform.DOMove(RightBox.transform.position, 1f).OnComplete(() =>
        {
            _bagChildItems[_bagChildItems.Count - 1].Show = true;
            GameObject.Destroy(go);
        }).Play();
    }

    public void ShowEndGame()
    {
        _endGameImage.gameObject.SetActive(true);
        Color color = _endGameImage.color;
        color.a = 0f;
        _endGameImage.color = color;
        color.a = 0.9f;
        _endGameImage.DOColor(color, 1f).OnComplete(() =>
        {
            AudioManager.Instance.PlayWinSound();
        });
    }
}
