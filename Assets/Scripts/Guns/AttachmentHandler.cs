using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentHandler : MonoBehaviour
{
    public Attachment attachmentIdentity;
    public GameObject attachmentObj;

    [Header("Main")]
    public string attachmentName;
    public Attachment.AttachmentType attachmentType;
    public int attachmentID;

    [Header("Percentage Modifiers")]
    public float damageMod;
    public float fireRateMod;
    public float accuracyMod;
    public float armorPenMod;
    public float bulletSpeedMod;
    public int bulletsAtOnceMod;

    [Header("Flat Modifiers")]
    public int damageModFlat;
    public float fireRateModFlat;
    public float accuracyModFlat;
    public int armorPenModFlat;
    public float bulletSpeedModFlat;
    public int bulletsAtOnceModFlat;

    [Header("Graphics and Prefabs")]
    bool editsBullet;
    public GameObject newBulletPrefab;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Sprite attachmentSprite;
    public bool isAnimated;
    public AnimationClip anim;
    //public Anim gunShooting

    [Header("Misc")]
    public bool altersBulletMovementBehavior;
    public bool isAttached;

    void Awake()
    {
        attachmentObj = this.gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    public void UpdateAttachments()
    {
        attachmentName = attachmentIdentity._attachmentName;
        attachmentType = attachmentIdentity._attachmentType;
        attachmentID = attachmentIdentity._attachmentID;

        damageMod = attachmentIdentity._damageMod;
        fireRateMod = attachmentIdentity._fireRateMod;
        accuracyMod = attachmentIdentity._accuracyMod;
        armorPenMod = attachmentIdentity._armorPenMod;
        bulletSpeedMod = attachmentIdentity._bulletSpeedMod;
        bulletsAtOnceMod = attachmentIdentity._bulletsAtOnceMod;

        damageModFlat = attachmentIdentity._damageModFlat;
        fireRateModFlat = attachmentIdentity._fireRateModFlat;
        accuracyModFlat = attachmentIdentity._accuracyModFlat;
        armorPenModFlat = attachmentIdentity._armorPenModFlat;
        bulletSpeedModFlat = attachmentIdentity._bulletSpeedModFlat;
        bulletsAtOnceModFlat = attachmentIdentity._bulletsAtOnceModFlat;
    
        editsBullet = attachmentIdentity._editsBullet;
        altersBulletMovementBehavior = attachmentIdentity.altersBulletMovementBehavior;
        newBulletPrefab = attachmentIdentity._newBulletPrefab;
        attachmentSprite = attachmentIdentity._attachmentSprite;
        isAnimated = attachmentIdentity._isAnimated;
        anim = attachmentIdentity._anim;

        if (isAnimated)
        {
            //Animation animMaster = this.gameObject.AddComponent(typeof(Animation)) as Animation;
            //animMaster.AddClip(Resources.Load<AnimationClip>("Misc Loadables/Animations/ShieldGenerator/ShieldGenerator.anim"), "AttachmentAnim");
            //animMaster.Play(Resources.Load<AnimationClip>("Misc Loadables/Animations/ShieldGenerator/ShieldGenerator.anim"));
        }

        //spriteRenderer.sprite = attachmentSprite;
    }

    public void ClearAttachments()
    {
        attachmentName = "";
        attachmentID = 0;

        damageMod = 0;
        fireRateMod = 0;
        accuracyMod = 0;
        armorPenMod = 0;
        bulletSpeedMod = 0;
        bulletsAtOnceMod = 0;
    
        editsBullet = false;
        newBulletPrefab = null;
        attachmentSprite = null;
        isAnimated = false;
        anim = null;
    }
}
