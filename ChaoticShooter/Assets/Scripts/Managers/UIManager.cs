using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<GameStateSetEvent>(OnGameStateSet);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<GameStateSetEvent>(OnGameStateSet);
    }

    private void OnGameStateSet(GameStateSetEvent e)
    {
        switch (e.gameState)
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
                gameOverPanel.SetActive(true);
                break;
        };
    }

    #region Button listeners

    public void RestartGame()
    {
        GameEventManager.Instance.TriggerSyncEvent(new RestartGameEvent());
        gameOverPanel.SetActive(false);
    }

    #endregion
}
