using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public interface ICollectableItem
{
    string GetKey();
}

public class CharacterItem : Item
{
    public const string ITEM_PREFIX = "Char";
    [SerializeField] private TextMesh _textMesh;
    [SerializeField] private BoxCollider2D _collider;

    public BoxCollider2D Collider => _collider;

    private void Start()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        _collider.enabled = false;
        transform.DOScale(scale, 1f).OnComplete(() =>
        {
            _collider.enabled = true;
        }).Play();
    }

    private char _character;
    public char Character
    {
        get
        {
            return _character;
        }
        set
        {
            _character = value;
            _textMesh.text = _character.ToString();
        }
    }

    public override string GetKey()
    {
        return $"{ITEM_PREFIX}_{_character}";
    }
}
