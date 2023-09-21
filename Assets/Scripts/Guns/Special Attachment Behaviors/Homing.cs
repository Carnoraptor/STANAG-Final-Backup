using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Homing", menuName = "Attachment/Create New Homing Attachment", order = 1)]
public class Homing : Attachment
{
    [Header("Homing Variables")]
    public float angleChangingSpeed;
    public float homeStrength;
    public float curveSpeed; //curve power

    public override void OnShoot()
    {
        foreach (GameObject bullet in gunHandler.mostRecentBullet)
        {
            if (bullet != null)
            {
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                bullet.AddComponent<HomingBullet>();
                bullet.GetComponent<HomingBullet>().StartCoroutine(bullet.GetComponent<HomingBullet>().HomeEnemy(bullet, rb, angleChangingSpeed, curveSpeed, homeStrength)); //fix
            }
            else
            {
                gunHandler.mostRecentBullet.RemoveAt(gunHandler.mostRecentBullet.IndexOf(bullet));
            }
        }

        gunHandler.ClearMRB();
    }

}