using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float camHeight = 10f;
    [SerializeField] private float cameraSmoothing = 0.05f;
    private Transform playerTransform;
    private bool playerMoveStarted = false;

    Vector3 targetPosition;

    private float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] private float shakeAmount = 0.7f;
    [SerializeField] private float decreaseFactor = 1.0f;

    Vector3 originalPos;
    bool shakeTriggered = false;

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<PlayerCreatedEvent>(InitializeCamera);
        GameEventManager.Instance.AddListener<ShakeCameraEvent>(OnShakeTriggered);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<PlayerCreatedEvent>(InitializeCamera);
        GameEventManager.Instance.RemoveListener<ShakeCameraEvent>(OnShakeTriggered);
    }

    private void InitializeCamera(PlayerCreatedEvent e)
    {
        playerTransform = e.transform;
        playerMoveStarted = true;
    }

    private void LateUpdate()
    {
        if (playerMoveStarted && playerTransform)
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

            transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothing);
        }

        if (shakeTriggered)
        {
            if (shakeDuration > 0)
            {
                transform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                transform.localPosition = originalPos;
                shakeTriggered = false;
            }
        }

    }

    private void OnShakeTriggered(ShakeCameraEvent e)
    {
        shakeDuration = 0.3f;
        originalPos = transform.localPosition;
        shakeTriggered = true;
    }
}
