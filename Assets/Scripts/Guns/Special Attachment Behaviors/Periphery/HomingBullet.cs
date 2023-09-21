using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public static HomingBullet homing;

    void Start()
    {
        HomingBullet.homing = this;
        Debug.Log("homing time");
    }
    
    public IEnumerator HomeEnemy(GameObject thisObj, Rigidbody2D rb, float angleChangingSpeed, float movementSpeed, float homeStrength)
    {
        rb.AddForce(GameObject.FindWithTag("Gun").GetComponent<GunHandler>().bulletOrigin.right * GameObject.FindWithTag("Gun").GetComponent<GunHandler>().bulletSpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        while(true)
        {
            NearestEnemy(thisObj, rb, angleChangingSpeed, movementSpeed, homeStrength);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void TrackEnemy(GameObject target, GameObject thisObj, Rigidbody2D rb, float angleChangingSpeed, float movementSpeed, float homeStrength)
    {
        Debug.Log("tracking enemy");
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        float rotateAmount = (Vector3.Cross(direction, thisObj.transform.right).z * (1 / (Vector2.Distance(transform.position, target.transform.position) * 0.2f)));

        Vector2 force = Vector2.right * GameObject.FindWithTag("Gun").GetComponent<GunHandler>().bulletSpeed;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddTorque(Mathf.Deg2Rad * (rotateAmount * angleChangingSpeed) * -1f);
        rb.AddRelativeForce(force, ForceMode2D.Impulse);
    }

    void NearestEnemy(GameObject thisObj, Rigidbody2D rb, float angleChangingSpeed, float movementSpeed, float homeStrength)
    {
        Debug.Log("finding enemy");
        //Vector2 transform = new Vector2(thisObj.transform.position.x, thisObj.transform.position.y);
        Collider2D[] targets = Physics2D.OverlapCircleAll(thisObj.transform.position, homeStrength, GeneralReference.enemyLayers);

        if (targets.Length > 0)
        {
            GameObject closestTarget = targets[0].gameObject;   
            Debug.Log("enemy found " + closestTarget.name);

            foreach (Collider2D enemy in targets)
            {
                if (Vector2.Distance(thisObj.transform.position, enemy.gameObject.transform.position) < Vector2.Distance(thisObj.transform.position, closestTarget.transform.position))
                {
                    closestTarget = enemy.gameObject;
                }
            }

            //Debug.Log(closestTarget.name);
            TrackEnemy(closestTarget, thisObj, rb, angleChangingSpeed, movementSpeed, homeStrength);
        }
        else
        {
            
        }
    }
}