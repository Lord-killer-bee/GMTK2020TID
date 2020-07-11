using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxEnemyHP = 1;

    private float currentHP;

    public void InitializeEnemy(GameObject player)
    {
        EnemyBaseBehaviour[] behaviours = GetComponents<EnemyBaseBehaviour>();

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].InitializeBehaviour(player);
        }

        currentHP = maxEnemyHP;
    }

    public bool TakeDamage(float damage)
    {
        currentHP -= damage;

        GameEventManager.Instance.TriggerAsyncEvent(new ShakeCameraEvent());

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }

    }
}
