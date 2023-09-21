using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EnemyGeneric;

public class BulletHandler : MonoBehaviour
{
    public GameObject gun; //The gun object
    public GunHandler gunHandler; //The GunHandler script on the gun object

    public Bullet thisBullet; //The current ScriptableObject Bullet in use

    public SpriteRenderer spriteRenderer; //This object's SpriteRenderer

    public float bulletDamage = 0;

    public float bulletAP = 0;

    void Start()
    {
        gun = GameObject.FindWithTag("Gun");
        gunHandler = gun.GetComponent<GunHandler>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = thisBullet._bulletSprite;
        thisBullet = gunHandler.currentBullet;

        bulletDamage = gunHandler.damage;
        bulletAP = gunHandler.armorPen;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            List<AttachmentEffect> attachmentEffects = new List<AttachmentEffect>();
            attachmentEffects = this.gameObject.GetComponents<AttachmentEffect>().ToList();
            foreach (AttachmentEffect attachmentEffect in attachmentEffects)
            {
                attachmentEffect.OnBulletHit(col.gameObject);
            }
            var AI = col.gameObject;
            GeneralAI genAI = AI.GetComponent<GeneralAI>();
            AI.GetComponent<GeneralAI>().TakeDamage( Mathf.RoundToInt(bulletDamage), Mathf.RoundToInt(bulletAP));
            Instantiate(thisBullet._bloodEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
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
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
