using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private int startingEnemyCount;
    [SerializeField] private float enemySpawnTimeGap;
    [SerializeField] private GameObject[] spawnableEnemies;

#if UNITY_EDITOR
    [Help("The weights should add up to 100")]
#endif
    [SerializeField] private float[] spawnableEnemyWeights;


    private List<GameObject> currentEnemies = new List<GameObject>();
    private float[] currentSpawnableEnemyWeights;
    private bool enemyCreated = false;
    private DateTime enemyCreatedTime;
    private Transform targetPlayer;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<PlayerCreatedEvent>(CreateStartingEnemies);
        GameEventManager.Instance.AddListener<GameStateSetEvent>(OnGameStateSet);
        GameEventManager.Instance.AddListener<DamageEnemyEvent>(DamageEnemy);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<PlayerCreatedEvent>(CreateStartingEnemies);
        GameEventManager.Instance.RemoveListener<GameStateSetEvent>(OnGameStateSet);
        GameEventManager.Instance.RemoveListener<DamageEnemyEvent>(DamageEnemy);

    }

    void Update()
    {
        if (!targetPlayer)
            return;

        if (enemyCreated)
        {
            if((DateTime.Now - enemyCreatedTime).TotalMilliseconds >= enemySpawnTimeGap * 1000)
            {
                CreateNextEnemy();
            }
        }
    }

    private void CreateStartingEnemies(PlayerCreatedEvent e)
    {
        currentEnemies.Clear();

        currentSpawnableEnemyWeights = new float[spawnableEnemyWeights.Length];
        for (int i = 0; i < spawnableEnemyWeights.Length; i++)
        {
            currentSpawnableEnemyWeights[i] = spawnableEnemyWeights[i];
        }

        targetPlayer = e.transform;

        for (int i = 0; i < startingEnemyCount; i++)
        {
            GameObject obj = GetEnemyBasedOnWeight();
            obj.transform.position = enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length - 1)].position;
            obj.transform.LookAt(targetPlayer.position);
            obj.GetComponent<Enemy>().InitializeEnemy(targetPlayer.gameObject);
            currentEnemies.Add(obj);
        }

        enemyCreated = true;
        enemyCreatedTime = DateTime.Now;
    }

    private void OnGameStateSet(GameStateSetEvent e)
    {
        switch (e.gameState)
        {
            case GameState.LevelGeneration:
                for (int i = 0; i < currentEnemies.Count; i++)
                {
                    Destroy(currentEnemies[i]);
                }

                currentEnemies.Clear();
                break;
        }
    }

    private void DamageEnemy(DamageEnemyEvent e)
    {
        if (e.targetEnemy.GetComponent<Enemy>().TakeDamage(e.damageDealt, e.direction))
        {
            currentEnemies.Remove(e.targetEnemy);
        }
    }

    void CreateNextEnemy()
    {
        enemyCreatedTime = DateTime.Now;

        GameObject obj = GetEnemyBasedOnWeight();
        obj.transform.position = enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length)].position;
        obj.transform.LookAt(targetPlayer.position);
        obj.GetComponent<Enemy>().InitializeEnemy(targetPlayer.gameObject);
        currentEnemies.Add(obj);
    }

    GameObject GetEnemyBasedOnWeight()
    {
        int index = Array.IndexOf(currentSpawnableEnemyWeights, Mathf.Max(currentSpawnableEnemyWeights));
        float addValue = Mathf.Min(spawnableEnemyWeights);

        for (int i = 0; i < currentSpawnableEnemyWeights.Length; i++)
        {
            if (i != index)
                currentSpawnableEnemyWeights[i] += (addValue / (currentSpawnableEnemyWeights.Length - 1));
            else
                currentSpawnableEnemyWeights[i] -= addValue;
        }

        return Instantiate(spawnableEnemies[index]);
    }
}
