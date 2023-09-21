using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TestingGunBehavior : Gun
{
    public override void Shoot()
    {
        Debug.Log("real");
    }
}
