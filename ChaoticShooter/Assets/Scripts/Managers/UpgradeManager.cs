using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private int baseLaserGunsCount;
    [SerializeField] private int baseShieldsCount;
    [SerializeField] private int basePushersCount;
    [SerializeField] private int baseRicochetGunsCount;

    [SerializeField] private int xpForEnemyKill;
    [SerializeField] private int xpPerSecond;
    [SerializeField] private int[] xpPerLevel;

    public static int LaserGunsCount;
    public static int ShieldsCount;
    public static int PushersCount;
    public static int RicochetGunsCount;

    private int currentXP;
    private int currentLevel;

    private Dictionary<int, bool> pendingUpgrades = new Dictionary<int, bool>();

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<StartGameEvent>(OnGameStarted);
        GameEventManager.Instance.AddListener<EnemyKilledEvent>(OnEnemyKilled);
        GameEventManager.Instance.AddListener<SecondPassed>(OnSecondPassed);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<StartGameEvent>(OnGameStarted);
        GameEventManager.Instance.RemoveListener<EnemyKilledEvent>(OnEnemyKilled);
        GameEventManager.Instance.RemoveListener<SecondPassed>(OnSecondPassed);

    }

    private void OnSecondPassed(SecondPassed e)
    {
        currentXP += xpPerSecond;

        if (currentLevel < xpPerLevel.Length)
            GameEventManager.Instance.TriggerAsyncEvent(new UpdateXPEvent(currentXP, xpPerLevel[currentLevel]));

        CheckForLevelUp();
    }

    private void OnGameStarted(StartGameEvent e)
    {
        pendingUpgrades.Clear();
        for (int i = 0; i < 4; i++)
        {
            pendingUpgrades.Add(i, true);
        }

        LaserGunsCount = baseLaserGunsCount;
        ShieldsCount = baseShieldsCount;
        PushersCount = basePushersCount;
        RicochetGunsCount = baseRicochetGunsCount;

        currentXP = 0;
        currentLevel = 0;
    }

    private void OnEnemyKilled(EnemyKilledEvent e)
    {
        currentXP += xpForEnemyKill;

        if(currentLevel < xpPerLevel.Length)
            GameEventManager.Instance.TriggerAsyncEvent(new UpdateXPEvent(currentXP, xpPerLevel[currentLevel]));

        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        if(currentXP > xpPerLevel[currentLevel])
        {
            currentXP = 0;

            if (currentLevel < xpPerLevel.Length - 1)
            {
                currentLevel++;
                GrantRandomUpgrade();
            }
        }
    }

    private void GrantRandomUpgrade()
    {
        int index = GetRandomIndex();
        PlayerBehaviourType behaviourType = PlayerBehaviourType.None;

        switch (index)
        {
            case 0:
                if (LaserGunsCount < 3)
                    LaserGunsCount++;

                if (LaserGunsCount == 3)
                    pendingUpgrades.Remove(0);

                behaviourType = PlayerBehaviourType.Triangle;
                break;
            case 1:
                if (ShieldsCount < 4)
                    ShieldsCount++;

                if (ShieldsCount == 4)
                    pendingUpgrades.Remove(1);

                behaviourType = PlayerBehaviourType.Square;
                break;
            case 2:
                if (PushersCount < 4)
                    PushersCount++;

                if (PushersCount == 4)
                    pendingUpgrades.Remove(2);

                behaviourType = PlayerBehaviourType.Circle;
                break;
            case 3:
                if (RicochetGunsCount < 4)
                    RicochetGunsCount++;

                if (RicochetGunsCount == 4)
                    pendingUpgrades.Remove(3);
                behaviourType = PlayerBehaviourType.Plus;
                break;
        }

        GameEventManager.Instance.TriggerSyncEvent(new UpgradeGranted(currentLevel, behaviourType));
    }

    private int GetRandomIndex()
    {
        List<int> keys = Enumerable.ToList(pendingUpgrades.Keys);

        return keys[UnityEngine.Random.Range(0, keys.Count)];
    }
}