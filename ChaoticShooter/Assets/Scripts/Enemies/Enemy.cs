using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxEnemyHP = 1;
    [SerializeField] private GameObject destroyEffectPref;

    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private GameObject mesh;

    private float currentHP;
    GameObject player;

    public void InitializeEnemy(GameObject player)
    {
        Invoke("RealInitialize", 1.5f);
        this.player = player;
    }

    void RealInitialize()
    {
        mesh.SetActive(true);
        spawnEffect.SetActive(false);
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

            GameEventManager.Instance.TriggerSyncEvent(new EnemyKilledEvent());

            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }

    }
}
