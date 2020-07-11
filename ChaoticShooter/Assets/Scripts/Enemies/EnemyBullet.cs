using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private bool bulletInitialized = false;
    private float bulletSpeed;
    private Vector3 fireDirection;
    private float bulletDamage;

    private Vector3 enemyVelocity;

    private Rigidbody rigidbody;

    public void ReflectBullet(Vector3 normal)
    {
        transform.forward = Vector3.Reflect(transform.forward, normal);
    }

    private void Update()
    {
        if(bulletInitialized)
            rigidbody.velocity = (transform.forward + enemyVelocity) * bulletSpeed;
    }

    public void InitializeBullet(float bulletSpeed, Vector3 fireDirection, float bulletDamage, Vector3 enemyVelocity)
    {
        bulletInitialized = true;
        this.bulletSpeed = bulletSpeed;
        this.fireDirection = fireDirection;
        this.bulletDamage = bulletDamage;
        this.enemyVelocity = enemyVelocity;

        rigidbody = GetComponent<Rigidbody>();
    }
}
