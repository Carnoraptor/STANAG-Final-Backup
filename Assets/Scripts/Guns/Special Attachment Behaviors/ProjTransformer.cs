using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utils;

[CreateAssetMenu(fileName = "ProjTransformer", menuName = "Attachment/Create New Transforming Attachment", order = 1)]
public class ProjTransformer : Attachment
{
    public override void OnShoot()
    {
        foreach (GameObject bullet in gunHandler.mostRecentBullet.ToList<GameObject>())
        {
            if (bullet != null)
            {
                if (!Utils.GenUtils.HasComponent<TransformedBullet>(bullet))
                {
                    bullet.AddComponent<TransformedBullet>();
                }
            }
            else
            {
                gunHandler.mostRecentBullet.RemoveAt(gunHandler.mostRecentBullet.IndexOf(bullet));
            }
        }
    }

}