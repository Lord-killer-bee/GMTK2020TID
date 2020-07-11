using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void InitializeEnemy(GameObject player)
    {
        EnemyBaseBehaviour[] behaviours = GetComponents<EnemyBaseBehaviour>();

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].InitializeBehaviour(player);
        }
    }
}
