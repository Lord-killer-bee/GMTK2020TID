using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlayer : MonoBehaviour
{
    [SerializeField] private float arcAngle;
    [SerializeField] private float arcRadius;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float reloadTime;
    [SerializeField] private Transform[] pusherPoints;

    [SerializeField] private int activePointsCount;

    private DateTime firedTime;
    bool waitStarted = false;

    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized;
    private bool[] pusherStatus;

    [SerializeField] private float minPusherSwitchTime = 1.5f;
    [SerializeField] private float maxPusherSwitchTime = 3f;

    private DateTime switchTime;
    private float currentSwitchTime;

    Collider[] overlapColliders;

    public void InitializeBehaviour()
    {
        if (behaviourInitialized)
            return;

        behaviourType = PlayerBehaviourType.Square;
        behaviourInitialized = true;

        pusherStatus = new bool[pusherPoints.Length];

        for (int i = 0; i < activePointsCount; i++)
        {
            pusherStatus[i] = true;
        }

        SetPushers();

        currentSwitchTime = UnityEngine.Random.Range(minPusherSwitchTime, maxPusherSwitchTime);
        switchTime = DateTime.Now;

        firedTime = DateTime.Now;
        waitStarted = true;
    }

    private void SetPushers()
    {
        for (int i = 0; i < pusherPoints.Length; i++)
        {
            if (pusherStatus[i])
                pusherPoints[i].gameObject.SetActive(true);
            else
                pusherPoints[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if ((DateTime.Now - switchTime).TotalMilliseconds >= currentSwitchTime * 1000)
        {
            ShuffleStatuses();
            SetPushers();

            currentSwitchTime = UnityEngine.Random.Range(minPusherSwitchTime, maxPusherSwitchTime);
            switchTime = DateTime.Now;
        }

        if (waitStarted)
        {
            if ((DateTime.Now - firedTime).TotalMilliseconds >= reloadTime * 1000)
            {
                for (int i = 0; i < pusherPoints.Length; i++)
                {
                    if (pusherStatus[i])
                    {
                        FireWave(i);
                    }
                }

                firedTime = DateTime.Now;
                waitStarted = true;
            }
        }
    }

    private void FireWave(int index)
    {
        overlapColliders = Physics.OverlapSphere(transform.position, arcRadius);

        Vector3 leftMargin = Quaternion.AngleAxis(arcAngle / 2, Vector3.up) * pusherPoints[index].transform.forward;
        Vector3 rightMargin = Quaternion.AngleAxis(-arcAngle / 2, Vector3.up) * pusherPoints[index].transform.forward;

        for (int i = 0; i < overlapColliders.Length; i++)
        {
            if (overlapColliders[i].tag == GameConsts.ENEMY_TAG)
            {
                Vector3 temp = overlapColliders[i].transform.position - pusherPoints[index].transform.position;

                if ((Vector3.Angle(temp, leftMargin) < arcAngle) && ((Vector3.Angle(temp, rightMargin) < arcAngle)))
                {
                    //overlapColliders[i].GetComponentInChildren<Rigidbody>().AddExplosionForce(knockbackDistance, transform.position, arcRadius, 0, ForceMode.VelocityChange);
                    overlapColliders[i].GetComponentInChildren<Rigidbody>().AddForce(((temp) * knockbackDistance), ForceMode.VelocityChange);
                }
            }
        }
    }

    private void ShuffleStatuses()
    {
        for (int i = 0; i < pusherStatus.Length; i++)
        {
            var temp = pusherStatus[i];
            int randomIndex = UnityEngine.Random.Range(i, pusherStatus.Length);
            pusherStatus[i] = pusherStatus[randomIndex];
            pusherStatus[randomIndex] = temp;
        }
    }
}
