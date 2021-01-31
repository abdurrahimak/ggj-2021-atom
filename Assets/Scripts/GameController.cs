using CoreProject.Singleton;
using CoreProject.States;
using System;
using UnityEngine;

public class GameController : SingletonComponent<GameController>, IGameStateController
{
    public Cinemachine.CinemachineVirtualCamera Camera;
    private IGameState _subState;

    public void SwitchGameState(IGameState gameState)
    {
        _subState?.End();
        _subState = gameState;
        _subState.Begin();
    }

    private void Start()
    {
        Application.targetFrameRate = 30;
        SwitchGameState(new StoryState(this));
    }

    private void Update()
    {
        _subState?.Update();
    }

    private void FixedUpdate()
    {
        _subState?.FixedUpdate();
    }

    private void LateUpdate()
    {
        _subState?.LateUpdate();
    }
}