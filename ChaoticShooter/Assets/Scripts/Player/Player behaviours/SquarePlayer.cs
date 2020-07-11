using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePlayer : MonoBehaviour
{
    [SerializeField] private Transform[] reflectors;

    private bool[] reflectorsStatus;
    private PlayerBehaviourType behaviourType;
    private bool behaviourInitialized = false;

    public void InitializeBehaviour()
    {
        behaviourType = PlayerBehaviourType.Square;
        behaviourInitialized = true;

        SetReflectors();
    }

    public void SetReflectors()
    {
        for (int i = 0; i < reflectors.Length; i++)
        {
            if(i % 2 == 0)
                reflectors[i].gameObject.SetActive(true);
            else
                reflectors[i].gameObject.SetActive(false);
        }
    }
}
