using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGeneric;

public class EnemyBulletHandler : MonoBehaviour
{
    public EnemyProjectile thisProj; //The current ScriptableObject Bullet in use

    void Start()
    {
        this.gameObject.transform.rotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            EnemyUtils enemyUtils = new EnemyUtils();
            enemyUtils.DealDamage(thisProj._damage, thisProj._armorPierce);
        }
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 5f);
        Destroy(gameObject);
    }

    void Awake()
    {
        StartCoroutine(BulletLifetime());
    }

    private IEnumerator BulletLifetime()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }
}