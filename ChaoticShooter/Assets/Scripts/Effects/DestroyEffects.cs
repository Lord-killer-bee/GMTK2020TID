using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffects : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;

    public void InitiateDestroy()
    {
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
