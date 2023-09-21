using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedGun : MonoBehaviour
{
    public Gun gunIdentity;
    GunHandler gunHandler;
    GameObject gun;
    public GameObject thisObj;
    SpriteRenderer spriteRenderer;
    Sprite gunSprite;

    public bool preexist = false;

    void Start()
    {
        if (preexist)
        {
            GenerateIdentity(gunIdentity);
        }
    }

    public void GenerateIdentity(Gun gunToBeIdentity)
    {
        gunIdentity = gunToBeIdentity;
        gunSprite = gunIdentity._gunSprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = gunSprite;
        gun = GameObject.FindWithTag("Gun");
        gunHandler = gun.GetComponent<GunHandler>();
        thisObj = this.gameObject;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && gunHandler.canPickUpGun)
        {
            gunHandler.PlayerPickUpGun(gunIdentity, this);
            Destroy(thisObj);
        }
    }
}
