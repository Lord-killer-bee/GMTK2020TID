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
    [SerializeField] private GameObject circlePref;
    [SerializeField] private GameObject plusPref;

    [SerializeField] private float minPlayerSwitchTime = 4f;
    [SerializeField] private float maxPlayerSwitchTime = 10f;

    public static float minMarginX = -5;
    public static float maxMarginX = 5;
    public static float minMarginZ = -5;
    public static float maxMarginZ = 5;

    public static float minCamMarginX = -5;
    public static float maxCamMarginX = 5;
    public static float minCamMarginZ = -5;
    public static float maxCamMarginZ = 5;

    private List<PlayerBehaviourType> currentAvailablePlayers = new List<PlayerBehaviourType>() { PlayerBehaviourType.Triangle, PlayerBehaviourType.Square , PlayerBehaviourType.Circle};
    private int currentPlayerIndex = 0;
    private GameObject selectedPlayerBehaviour;
    private GameObject player;
    private float currentSwitchTime;
    private DateTime switchTime;

    private GameState currentState;

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
        currentState = e.gameState;

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

            player = Instantiate(playerPref, Vector3.zero, Quaternion.identity);

            currentPlayerIndex = 0;
            ShufflePlayers();

            GameEventManager.Instance.TriggerSyncEvent(new GameStateCompletedEvent(GameState.LevelGeneration));
        }
        else if (e.gameState == GameState.RoulettePlayerWheel)
        {
            SelectRandomPlayer();
            GameEventManager.Instance.TriggerSyncEvent(new GameStateCompletedEvent(GameState.RoulettePlayerWheel));
        }
    }

    private void Update()
    {
        if (!player)
            return;

        if(currentState == GameState.Gameplay)
        {
            if ((DateTime.Now - switchTime).TotalMilliseconds >= currentSwitchTime * 1000f)
            {
                SelectRandomPlayer();
            }
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
        PlayerBehaviourType behaviour = currentAvailablePlayers[currentPlayerIndex];

        Destroy(selectedPlayerBehaviour);
        GameObject pref = trianglePref;

        switch (behaviour)
        {
            case PlayerBehaviourType.Triangle:
                pref = trianglePref;
                break;
            case PlayerBehaviourType.Square:
                pref = squarePref;
                break;
            case PlayerBehaviourType.Circle:
                pref = circlePref;
                break;
            case PlayerBehaviourType.Plus:
                pref = plusPref;
                break;
        }

        selectedPlayerBehaviour = Instantiate(pref, player.transform);
        selectedPlayerBehaviour.transform.localPosition = Vector3.zero;

        switch (behaviour)
        {
            case PlayerBehaviourType.Triangle:
                selectedPlayerBehaviour.GetComponentInChildren<TrianglePlayer>().InitializeBehaviour();
                break;
            case PlayerBehaviourType.Square:
                selectedPlayerBehaviour.GetComponentInChildren<SquarePlayer>().InitializeBehaviour();
                break;
            case PlayerBehaviourType.Circle:
                selectedPlayerBehaviour.GetComponentInChildren<CirclePlayer>().InitializeBehaviour();
                break;
            case PlayerBehaviourType.Plus:
                selectedPlayerBehaviour.GetComponentInChildren<PlusPlayer>().InitializeBehaviour();
                break;
        }

        currentSwitchTime = UnityEngine.Random.Range(minPlayerSwitchTime, maxPlayerSwitchTime);
        switchTime = DateTime.Now;

        currentPlayerIndex++;

        if (currentPlayerIndex > currentAvailablePlayers.Count - 1)
            currentPlayerIndex = 0;
    }
}
