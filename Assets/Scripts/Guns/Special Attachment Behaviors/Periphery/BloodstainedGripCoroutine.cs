using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodstainedGripCoroutine : MonoBehaviour
{
    public void BeginCoroutine(BloodstainedGrip bloodstainedGrip)
    {
        StartCoroutine(Wait3Seconds(bloodstainedGrip));
    }

    IEnumerator Wait3Seconds(BloodstainedGrip bloodstainedGrip)
    {
        yield return new WaitForSeconds(3);
        bloodstainedGrip.BloodCooldown();
    }
}
