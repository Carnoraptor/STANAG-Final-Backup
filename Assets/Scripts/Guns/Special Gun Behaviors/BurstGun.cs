using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstGun", menuName = "Gun/Create New Burst Gun", order = 1)]
public class BurstGun : Gun
{
    public int bulletsInBurst;

    public override void Shoot()
    {
        gunHandler.StartCoroutine(gunHandler.Burst(bulletsInBurst));
        gunHandler.StartCoroutine(gunHandler.FireRateCheck());
    }
}
