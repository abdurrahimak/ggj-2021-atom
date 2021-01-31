using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagChildItem : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Text _text;
    [SerializeField] private Image _checkmarkImage;

    [Header("Sprites")]
    [SerializeField] private Sprite _correctSprite;
    [SerializeField] private Sprite _uncorrectSprite;

    public bool _correct = false;
    public bool Correct
    {
        get { return _correct; }
        set
        {
            _correct = value;
            if (_correct)
            {
                _backgroundImage.sprite = _correctSprite;
                _checkmarkImage.enabled = true;
            }
            else
            {
                _backgroundImage.sprite = _uncorrectSprite;
                _checkmarkImage.enabled = false;
            }
        }
    }

    public bool _show = false;
    public bool Show
    {
        get { return _show; }
        set
        {
            _show = value;
            _backgroundImage.enabled = _show;
            _checkmarkImage.enabled = _show && _correct;
            _text.gameObject.SetActive(_show);
        }
    }

    public Text Text => _text;
    public Image Checkmark => _checkmarkImage;
    public Image BackgroundImage => _backgroundImage;
}
