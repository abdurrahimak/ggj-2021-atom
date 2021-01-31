using UnityEngine;
using CoreProject.States;
using System;

public class MenuState : BaseGameState
{
    private MenuView _menuView;
    public MenuState(IGameStateController parentGameStateController) : base(parentGameStateController)
    {
    }

    public override void Begin()
    {
        base.Begin();
        _menuView = StateFactory.Instance.CreateMenuView();

        _menuView.PlaySinglePlayerEvent += OnPlaySinglePlayer;
    }

    private void OnPlaySinglePlayer(string name, LevelDifficult levelDifficult)
    {
        _parentGameStateController.SwitchGameState(new SinglePlayState(_parentGameStateController, name, levelDifficult));
    }

    public override void End()
    {
        base.End();
        _menuView.PlaySinglePlayerEvent -= OnPlaySinglePlayer;

        GameObject.Destroy(_menuView.gameObject);
        _menuView = null;
    }
}

public class StoryState : BaseGameState
{
    private StoryView _storyView;
    public StoryState(IGameStateController parentGameStateController) : base(parentGameStateController)
    {
    }

    public override void Begin()
    {
        base.Begin();

        _storyView = StateFactory.Instance.CreateStoryView();
        _storyView.SkipEvent += OnSkip;

    }

    public override void End()
    {
        base.End();
        _storyView.SkipEvent -= OnSkip;
        GameObject.Destroy(_storyView.gameObject);
        _storyView = null;
    }

    private void OnSkip()
    {
        _parentGameStateController.SwitchGameState(new MenuState(_parentGameStateController));
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void Update()
    {
        base.Update();
    }
}