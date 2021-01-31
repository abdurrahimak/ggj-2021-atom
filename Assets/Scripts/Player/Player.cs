using System;
using System.Collections;
using System.Collections.Generic;
using CoreProject.Resource;
using DG.Tweening;
using UnityEngine;

public enum AnimType
{
    Idle,
    Run,
}

[Serializable]
public class PlayerAnimSprites
{
    public List<Sprite> RunRightSprites;
    public List<Sprite> RunLeftSprites;
    public List<Sprite> IdleSprites;
}

[Serializable]
public class CharacterData
{
    public string Name;
    public PlayerAnimSprites AnimSprites;
}

public class Player : BaseAudioComponent
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private AudioSource _movementAudioSource;

    private bool _canMove = true;
    public bool CanMove { get => _canMove; set => _canMove = value; }

    public event Action<CharacterItem> CollectedItem;
    public event Action<Collider2D> ColliderEnterEvent;
    public event Action<Player, List<string>> BagQueueUpdated;
    public event Action<Player, string> BagItemRemoved;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private CharacterData _characterData;
    private Vector2 _input = Vector2.zero;
    private Bag<string> _bag;

    private void Start()
    {
        base.InitializeAudioComp();
        _movementAudioSource.clip = AudioManager.Instance.PlayerMovementSound;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

#if UNITY_ANDROID || UNITY_IOS
        GameObject go = GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("JoystickView"), transform);
        JoystickController joystickController = go.GetComponentInChildren<JoystickController>();
        joystickController.AxisChanged += JoystickController_AxisChanged;
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    private void JoystickController_AxisChanged(Vector2 axis)
    {
        _input = axis;
    }
#endif

    private float movementSoundInterval = 0f;
    void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _input.x = horizontal;
        _input.y = vertical;
#endif
        _animator.SetFloat("MoveSpeed", Mathf.Abs(_input.magnitude));
    }

    private void FixedUpdate()
    {
        if (_canMove)
            _rb2d.MovePosition(_rb2d.position + _input * _movementSpeed * Time.fixedDeltaTime);
    }

    public void CreateCharacterData(string characterName)
    {
        _characterData = ResourceManager.Instance.GetResource<CharacterDatas>("CharacterDatas").GetCharacterData(characterName);
    }

    public void CreateBag(int bagSize)
    {
        _bag = new Bag<string>(bagSize);
        _bag.QueueUpdated += Bag_QueueUpdated;
        _bag.ItemRemoved += Bag_ItemRemoved;
    }

    private void Bag_ItemRemoved(string obj)
    {
        BagItemRemoved?.Invoke(this, obj);
    }

    private void Bag_QueueUpdated(List<string> obj)
    {
        BagQueueUpdated?.Invoke(this, obj);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_canMove)
            return;

        if (other.tag.Equals("Item"))
        {
            AudioSource.clip = AudioManager.Instance.CollectSound;
            AudioSource.Play();
            CharacterItem item = other.gameObject.GetComponent<CharacterItem>();
            item.Collider.enabled = false;
            string key = item.GetKey();
            _bag.AddItemKey(key);
            CollectedItem?.Invoke(item);
            GameObject.Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    private bool _lastRight = false;

    public void SetRunSprite(int order)
    {
        if (!_canMove)
            return;

        if (_input.x > 0f)
        {
            _spriteRenderer.sprite = _characterData.AnimSprites.RunRightSprites[order];
            _lastRight = true;
        }
        else if (_input.x < 0f)
        {
            _spriteRenderer.sprite = _characterData.AnimSprites.RunLeftSprites[order];
            _lastRight = false;
        }
        else
        {
            if (_lastRight)
            {
                _spriteRenderer.sprite = _characterData.AnimSprites.RunRightSprites[order];
            }
            else
            {
                _spriteRenderer.sprite = _characterData.AnimSprites.RunLeftSprites[order];
            }
        }

        if (order == 1)
        {
            _movementAudioSource.Play();
        }
    }

    public void SetIdleSprite(int order)
    {
        if (!_canMove)
            return;
        _spriteRenderer.sprite = _characterData.AnimSprites.IdleSprites[order];
    }
}
