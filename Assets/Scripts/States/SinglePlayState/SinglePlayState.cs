using UnityEngine;
using CoreProject.States;
using System.Collections.Generic;
using System;
using DG.Tweening;
using CoreProject.Resource;
public class SinglePlayState : BaseGameState
{
    private Player _player;
    private SinglePlayView _view;
    private Transform _parentTransform;
    private List<Item> _items;
    private Map _map;
    private LevelData _levelData;
    private string _characterName;
    private LevelDifficult _levelDifficult;

    public SinglePlayState(IGameStateController parentGameStateController, string name, LevelDifficult levelDifficult) : base(parentGameStateController)
    {
        _characterName = name;
        _parentTransform = new GameObject("SinglePlayer").transform;
        _items = new List<Item>();
        _levelData = ItemFactory.Instance.CreateLevelData(levelDifficult);
    }

    public override void Begin()
    {
        base.Begin();
        CreateView();
        LoadMap();
        LoadPlayer();
    }

    public override void End()
    {
        base.End();
        DestroyView();
        UnloadPlayer();
        UnloadMap();
        GameObject.Destroy(_parentTransform.gameObject);
    }

    private void CreateView()
    {
        _view = StateFactory.Instance.CreateSinglePlayView(_parentTransform);
        _view.Init(_levelData);
        _view.ReturnToMain += View_ReturnToMain;
        _view.PlayAgain += View_PlayAgain;
    }

    private void View_PlayAgain()
    {
        _parentGameStateController.SwitchGameState(new SinglePlayState(_parentGameStateController, _characterName, _levelDifficult));
    }

    private void View_ReturnToMain()
    {
        _parentGameStateController.SwitchGameState(new MenuState(_parentGameStateController));
    }

    private void DestroyView()
    {
        _view.ReturnToMain -= View_ReturnToMain;
        _view.PlayAgain -= View_PlayAgain;
        GameObject.Destroy(_view.gameObject);
    }

    private void LoadMap()
    {
        _map = (GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("Map"), _parentTransform)).GetComponent<Map>();

        foreach (var character in _levelData.Items)
        {
            Item item = ItemFactory.Instance.CreateCharacterItem(character, _parentTransform);
            item.transform.position = GetRandomPosition(5f);
            _items.Add(item);
        }

        for (int i = 0; i < 10; i++)
        {
            char random = Char.Parse(i.ToString());
            Item item = ItemFactory.Instance.CreateCharacterItem(random, _parentTransform);
            item.transform.position = GetRandomPosition(5f);
            _items.Add(item);
        }
    }

    private void UnloadMap()
    {
        foreach (var item in _items)
        {
            if (item)
                GameObject.Destroy(item.gameObject);
        }

        GameObject.Destroy(_map.gameObject);
    }

    private Vector3 GetRandomPosition(float radius)
    {
        Vector3 targetPos = Vector3.zero;
        bool tryAgain = false;
        do
        {
            tryAgain = false;
            targetPos = _map.GetRandomPosition(_map.StartPosition, radius);
            foreach (var item in _items)
            {
                if ((item.transform.position - targetPos).magnitude < 0.2f)
                {
                    tryAgain = true;
                    break;
                }
            }
        } while (tryAgain);
        return targetPos;
    }

    private void LoadPlayer()
    {
        _player = PlayerFactory.Instance.CreatePlayer();
        _player.transform.position = _map.StartPosition;
        _player.CreateBag(_levelData.BagSize);
        _player.CreateCharacterData(_characterName);

        _player.BagQueueUpdated += Player_BagQueueUpdated;
        _player.BagItemRemoved += Player_BagItemRemoved;
        _player.CollectedItem += Player_CollectedItem;

        GameController.Instance.Camera.Follow = _player.transform;
    }

    private void UnloadPlayer()
    {
        _player.BagQueueUpdated -= Player_BagQueueUpdated;
        _player.BagItemRemoved -= Player_BagItemRemoved;
        _player.CollectedItem -= Player_CollectedItem;
        GameObject.Destroy(_player.gameObject);
    }

    private void Player_CollectedItem(CharacterItem item)
    {
        _items.Remove(item);
        _view.CollectedItem(item);
    }

    private void Player_BagItemRemoved(Player player, string itemKey)
    {
        Item item = ItemFactory.Instance.CreateItem(itemKey, _parentTransform);
        item.transform.position = GetRandomPosition(2.5f);
        _items.Add(item);
    }

    private void Player_BagQueueUpdated(Player player, List<string> bagList)
    {
        _view?.UpdateBag(bagList);

        if (_levelData.BagSize == bagList.Count)
        {
            bool isEqual = true;
            for (int i = 0; i < bagList.Count; i++)
            {
                if (!_levelData.Keys[i].Equals(bagList[i]))
                {
                    isEqual = false;
                    break;
                }
            }
            if (isEqual)
            {
                FinishSequence();
                // _player.transform.DOScale(_player.transform.localScale * 2, 2f).OnComplete(() =>
                // {
                //     _parentGameStateController.SwitchGameState(new MenuState(_parentGameStateController));
                // }
                // ).Play();
            }
        }
    }

    private void FinishSequence()
    {
        _player.CanMove = false;
        GameController.Instance.Camera.Follow = null;
        Ship ship = GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("Ship"), _parentTransform).GetComponent<Ship>();
        ship.MovePosition(_map.StartPosition);
        ship.MovePosition(_player.transform.position);
        ship.LightsOpen();
        ship.GetTransformToShip(_player.transform);
        ship.StartSequence(() =>
        {
            _view.ShowEndGame();
        });
    }

    public override void Update()
    {
        base.Update();

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     FinishSequence();
        // }
    }
}
