using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDSBool : MonoBehaviour
{
    public bool isSlowedByRDS = false;


    public void BeginCooldown()
    {
        StartCoroutine(SlowdownCooldown());
    }

    IEnumerator SlowdownCooldown()
    {
        yield return new WaitForSeconds(2f);

        this.gameObject.GetComponent<GeneralAI>().enemySpeed /= 0.7f;
        isSlowedByRDS = false;
    }
}
