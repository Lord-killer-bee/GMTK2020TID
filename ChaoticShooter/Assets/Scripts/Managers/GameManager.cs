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
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<GameStateCompletedEvent>(OnGameStateCompleted);
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
            case GameState.EnemiesGeneration:
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
            case GameState.EnemiesGeneration:
                break;
            case GameState.Gameplay:
                break;
            case GameState.LevelComplete:
                break;
            case GameState.LevelFailed:
                break;
        }
    }

}
