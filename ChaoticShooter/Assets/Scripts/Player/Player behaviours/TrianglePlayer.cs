using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePlayer : MonoBehaviour
{
    [SerializeField] private GameObject laserPref;
    [SerializeField] private GameObject chargeEffectPref;
    [SerializeField] private Transform[] firepoints;
    [SerializeField] private float laserStayTime = 2f;
    [SerializeField] private float laserChargeTime = 0.5f;
    [SerializeField] private float reloadTime = 0.5f;
    [SerializeField] private float laserDamage = 1f;
    [SerializeField] private float laserLength = 1f;


    [SerializeField] private float minFirepointSwitchTime = 1.5f;
    [SerializeField] private float maxFirepointSwitchTime = 3f;


    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized = false;
    private bool[] firepointsStatus;
    private DateTime firedTime, waitTime, chargeTime;
    private bool fireStarted, waitStarted, chargeStarted;

    private DateTime switchTime;
    private float currentSwitchTime;

    private bool randomizeTriggered = false;

    private List<GameObject> lasersList = new List<GameObject>();
    private List<GameObject> chargeEffects = new List<GameObject>();

    public void InitializeBehaviour()
    {
        if (behaviourInitialized)
            return;

        behaviourType = PlayerBehaviourType.Triangle;
        behaviourInitialized = true;

        firepointsStatus = new bool[firepoints.Length];
        for (int i = 0; i < UpgradeManager.LaserGunsCount; i++)
        {
            firepointsStatus[i] = true;
            firepoints[i].gameObject.SetActive(true);
        }

        waitTime = DateTime.Now;
        waitStarted = true;
        fireStarted = false;
        chargeStarted = false;

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
                    if (randomizeTriggered)
                        RandomizeFirepoints();

                    chargeTime = DateTime.Now;
                    chargeStarted = true;

                    for (int i = 0; i < firepointsStatus.Length; i++)
                    {
                        if (firepointsStatus[i])
                        {
                            GameObject chargeEffect = Instantiate(chargeEffectPref, Vector3.zero, Quaternion.identity, firepoints[i]);
                            chargeEffect.transform.localPosition = Vector3.zero;

                            chargeEffects.Add(chargeEffect);
                        }
                    }
                    

                    fireStarted = false;
                    waitStarted = false;
                }
            }
            else if (chargeStarted)
            {
                if ((DateTime.Now - chargeTime).TotalMilliseconds >= laserChargeTime * 1000)
                {
                    lasersList.Clear();

                    for (int i = 0; i < chargeEffects.Count; i++)
                    {
                        Destroy(chargeEffects[i]);
                    }

                    chargeEffects.Clear();

                    for (int i = 0; i < firepoints.Length; i++)
                    {
                        if (firepoints[i].gameObject.activeInHierarchy)
                        {
                            FireLaser(i);
                        }
                    }

                    firedTime = DateTime.Now;
                    fireStarted = true;

                    waitStarted = false;
                    chargeStarted = false;
                }
            }
            else if (fireStarted)
            {
                if ((DateTime.Now - firedTime).TotalMilliseconds >= laserStayTime * 1000)
                {
                    waitTime = DateTime.Now;
                    waitStarted = true;

                    fireStarted = false;
                    chargeStarted = false;

                    for (int i = 0; i < lasersList.Count; i++)
                    {
                        Destroy(lasersList[i]);
                    }
                    lasersList.Clear();
                }
            }

            if ((DateTime.Now - switchTime).TotalMilliseconds >= currentSwitchTime * 1000 && !randomizeTriggered)
            {
                TriggerRandomize();
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

        //GameEventManager.Instance.TriggerAsyncEvent(new ShakeCameraEvent());
    }

    void TriggerRandomize()
    {
        randomizeTriggered = true;
    }

    private void RandomizeFirepoints()
    {
        firepointsStatus = new bool[firepoints.Length];

        for (int i = UpgradeManager.LaserGunsCount - 1; i >= 0; i--)
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

        for (int i = 0; i < firepointsStatus.Length; i++)
        {
            firepoints[i].gameObject.SetActive(firepointsStatus[i]);
        }
    }
}
