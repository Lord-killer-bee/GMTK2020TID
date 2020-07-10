using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    Vector3 moveDirection = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;

    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal"); 
        moveDirection.z = Input.GetAxisRaw("Vertical");

        targetPosition = moveDirection * moveSpeed * Time.deltaTime;

        transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));

        if (targetPosition.x + transform.position.x < LevelManager.minMarginX)
            targetPosition.x = 0;

        if (targetPosition.x + transform.position.x > LevelManager.maxMarginX)
            targetPosition.x = 0;

        if (targetPosition.z + transform.position.z < LevelManager.minMarginZ)
            targetPosition.z = 0;

        if (targetPosition.z + transform.position.z > LevelManager.maxMarginZ)
            targetPosition.z = 0;

        transform.Translate(targetPosition, Space.World);
    }
}
