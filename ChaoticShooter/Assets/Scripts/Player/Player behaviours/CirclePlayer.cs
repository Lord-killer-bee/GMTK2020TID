using Core;
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
    [SerializeField] private float chargeDuration;
    [SerializeField] private Transform[] pusherPoints;
    [SerializeField] private GameObject chargeEffectPref;
    [SerializeField] private GameObject pushEffectPref;

    private DateTime waitTime, chargeTime;
    private bool waitStarted, chargeStarted;

    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized;
    private bool[] pusherStatus;

    [SerializeField] private float minPusherSwitchTime = 1.5f;
    [SerializeField] private float maxPusherSwitchTime = 3f;
    [SerializeField] private int activePointsCount;

    private DateTime switchTime;
    private float currentSwitchTime;

    private bool randomizeTriggered = false;

    Collider[] overlapColliders;
    private List<GameObject> chargeEffects = new List<GameObject>();
    private List<GameObject> pushEffects = new List<GameObject>();

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

        waitTime = DateTime.Now;
        waitStarted = true;
        chargeStarted = false;
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
            TriggerRandomize();
        }

        if (waitStarted)
        {
            if ((DateTime.Now - waitTime).TotalMilliseconds >= reloadTime * 1000)
            {
                for (int i = 0; i < pushEffects.Count; i++)
                {
                    Destroy(pushEffects[i]);
                }

                pushEffects.Clear();

                if (randomizeTriggered)
                {
                    ShuffleStatuses();
                    SetPushers();

                    currentSwitchTime = UnityEngine.Random.Range(minPusherSwitchTime, maxPusherSwitchTime);
                    switchTime = DateTime.Now;

                    randomizeTriggered = false;
                }

                for (int i = 0; i < pusherStatus.Length; i++)
                {
                    if (pusherStatus[i])
                    {
                        GameObject chargeEffect = Instantiate(chargeEffectPref, Vector3.zero, Quaternion.identity, pusherPoints[i]);
                        chargeEffect.transform.localPosition = Vector3.zero;
                        chargeEffect.transform.localEulerAngles = Vector3.zero;

                        chargeEffects.Add(chargeEffect);
                    }
                }

                chargeTime = DateTime.Now;
                chargeStarted = true;

                waitStarted = false;
            }
        }
        else if (chargeStarted)
        {
            if ((DateTime.Now - chargeTime).TotalMilliseconds >= chargeDuration * 1000)
            {
                for (int i = 0; i < chargeEffects.Count; i++)
                {
                    Destroy(chargeEffects[i]);
                }

                chargeEffects.Clear();

                for (int i = 0; i < pusherPoints.Length; i++)
                {
                    if (pusherStatus[i])
                    {
                        FireWave(i);
                    }
                }

                GameEventManager.Instance.TriggerAsyncEvent(new ShakeCameraEvent());

                waitTime = DateTime.Now;
                waitStarted = true;

                chargeStarted = false;
            }
        }
    }

    void TriggerRandomize()
    {
        randomizeTriggered = true;
    }

    private void FireWave(int index)
    {
        GameObject pushEffect = Instantiate(pushEffectPref, Vector3.zero, Quaternion.identity, pusherPoints[index]);
        pushEffect.transform.localPosition = Vector3.zero;
        pushEffect.transform.localEulerAngles = new Vector3(0, 0, 90);

        pushEffects.Add(pushEffect);

        overlapColliders = Physics.OverlapSphere(transform.position, arcRadius);

        Vector3 leftMargin = Quaternion.AngleAxis(arcAngle / 2, Vector3.up) * pusherPoints[index].transform.forward;
        Vector3 rightMargin = Quaternion.AngleAxis(-arcAngle / 2, Vector3.up) * pusherPoints[index].transform.forward;

        for (int i = 0; i < overlapColliders.Length; i++)
        {
            if (overlapColliders[i].tag == GameConsts.ENEMY_TAG || overlapColliders[i].tag == GameConsts.ENEMY_BULLET_TAG)
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
