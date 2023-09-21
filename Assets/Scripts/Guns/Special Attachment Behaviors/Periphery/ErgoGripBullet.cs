using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ErgoGripBullet : AttachmentEffect
{
    public override void OnBulletHit(GameObject enemy)
    {
        List<GameObject> bullets = new List<GameObject>();
        bullets = GameObject.FindGameObjectsWithTag("Bullet").ToList();
        this.gameObject.GetComponent<BulletHandler>().bulletDamage *= (1 + (bullets.Count / 33));
    }
}
