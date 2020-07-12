using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxEnemyHP = 1;
    [SerializeField] private GameObject destroyEffectPref;

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

    public bool TakeDamage(float damage, Vector3 direction)
    {
        currentHP -= damage;

        GameEventManager.Instance.TriggerAsyncEvent(new ShakeCameraEvent());

        if (currentHP <= 0)
        {
            GameObject obj = Instantiate(destroyEffectPref);
            obj.transform.position = transform.position + new Vector3(0, 1, 0);
            obj.transform.forward = direction;
            obj.GetComponent<DestroyEffects>().InitiateDestroy();

            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }

    }
}
