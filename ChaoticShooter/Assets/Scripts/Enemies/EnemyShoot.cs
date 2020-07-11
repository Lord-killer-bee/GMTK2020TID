using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyShoot : EnemyBaseBehaviour
{
    private enum ShootingEnemyState
    {
        None,
        Searching,
        LockingOnTarget,
        ChargingUpShot,
        WindingUp
    }

    [SerializeField] private float detectionRadius;
    [SerializeField] private float bulletChargeTime;
    [SerializeField] private float windupTime;
    [Range(0, 1)] [SerializeField] private float rotationSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;
    [SerializeField] GameObject bulletPref;
    [SerializeField] GameObject firePoint;

    private ShootingEnemyState currentState = ShootingEnemyState.Searching;

    private DateTime lockedOnTimeStamp, shotFiredTimeStamp;

    Collider[] overlapColliders;
    Vector3 fireDirection;

    private GameObject shootingTarget;

    public override void InitializeBehaviour(GameObject target)
    {

    }

    void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// Set any initial parameters here
    /// </summary>
    /// <param name="nextState"></param>
    void SetState(ShootingEnemyState nextState)
    {
        if (currentState == nextState)
            return;

        currentState = nextState;
    }

    /// <summary>
    /// Updates the current ongoing state
    /// </summary>
    void UpdateState()
    {
        switch (currentState)
        {
            case ShootingEnemyState.Searching:
                CheckForPlayerInRange();
                break;

            case ShootingEnemyState.LockingOnTarget:
                RotateTowardsTarget();
                break;

            case ShootingEnemyState.ChargingUpShot:
                ChargeAndShoot();
                break;

            case ShootingEnemyState.WindingUp:
                WindUpForNextLockOn();
                break;
        }
    }

    private void CheckForPlayerInRange()
    {
        overlapColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        for (int i = 0; i < overlapColliders.Length; i++)
        {
            if (overlapColliders[i].tag == GameConsts.PLAYER_TAG)
            {
                shootingTarget = overlapColliders[i].gameObject;
                SetState(ShootingEnemyState.LockingOnTarget);
            }
        }

    }

    private void RotateTowardsTarget()
    {
        transform.LookAt(shootingTarget.transform);

        lockedOnTimeStamp = DateTime.Now;
        SetState(ShootingEnemyState.ChargingUpShot);
    }

    private void ChargeAndShoot()
    {
        if ((DateTime.Now - lockedOnTimeStamp).TotalMilliseconds >= bulletChargeTime * 1000f)
        {
            FireBullet(shootingTarget);

            shotFiredTimeStamp = DateTime.Now;
            SetState(ShootingEnemyState.WindingUp);
        }
    }

    private void WindUpForNextLockOn()
    {
        if ((DateTime.Now - shotFiredTimeStamp).TotalMilliseconds >= windupTime * 1000f)
        {
            SetState(ShootingEnemyState.Searching);
        }
    }

    private void FireBullet(GameObject player)
    {
        fireDirection = firePoint.transform.forward;

        GameObject temp = Instantiate(bulletPref);
        temp.transform.position = firePoint.transform.position;
        temp.transform.forward = fireDirection;
        temp.transform.localScale = Vector3.one;

        temp.GetComponentInChildren<EnemyBullet>().InitializeBullet(bulletSpeed, fireDirection, bulletDamage, GetComponent<Rigidbody>().velocity);
    }
}
