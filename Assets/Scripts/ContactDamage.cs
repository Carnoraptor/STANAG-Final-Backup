using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGeneric;

public class ContactDamage : MonoBehaviour
{
    public int contactDamage = 1;
    public int contactAP = 99;
    private EnemyUtils enemyUtils = new EnemyUtils();

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            enemyUtils.DealDamage(contactDamage, contactAP);
        }
    }
}
