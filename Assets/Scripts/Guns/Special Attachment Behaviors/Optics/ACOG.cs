using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utils;

[CreateAssetMenu(fileName = "ACOG", menuName = "Attachment/Create ACOG", order = 1)]
public class ACOG : Attachment
{
    public override void OnShoot()
    {
        foreach (GameObject bullet in gunHandler.mostRecentBullet.ToList<GameObject>())
        {
            if (bullet != null)
            {
                if (!Utils.GenUtils.HasComponent<ACOGBullet>(bullet))
                {
                    bullet.AddComponent<ACOGBullet>();
                }
            }
            else
            {
                gunHandler.mostRecentBullet.RemoveAt(gunHandler.mostRecentBullet.IndexOf(bullet));
            }
        }
    }

}