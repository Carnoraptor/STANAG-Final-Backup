using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Bloodstained Grip", menuName = "Attachment/Create Bloodstained Grip", order = 1)]
public class BloodstainedGrip : Attachment
{
    public GameObject coroutinePrefab;

    public override void OnEnemyKill(GameObject enemy)
    {
        gunHandler.damage *= 1.2f;
        GameObject coPrefab = Instantiate(coroutinePrefab);
        coPrefab.GetComponent<BloodstainedGripCoroutine>().BeginCoroutine(this);
        //StartCoroutine(BloodCooldown());
    }

    public void BloodCooldown()
    {
        gunHandler.damage /= 1.2f;
    }
}

//Idea: Make it so the gun either gives off particles when buffed or turns slightly red or even both