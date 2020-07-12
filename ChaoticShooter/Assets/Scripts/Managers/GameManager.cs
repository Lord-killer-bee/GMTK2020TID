using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int scoreIncrementPerSecond = 1;
    [SerializeField] int scoreIncrementForEnemyKill = 1;

    private GameState currentState;
    private DateTime gameStartTime, deadTime;

    private DateTime scoreTime;

    private int score;
    private bool calculateScore = false;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<GameStateCompletedEvent>(OnGameStateCompleted);
        GameEventManager.Instance.AddListener<StartGameEvent>(OnGameStarted);
        GameEventManager.Instance.AddListener<PlayerDestroyedEvent>(OnPlayerDead);
        GameEventManager.Instance.AddListener<RestartGameEvent>(OnGameRestarted);
        GameEventManager.Instance.AddListener<EnemyKilledEvent>(OnEnemyKilled);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<GameStateCompletedEvent>(OnGameStateCompleted);
        GameEventManager.Instance.RemoveListener<StartGameEvent>(OnGameStarted);
        GameEventManager.Instance.RemoveListener<PlayerDestroyedEvent>(OnPlayerDead);
        GameEventManager.Instance.RemoveListener<RestartGameEvent>(OnGameRestarted);
        GameEventManager.Instance.RemoveListener<EnemyKilledEvent>(OnEnemyKilled);
    }

    void Update()
    {
        UpdateState();

        if (calculateScore)
        {
            if ((DateTime.Now - scoreTime).TotalMilliseconds >= 1000)
            {
                score += scoreIncrementPerSecond;
                scoreTime = DateTime.Now;

                GameEventManager.Instance.TriggerAsyncEvent(new UpdateScoreEvent(score));
                GameEventManager.Instance.TriggerAsyncEvent(new SecondPassed());
            }
        }
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

    private void OnGameStarted(StartGameEvent e)
    {
        SetState(GameState.LevelGeneration);

        score = 0;
        calculateScore = true;
        gameStartTime = DateTime.Now;
        scoreTime = DateTime.Now;
    }

    private void OnGameRestarted(RestartGameEvent e)
    {
        SetState(GameState.LevelGeneration);

        calculateScore = true;
    }

    private void OnPlayerDead(PlayerDestroyedEvent e)
    {
        deadTime = DateTime.Now;
        calculateScore = false;
    }

    private void OnEnemyKilled(EnemyKilledEvent e)
    {
        if (calculateScore)
        {
            score += scoreIncrementForEnemyKill;

            GameEventManager.Instance.TriggerAsyncEvent(new UpdateScoreEvent(score));
        }
    }

}
