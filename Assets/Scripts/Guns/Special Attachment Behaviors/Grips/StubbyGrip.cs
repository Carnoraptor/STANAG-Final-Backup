using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Stubby Grip", menuName = "Attachment/Create Stubby Grip", order = 1)]
public class StubbyGrip : Attachment
{
    public override void OnShoot()
    {
        foreach (GameObject bullet in gunHandler.mostRecentBullet.ToList<GameObject>())
        {
            if (bullet != null)
            {
                bullet.AddComponent<StubbyGripBullet>();
            }
            else
            {
                gunHandler.mostRecentBullet.RemoveAt(gunHandler.mostRecentBullet.IndexOf(bullet));
            }
        }
    }
}
