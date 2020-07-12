using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusPlayer : MonoBehaviour
{
    [SerializeField] private GameObject ricochetBulletPref;
    [SerializeField] private Transform[] firepoints;
    [SerializeField] private float bulletSpeed = 2f;
    [SerializeField] private float reloadTime = 0.5f;
    [SerializeField] private float bulletDamage = 1f;

    [SerializeField] private float minFirepointSwitchTime = 1.5f;
    [SerializeField] private float maxFirepointSwitchTime = 3f;
    [SerializeField] private int activeFirepoints = 1;

    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized = false;
    private bool[] firepointsStatus;
    private DateTime firedTime;

    private DateTime switchTime;
    private float currentSwitchTime;

    public void InitializeBehaviour()
    {
        behaviourType = PlayerBehaviourType.Plus;
        behaviourInitialized = true;

        firepointsStatus = new bool[firepoints.Length];
        for (int i = 0; i < activeFirepoints; i++)
        {
            firepointsStatus[i] = true;
        }

        firedTime = DateTime.Now;

        currentSwitchTime = UnityEngine.Random.Range(minFirepointSwitchTime, maxFirepointSwitchTime);
        switchTime = DateTime.Now;
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

            if ((DateTime.Now - switchTime).TotalMilliseconds >= currentSwitchTime * 1000)
            {
                RandomizeFirepoints();
            }
        }
    }

    private void FireBullet(int index)
    {
        GameObject bullet = Instantiate(ricochetBulletPref);
        bullet.transform.position = firepoints[index].position;
        bullet.transform.forward = firepoints[index].forward;
        //bullet.transform.parent = transform;
        bullet.GetComponent<RicochetBullet>().InitializeBullet(bulletSpeed, bulletDamage, GetComponentInParent<PlayerController>().GetVelocity());
    }

    private void RandomizeFirepoints()
    {
        firepointsStatus = new bool[firepoints.Length];

        for (int i = activeFirepoints - 1; i >= 0; i--)
        {
            firepointsStatus[i] = true;
        }

        ShuffleStatuses();

        currentSwitchTime = UnityEngine.Random.Range(minFirepointSwitchTime, maxFirepointSwitchTime);
        switchTime = DateTime.Now;
    }


    private void ShuffleStatuses()
    {
        for (int i = 0; i < firepointsStatus.Length; i++)
        {
            var temp = firepointsStatus[i];
            int randomIndex = UnityEngine.Random.Range(i, firepointsStatus.Length);
            firepointsStatus[i] = firepointsStatus[randomIndex];
            firepointsStatus[randomIndex] = temp;
        }
    }
}
