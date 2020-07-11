using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int rowCount = 10;
    [SerializeField] private int columnCount = 10;
    [SerializeField] private int camMarginOffset = 1;

    [SerializeField] private GameObject playerPref;
    [SerializeField] private GameObject trianglePref;
    [SerializeField] private GameObject squarePref;

    public static float minMarginX = -5;
    public static float maxMarginX = 5;
    public static float minMarginZ = -5;
    public static float maxMarginZ = 5;

    public static float minCamMarginX = -5;
    public static float maxCamMarginX = 5;
    public static float minCamMarginZ = -5;
    public static float maxCamMarginZ = 5;

    private List<PlayerBehaviourType> currentAvailablePlayers = new List<PlayerBehaviourType>() { PlayerBehaviourType.Triangle, PlayerBehaviourType.Square };
    private int currentPlayerIndex = 0;
    private GameObject selectedPlayerBehaviour;
    private GameObject player;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<GameStateSetEvent>(SetupLevel);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<GameStateSetEvent>(SetupLevel);
    }

    private void SetupLevel(GameStateSetEvent e)
    {
        if (e.gameState == GameState.LevelGeneration)
        {
            minMarginX = -rowCount / 2;
            maxMarginX = -minMarginX;
            minMarginZ = -columnCount / 2;
            maxMarginZ = -minMarginZ;

            minCamMarginX = (-rowCount / 2) + camMarginOffset;
            maxCamMarginX = -minMarginX - camMarginOffset;
            minCamMarginZ = (-columnCount / 2) + camMarginOffset;
            maxCamMarginZ = -minMarginZ - camMarginOffset;

            GameEventManager.Instance.TriggerSyncEvent(new GameStateCompletedEvent(GameState.LevelGeneration));

            player = Instantiate(playerPref, Vector3.zero, Quaternion.identity);

            currentPlayerIndex = 0;
            ShufflePlayers();
        }
        else if (e.gameState == GameState.RoulettePlayerWheel)
        {
            SelectRandomPlayer();
        }
    }

    private void ShufflePlayers()
    {
        for (int i = 0; i < currentAvailablePlayers.Count; i++)
        {
            var temp = currentAvailablePlayers[i];
            int randomIndex = UnityEngine.Random.Range(i, currentAvailablePlayers.Count);
            currentAvailablePlayers[i] = currentAvailablePlayers[randomIndex];
            currentAvailablePlayers[randomIndex] = temp;
        }
    }

    private void SelectRandomPlayer()
    {
        
    }
}
