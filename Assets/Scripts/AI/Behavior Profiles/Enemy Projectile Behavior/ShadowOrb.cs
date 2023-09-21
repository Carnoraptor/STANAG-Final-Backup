using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGeneric;

public class ShadowOrb : MonoBehaviour
{
    GameObject target;
    Vector2 targPos;

    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindWithTag("Player");
        targPos = target.transform.position;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.7f);
        Charge();
    }

    void PointAtPlayer()
    {
        float offset = 0; //can be messed with

        Vector2 targetPos = targPos;
        Vector2 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }

    void Charge()
    {
        PointAtPlayer();
        Rigidbody2D projRB = this.gameObject.GetComponent<Rigidbody2D>();
        projRB.AddForce(this.gameObject.transform.right * 35f, ForceMode2D.Impulse);
        StartCoroutine(DeathCooldown());
    }

    IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            EnemyUtils enemyUtils = new EnemyUtils();
            enemyUtils.DealDamage(25, 10);
        }
    }
}
