using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    private float laserLength;
    private float laserDamage;

    public void InitializeBullet(float laserLength, float laserDamage)
    {
        this.laserLength = laserLength;
        this.laserDamage = laserDamage;

        Vector3 scale = transform.localScale;
        scale.y = laserLength;
        transform.localScale = scale;

        Vector3 pos = transform.localPosition;
        pos.z = laserLength / 2;
        transform.localPosition = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == GameConsts.ENEMY_TAG)
        {
            GameEventManager.Instance.TriggerAsyncEvent(new DamageEnemyEvent(collision.gameObject, laserDamage));
        }
        else if(collision.gameObject.tag == GameConsts.ENEMY_BULLET_TAG)
        {
            collision.gameObject.GetComponent<EnemyBullet>().DestroyWithEffect();
        }
    }
}
