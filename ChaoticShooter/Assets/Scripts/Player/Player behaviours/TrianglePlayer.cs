using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePlayer : MonoBehaviour
{
    [SerializeField] private GameObject laserPref;
    [SerializeField] private Transform[] firepoints;
    [SerializeField] private float laserStayTime = 2f;
    [SerializeField] private float reloadTime = 0.5f;
    [SerializeField] private float laserDamage = 1f;
    [SerializeField] private float laserLength = 1f;

    [SerializeField] private float minFirepointSwitchTime = 1.5f;
    [SerializeField] private float maxFirepointSwitchTime = 3f;
    [SerializeField] private int activeFirepoints = 1;


    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized = false;
    private bool[] firepointsStatus;
    private DateTime firedTime, waitTime;
    private bool fireStarted, waitStarted;

    private DateTime switchTime;
    private float currentSwitchTime;

    private List<GameObject> lasersList = new List<GameObject>();

    public void InitializeBehaviour()
    {
        behaviourType = PlayerBehaviourType.Triangle;
        behaviourInitialized = true;

        firepointsStatus = new bool[firepoints.Length];
        for (int i = 0; i < activeFirepoints; i++)
        {
            firepointsStatus[i] = true;
        }

        waitTime = DateTime.Now;
        waitStarted = true;
        fireStarted = false;

        currentSwitchTime = UnityEngine.Random.Range(minFirepointSwitchTime, maxFirepointSwitchTime);
        switchTime = DateTime.Now;
    }

    private void Update()
    {
        if (behaviourInitialized)
        {
            if (waitStarted)
            {
                if ((DateTime.Now - waitTime).TotalMilliseconds >= reloadTime * 1000)
                {
                    lasersList.Clear();
                    for (int i = 0; i < firepointsStatus.Length; i++)
                    {
                        if (firepointsStatus[i])
                        {
                            FireLaser(i);
                        }
                    }

                    firedTime = DateTime.Now;
                    fireStarted = true;
                    waitStarted = false;
                }
            }
            else if (fireStarted)
            {
                if ((DateTime.Now - firedTime).TotalMilliseconds >= laserStayTime * 1000)
                {
                    waitTime = DateTime.Now;
                    fireStarted = false;
                    waitStarted = true;

                    for (int i = 0; i < lasersList.Count; i++)
                    {
                        Destroy(lasersList[i]);
                    }
                    lasersList.Clear();
                }
            }

            if ((DateTime.Now - switchTime).TotalMilliseconds >= currentSwitchTime * 1000)
            {
                RandomizeFirepoints();
            }
        }
    }

    private void FireLaser(int index)
    {
        GameObject laser = Instantiate(laserPref);
        laser.transform.parent = transform;
        laser.transform.localPosition = firepoints[index].localPosition;
        laser.transform.forward = firepoints[index].forward;
        lasersList.Add(laser);
        laser.GetComponentInChildren<LaserBullet>().InitializeBullet(laserLength, laserDamage);
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
