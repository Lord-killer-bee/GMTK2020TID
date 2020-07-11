using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusPlayer : MonoBehaviour
{
    [SerializeField] private GameObject triangleBulletPref;
    [SerializeField] private Transform[] firepoints;
    [SerializeField] private float bulletSpeed = 2f;
    [SerializeField] private float reloadTime = 0.5f;
    [SerializeField] private float bulletDamage = 1f;

    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized = false;
    private bool[] firepointsStatus;
    private DateTime firedTime;

    public void InitializeBehaviour()
    {
        behaviourType = PlayerBehaviourType.Plus;
        behaviourInitialized = true;

        firepointsStatus = new bool[firepoints.Length];
        for (int i = 0; i < firepoints.Length; i++)
        {
            firepointsStatus[i] = true;
        }

        firedTime = DateTime.Now;
    }

    private void Update()
    {
        if (behaviourInitialized)
        {
            if ((DateTime.Now - firedTime).TotalMilliseconds >= reloadTime * 1000)
            {
                for (int i = 0; i < firepointsStatus.Length; i++)
                {
                    if (firepointsStatus[i])
                    {
                        FireBullet(i);
                    }
                }

                firedTime = DateTime.Now;
            }

        }
    }

    private void FireBullet(int index)
    {
        GameObject bullet = Instantiate(triangleBulletPref);
        bullet.transform.position = firepoints[index].position;
        bullet.transform.forward = firepoints[index].forward;
        //bullet.transform.parent = transform;
        bullet.GetComponent<RicochetBullet>().InitializeBullet(bulletSpeed, bulletDamage, GetComponent<PlayerController>().GetVelocity());
    }
}
