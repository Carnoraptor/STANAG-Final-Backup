using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Ergo Grip", menuName = "Attachment/Create Ergo Grip", order = 1)]
public class ErgoGrip : Attachment
{
    public override void OnShoot()
    {
        foreach (GameObject bullet in gunHandler.mostRecentBullet.ToList<GameObject>())
        {
            if (bullet != null)
            {
                bullet.AddComponent<ErgoGripBullet>();
            }
            else
            {
                gunHandler.mostRecentBullet.RemoveAt(gunHandler.mostRecentBullet.IndexOf(bullet));
            }
        }
    }
}
