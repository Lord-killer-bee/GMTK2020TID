using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorWall : MonoBehaviour
{
    [SerializeField] private AudioClip deflect;


    [SerializeField] private GameObject impactEffectPref;

    private List<GameObject> impactObjects = new List<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == GameConsts.ENEMY_BULLET_TAG)
        {
            GetComponentInParent<AudioSource>().PlayOneShot(deflect);
            var point = collision.contacts[0].point;
            var dir = -collision.contacts[0].normal;

            point -= dir;
            RaycastHit hitInfo;
            if (collision.collider.Raycast(new Ray(point, dir), out hitInfo, 2))
            {
                var normal = hitInfo.normal;
                collision.gameObject.GetComponentInParent<EnemyBullet>().ReflectBullet(normal);

                GameEventManager.Instance.TriggerAsyncEvent(new ShakeCameraEvent());

                GameObject impactObj = Instantiate(impactEffectPref, Vector3.zero, Quaternion.identity, transform);
                impactObj.transform.position = hitInfo.point;
                impactObj.transform.localEulerAngles = new Vector3(90, 0, 0);
                impactObj.GetComponent<DestroyEffects>().InitiateDestroy();

                impactObjects.Add(impactObj);
            }
        }
    }
}
