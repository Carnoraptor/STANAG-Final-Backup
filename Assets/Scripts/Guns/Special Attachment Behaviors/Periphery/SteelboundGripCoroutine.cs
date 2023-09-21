using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelboundGripCoroutine : MonoBehaviour
{
    public void BeginCoroutine(SteelboundGrip steelboundGrip)
    {
        StartCoroutine(Wait3Seconds(steelboundGrip));
    }

    IEnumerator Wait3Seconds(SteelboundGrip steelboundGrip)
    {
        yield return new WaitForSeconds(3);
        steelboundGrip.SteelCooldown();
    }
}
