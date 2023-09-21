using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Red Dot Sight", menuName = "Attachment/Create Red Dot Sight", order = 1)]
public class RedDotSight : Attachment
{
    public override void OnShoot()
    {
        foreach (GameObject bullet in gunHandler.mostRecentBullet.ToList<GameObject>())
        {
            if (bullet != null)
            {
                bullet.AddComponent<RedDotSightBullet>();
            }
            else
            {
                gunHandler.mostRecentBullet.RemoveAt(gunHandler.mostRecentBullet.IndexOf(bullet));
            }
        }
    }

}