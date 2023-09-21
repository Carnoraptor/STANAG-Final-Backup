using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingBullet : AttachmentEffect
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float force = 1f;

    
    void Start()
    {
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
        force = GameObject.FindWithTag("Gun").GetComponent<GunHandler>().bulletSpeed / 10;
        if (force < 1f)
        {
            force = 1f;
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
        force *= 1.1f;
        rb.AddForce(transform.right * force);
    }
}
