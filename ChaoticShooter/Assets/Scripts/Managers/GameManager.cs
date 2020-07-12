using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState currentState;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<GameStateCompletedEvent>(OnGameStateCompleted);
        GameEventManager.Instance.AddListener<RestartGameEvent>(OnGameRestarted);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<GameStateCompletedEvent>(OnGameStateCompleted);
        GameEventManager.Instance.RemoveListener<RestartGameEvent>(OnGameRestarted);
    }

    void Start()
    {
        SetState(GameState.LevelGeneration);
    }

    void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// Set any initial parameters here
    /// </summary>
    /// <param name="nextState"></param>
    void SetState(GameState nextState)
    {
        if (currentState == nextState)
            return;

        currentState = nextState;

        GameEventManager.Instance.TriggerSyncEvent(new GameStateSetEvent(currentState));
    }

    /// <summary>
    /// Updates the current ongoing state
    /// </summary>
    void UpdateState()
    {
        switch (currentState)
        {
            case GameState.LevelGeneration:
                break;
            case GameState.RoulettePlayerWheel:
                break;
            case GameState.Gameplay:
                break;
            case GameState.LevelComplete:
                break;
            case GameState.LevelFailed:
                break;
        }
    }

    private void OnGameStateCompleted(GameStateCompletedEvent e)
    {
        switch (currentState)
        {
            case GameState.LevelGeneration:
                SetState(GameState.RoulettePlayerWheel);
                break;
            case GameState.RoulettePlayerWheel:
                SetState(GameState.Gameplay);
                break;
            case GameState.Gameplay:
                if (!(bool)e.param)
                    SetState(GameState.LevelFailed);
                else
                    SetState(GameState.LevelComplete);
                break;
            case GameState.LevelComplete:
                break;
            case GameState.LevelFailed:
                break;
        }
    }

    private void OnGameRestarted(RestartGameEvent e)
    {
        SetState(GameState.LevelGeneration);
    }

}
