using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformedBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localScale = new Vector3(this.gameObject.GetComponent<BulletHandler>().bulletDamage, this.gameObject.GetComponent<BulletHandler>().bulletDamage, 1f);
    }

    //add an event so that when bullet damage changes so does localscale
}
