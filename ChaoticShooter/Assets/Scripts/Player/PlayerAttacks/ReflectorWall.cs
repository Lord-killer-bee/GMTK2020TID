using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == GameConsts.ENEMY_BULLET_TAG)
        {
            // find collision point and normal. You may want to average over all contacts
            var point = collision.contacts[0].point;
            var dir = -collision.contacts[0].normal; // you need vector pointing TOWARDS the collision, not away from it
                                                // step back a bit
            point -= dir;
            RaycastHit hitInfo;
            // cast a ray twice as far as your step back. This seems to work in all
            // situations, at least when speeds are not ridiculously big
            if (collision.collider.Raycast(new Ray(point, dir), out hitInfo, 2))
            {
                // this is the collider surface normal
                var normal = hitInfo.normal;
                // this is the collision angle
                // you might want to use .velocity instead of .forward here, but it 
                // looks like it's already changed due to bounce in OnCollisionEnter
                var angle = Vector3.Angle(-transform.forward, normal);

                collision.gameObject.GetComponentInParent<EnemyBullet>().ReflectBullet(normal);
            }
        }
    }
}
