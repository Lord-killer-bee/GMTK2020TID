using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text xpText;
    [SerializeField] private Text levelText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private GameObject upgradeText;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<GameStateSetEvent>(OnGameStateSet);
        GameEventManager.Instance.AddListener<UpdateScoreEvent>(OnUpdateScore);
        GameEventManager.Instance.AddListener<UpdateXPEvent>(OnUpdateXP);
        GameEventManager.Instance.AddListener<UpgradeGranted>(OnUpgradeGranted);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<GameStateSetEvent>(OnGameStateSet);
        GameEventManager.Instance.RemoveListener<UpdateScoreEvent>(OnUpdateScore);
        GameEventManager.Instance.RemoveListener<UpdateXPEvent>(OnUpdateXP);
        GameEventManager.Instance.RemoveListener<UpgradeGranted>(OnUpgradeGranted);

    }

    private void OnUpdateXP(UpdateXPEvent e)
    {
        xpText.text = e.xp + "/" + e.maxXP;
        float value = ((float)e.xp) / ((float)e.maxXP);
        xpSlider.value = value;
    }

    private void OnUpgradeGranted(UpgradeGranted e)
    {
        upgradeText.SetActive(true);
        levelText.text = e.currentLevel.ToString();

        Invoke("DisaableUpgradeText", 3f);
    }

    void DisaableUpgradeText()
    {
        upgradeText.SetActive(false);
    }

    private void OnUpdateScore(UpdateScoreEvent e)
    {
        scoreText.text = e.score.ToString();
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
                inGamePanel.SetActive(true);
                break;
            case GameState.LevelComplete:
                break;
            case GameState.LevelFailed:
                inGamePanel.SetActive(false);
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

    public void StartGame()
    {
        GameEventManager.Instance.TriggerSyncEvent(new StartGameEvent());
        mainMenuPanel.SetActive(false);
    }

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    #endregion
}
