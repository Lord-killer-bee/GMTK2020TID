using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    Vector3 moveDirection = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;
    float minX = -5;
    float maxX = 5;
    float minZ = -5;
    float maxZ = 5;

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal"); 
        moveDirection.z = Input.GetAxisRaw("Vertical");

        targetPosition = moveDirection * moveSpeed * Time.deltaTime;

        transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));

        if (targetPosition.x + transform.position.x < minX)
            targetPosition.x = 0;

        if (targetPosition.x + transform.position.x > maxX)
            targetPosition.x = 0;

        if (targetPosition.z + transform.position.z < minZ)
            targetPosition.z = 0;

        if (targetPosition.z + transform.position.z > maxZ)
            targetPosition.z = 0;

        transform.Translate(targetPosition, Space.World);
    }
}
