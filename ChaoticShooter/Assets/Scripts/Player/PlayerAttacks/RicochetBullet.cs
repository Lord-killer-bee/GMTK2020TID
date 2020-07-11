using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : MonoBehaviour
{
    private float bulletSpeed;
    private float bulletDamage;
    private Vector3 playerVelocity;
    bool bulletInitialized = false;

    private Rigidbody rigidbody;

    public void InitializeBullet(float bulletSpeed, float bulletDamage, Vector3 playerVelocity)
    {
        this.bulletSpeed = bulletSpeed;
        this.bulletDamage = bulletDamage;
        this.playerVelocity = playerVelocity;

        bulletInitialized = true;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(((transform.forward) * bulletSpeed) + playerVelocity, ForceMode.VelocityChange);
    }

    private void Update()
    {
        if (bulletInitialized)
        {
        }
    }
}
