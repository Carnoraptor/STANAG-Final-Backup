using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACOGBullet : AttachmentEffect
{
    public override void OnBulletHit(GameObject enemy)
    {
        this.gameObject.GetComponent<BulletHandler>().bulletDamage += (this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude / 2);
    }
}
