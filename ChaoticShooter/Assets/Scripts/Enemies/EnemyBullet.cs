using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffectPref;

    private bool bulletInitialized = false;
    private float bulletSpeed;
    private Vector3 fireDirection;
    private float bulletDamage;

    private Vector3 enemyVelocity;

    private Rigidbody rigidbody;
    bool reflected = false;

    public void ReflectBullet(Vector3 normal)
    {
        transform.forward = Vector3.Reflect(transform.forward, normal);
        reflected = true;
    }

    public void PropelBullet(Vector3 direction)
    {
        transform.forward = direction;
        reflected = true;
    }

    private void Update()
    {
        if (bulletInitialized)
        {
            if(reflected)
                rigidbody.velocity = (transform.forward - enemyVelocity) * bulletSpeed * 2;
            else
                rigidbody.velocity = (transform.forward + enemyVelocity) * bulletSpeed;
        }
    }

    public void InitializeBullet(float bulletSpeed, Vector3 fireDirection, float bulletDamage, Vector3 enemyVelocity)
    {
        bulletInitialized = true;
        this.bulletSpeed = bulletSpeed;
        this.fireDirection = fireDirection;
        this.bulletDamage = bulletDamage;
        this.enemyVelocity = enemyVelocity;

        rigidbody = GetComponent<Rigidbody>();

        GameEventManager.Instance.AddListener<GameStateSetEvent>(OnGameStateSet);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<GameStateSetEvent>(OnGameStateSet);
    }

    private void OnGameStateSet(GameStateSetEvent e)
    {
        if (this == null)
            return;

        if(e.gameState == GameState.LevelGeneration)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == GameConsts.ENEMY_TAG || collision.gameObject.tag == GameConsts.PLAYER_TAG || collision.gameObject.tag == GameConsts.BOUNDARY_TAG)
        {
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == GameConsts.ENEMY_TAG)
        {
            GameEventManager.Instance.TriggerSyncEvent(new DamageEnemyEvent(collision.gameObject, bulletDamage));
        }
    }

    public void DestroyWithEffect()
    {
        GameObject obj = Instantiate(destroyEffectPref);
        obj.transform.position = transform.position;
        obj.transform.localEulerAngles = new Vector3(-90, 0, 0);
        obj.GetComponent<DestroyEffects>().InitiateDestroy();

        Destroy(gameObject);
    }
}
