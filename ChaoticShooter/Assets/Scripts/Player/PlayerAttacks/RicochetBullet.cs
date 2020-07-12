using Core;
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

        GameEventManager.Instance.AddListener<GameStateSetEvent>(OnGameStateSet);
    }

    private void OnGameStateSet(GameStateSetEvent e)
    {
        if (e.gameState == GameState.LevelGeneration)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == GameConsts.BOUNDARY_TAG)
        {
            // find collision point and normal. You may want to average over all contacts
            var point = collision.contacts[0].point;
            var dir = -collision.contacts[0].normal; // you need vector pointing TOWARDS the collision, not away from it
                                                     // step back a bit
            point -= dir;
            RaycastHit hitInfo;
            // cast a ray twice as far as your step back. This seems to work in all
            // situations, at least when speeds are not ridiculously big
            if (GetComponent<Collider>().Raycast(new Ray(point, dir), out hitInfo, 2))
            {
                // this is the collider surface normal
                var normal = hitInfo.normal;

                ReflectBullet(normal);
            }
        }
    }

    public void ReflectBullet(Vector3 normal)
    {
        transform.forward = Vector3.Reflect(transform.forward, normal);
        rigidbody.AddForce(((transform.forward) * bulletSpeed) - playerVelocity, ForceMode.VelocityChange);
    }
}
