using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


public class RedDotSightBullet : AttachmentEffect
{
    public override void OnBulletHit(GameObject enemy)
    {
        if (!Utils.GenUtils.HasComponent<RDSBool>(enemy))
        {
            enemy.AddComponent<RDSBool>();
        }

        if (!enemy.GetComponent<RDSBool>().isSlowedByRDS)
        {
            enemy.GetComponent<RDSBool>().isSlowedByRDS = true;
            enemy.GetComponent<GeneralAI>().enemySpeed *= 0.7f;

            enemy.GetComponent<RDSBool>().BeginCooldown();
        }
    }
}
