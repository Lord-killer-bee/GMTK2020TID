using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePlayer : MonoBehaviour
{
    [SerializeField] private Transform[] reflectors;

    private bool[] reflectorsStatus;
    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized = false;

    [SerializeField] private float minShieldSwitchTime = 1.5f;
    [SerializeField] private float maxShieldSwitchTime = 3f;

    private DateTime switchTime;
    private float currentSwitchTime;

    public void InitializeBehaviour()
    {
        behaviourType = PlayerBehaviourType.Square;
        behaviourInitialized = true;

        reflectorsStatus = new bool[reflectors.Length];
        reflectorsStatus[0] = true;
        reflectorsStatus[2] = true;

        SetReflectors();

        currentSwitchTime = UnityEngine.Random.Range(minShieldSwitchTime, maxShieldSwitchTime);
        switchTime = DateTime.Now;
    }

    public void SetReflectors()
    {
        for (int i = 0; i < reflectors.Length; i++)
        {
            if(reflectorsStatus[i])
                reflectors[i].gameObject.SetActive(true);
            else
                reflectors[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if ((DateTime.Now - switchTime).TotalMilliseconds >= currentSwitchTime * 1000)
        {
            ShuffleStatuses();
            SetReflectors();

            currentSwitchTime = UnityEngine.Random.Range(minShieldSwitchTime, maxShieldSwitchTime);
            switchTime = DateTime.Now;
        }
    }

    private void ShuffleStatuses()
    {
        for (int i = 0; i < reflectorsStatus.Length; i++)
        {
            var temp = reflectorsStatus[i];
            int randomIndex = UnityEngine.Random.Range(i, reflectorsStatus.Length);
            reflectorsStatus[i] = reflectorsStatus[randomIndex];
            reflectorsStatus[randomIndex] = temp;
        }
    }
}
