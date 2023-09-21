using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Steelbound Grip", menuName = "Attachment/Create Steelbound Grip", order = 1)]
public class SteelboundGrip : Attachment
{
    public GameObject coroutinePrefab;

    public override void OnEnemyKill(GameObject enemy)
    {
        gunHandler.damage *= 1.2f;
        GameObject coPrefab = Instantiate(coroutinePrefab);
        coPrefab.GetComponent<SteelboundGripCoroutine>().BeginCoroutine(this);
        //StartCoroutine(BloodCooldown());
    }

    public void SteelCooldown()
    {
        gunHandler.damage /= 1.2f;
    }
}

//Idea: Make it so the gun either gives off particles when buffed or turns slightly red or even both