using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int rowCount = 10;
    [SerializeField] private int columnCount = 10;
    [SerializeField] private int camMarginOffset = 1;

    public static float minMarginX = -5;
    public static float maxMarginX = 5;
    public static float minMarginZ = -5;
    public static float maxMarginZ = 5;

    public static float minCamMarginX = -5;
    public static float maxCamMarginX = 5;
    public static float minCamMarginZ = -5;
    public static float maxCamMarginZ = 5;

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

        minCamMarginX = (-rowCount / 2) + camMarginOffset;
        maxCamMarginX = -minMarginX - camMarginOffset;
        minCamMarginZ = (-columnCount / 2) + camMarginOffset;
        maxCamMarginZ = -minMarginZ - camMarginOffset;
    }
}
