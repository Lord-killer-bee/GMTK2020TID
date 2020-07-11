using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float camHeight = 10f;
    private Transform playerTransform;
    private bool playerMoveStarted = false;

    Vector3 targetPosition;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<PlayerCreatedEvent>(InitializeCamera);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<PlayerCreatedEvent>(InitializeCamera);
    }

    private void InitializeCamera(PlayerCreatedEvent e)
    {
        playerTransform = e.transform;
        playerMoveStarted = true;
    }

    private void LateUpdate()
    {
        if (!playerTransform)
            return;

        if (playerMoveStarted)
        {
            targetPosition = new Vector3(playerTransform.position.x, camHeight, playerTransform.position.z);

            if (targetPosition.x < LevelManager.minCamMarginX)
                targetPosition.x = LevelManager.minCamMarginX;

            if (targetPosition.x > LevelManager.maxCamMarginX)
                targetPosition.x = LevelManager.maxCamMarginX;

            if (targetPosition.z < LevelManager.minCamMarginZ)
                targetPosition.z = LevelManager.minCamMarginZ;

            if (targetPosition.z > LevelManager.maxCamMarginZ)
                targetPosition.z = LevelManager.maxCamMarginZ;

            transform.position = targetPosition;
        }
    }
}
