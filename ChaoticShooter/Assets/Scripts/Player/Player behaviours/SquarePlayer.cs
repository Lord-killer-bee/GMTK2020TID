using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePlayer : MonoBehaviour
{
    [SerializeField] private AudioClip charge;
    

    [SerializeField] private Transform[] reflectors;
    [SerializeField] private GameObject chargeEffectPref;
    [SerializeField] private float shieldChargeTime = 0.5f;

    private bool[] reflectorsStatus;
    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized = false;

    [SerializeField] private float minShieldSwitchTime = 1.5f;
    [SerializeField] private float maxShieldSwitchTime = 3f;

    private List<GameObject> chargeEffects = new List<GameObject>();

    private DateTime switchTime, chargeTime;
    private bool chargeStarted;
    private float currentSwitchTime;

    public void InitializeBehaviour()
    {
        if (behaviourInitialized)
            return;

        behaviourType = PlayerBehaviourType.Square;
        behaviourInitialized = true;

        reflectorsStatus = new bool[reflectors.Length];
        for (int i = 0; i < UpgradeManager.ShieldsCount; i++)
        {
            reflectorsStatus[i] = true;
        }

        SetReflectors();

        currentSwitchTime = UnityEngine.Random.Range(minShieldSwitchTime, maxShieldSwitchTime);
        switchTime = DateTime.Now;
        chargeStarted = false;
    }

    public void SetReflectors()
    {
        for (int i = 0; i < reflectors.Length; i++)
        {
            if(reflectorsStatus[i])
                reflectors[i].GetChild(0).gameObject.SetActive(true);
            else
                reflectors[i].GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (chargeStarted)
        {

            if ((DateTime.Now - chargeTime).TotalMilliseconds >= shieldChargeTime * 1000)
            {
                for (int i = 0; i < chargeEffects.Count; i++)
                {
                    Destroy(chargeEffects[i]);
                }
                chargeEffects.Clear();

                SetReflectors();

                currentSwitchTime = UnityEngine.Random.Range(minShieldSwitchTime, maxShieldSwitchTime);
                switchTime = DateTime.Now;
                chargeStarted = false;
            }
        }
        else
        {
            if ((DateTime.Now - switchTime).TotalMilliseconds >= currentSwitchTime * 1000)
            {
                ShuffleStatuses();
                GetComponent<AudioSource>().PlayOneShot(charge);

                for (int i = 0; i < reflectorsStatus.Length; i++)
                {
                    if (reflectorsStatus[i])
                    {
                        GameObject chargeEffect = Instantiate(chargeEffectPref, Vector3.zero, Quaternion.identity, reflectors[i]);
                        chargeEffect.transform.localPosition = Vector3.zero;
                        chargeEffect.transform.localEulerAngles = Vector3.zero;

                        chargeEffects.Add(chargeEffect);
                    }
                }

                chargeStarted = true;
                chargeTime = DateTime.Now;
            }
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
