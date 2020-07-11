using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    private float laserLength;
    private float laserDamage;

    private Transform mesh;

    public void InitializeBullet(float laserLength, float laserDamage)
    {
        this.laserLength = laserLength;
        this.laserDamage = laserDamage;

        mesh = transform.GetChild(0);

        Vector3 scale = mesh.transform.localScale;
        scale.y = laserLength;
        mesh.transform.localScale = scale;

        Vector3 pos = mesh.transform.localPosition;
        pos.z = laserLength / 2;
        mesh.transform.localPosition = pos;
    }
}
