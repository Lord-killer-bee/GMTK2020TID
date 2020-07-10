using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float rowCount;
    [SerializeField] private float columnCount;

    public static float minMarginX = -5;
    public static float maxMarginX = 5;
    public static float minMarginZ = -5;
    public static float maxMarginZ = 5;

    void Start()
    {
        SetupLevel();
    }

    public void SetupLevel()
    {
        minMarginX = -rowCount / 2;
        maxMarginX = -minMarginX;
        minMarginZ = -columnCount / 2;
        maxMarginZ = -minMarginZ;


    }
}
