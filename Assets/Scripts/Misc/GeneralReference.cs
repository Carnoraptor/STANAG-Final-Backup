using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralReference
{
    [SerializeField] public static LayerMask enemyLayers = LayerMask.GetMask("Enemy");

    public static GameObject playerGun = GameObject.FindWithTag("Gun");
}
