using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prism Sight", menuName = "Attachment/Create Prism Sight", order = 1)]
public class PrismSight : Attachment
{
    bool resetOnFin = false;
    // Start is called before the first frame update
    public override void OnShoot()
    {
        resetOnFin = false;
        if (GameObject.FindWithTag("Player").GetComponent<Player>().currentSpeed > (GameObject.FindWithTag("Player").GetComponent<Player>().moveSpeed - 2))
        {
            gunHandler.inaccuracy *= (0.8f);
            resetOnFin = true;
        }
    }

    public override void OnFinShoot()
    {   
        if (resetOnFin)
        {
            gunHandler.inaccuracy /= (0.8f);
        }
    }
}
