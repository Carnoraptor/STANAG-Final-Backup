using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StubbyGripBullet : AttachmentEffect
{
    public override void OnBulletHit(GameObject enemy)
    {
        if (Vector2.Distance(enemy.transform.position, GameObject.FindWithTag("Player").transform.position) < 5f)
        {
            this.gameObject.GetComponent<BulletHandler>().bulletDamage *= 2;
        }
    }
}
