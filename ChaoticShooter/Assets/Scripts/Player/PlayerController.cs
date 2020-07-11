using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float playerWidth = 0.5f;
    [SerializeField] private float maxPlayerHP = 1;

    Vector3 moveDirection = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;

    private float playerHP;

    private Vector3 velocity;

    void Start()
    {
        GameEventManager.Instance.TriggerSyncEvent(new PlayerCreatedEvent(transform));
        playerHP = maxPlayerHP;
    }

    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal"); 
        moveDirection.z = Input.GetAxisRaw("Vertical");

        targetPosition = moveDirection * moveSpeed * Time.deltaTime;

        transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));

        if (targetPosition.x + transform.position.x - playerWidth < LevelManager.minMarginX)
            targetPosition.x = 0;

        if (targetPosition.x + transform.position.x + playerWidth > LevelManager.maxMarginX)
            targetPosition.x = 0;

        if (targetPosition.z + transform.position.z - playerWidth < LevelManager.minMarginZ)
            targetPosition.z = 0;

        if (targetPosition.z + transform.position.z + playerWidth > LevelManager.maxMarginZ)
            targetPosition.z = 0;

        transform.Translate(targetPosition, Space.World);
        velocity = targetPosition / Time.deltaTime;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag == GameConsts.ENEMY_BULLET_TAG) || (collision.gameObject.tag == GameConsts.ENEMY_TAG))
        {
            playerHP--;
            if(playerHP <= 0)
            {
                GameEventManager.Instance.TriggerSyncEvent(new PlayerDestroyedEvent(transform));

                Destroy(gameObject);
            }
        }
    }
}
