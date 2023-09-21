using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedAttachment : MonoBehaviour
{
    public Attachment attachmentIdentity;
    //AttachmentHandler attachmentHandler;
    GunHandler gunHandler;
    GameObject gun;
    public GameObject thisObj;
    SpriteRenderer spriteRenderer;
    Sprite attachmentSprite;

    void Start()
    {
        GenerateIdentity(attachmentIdentity);
    }

    public void GenerateIdentity(Attachment attachmentToBeIdentity)
    {
        attachmentIdentity = attachmentToBeIdentity;
        attachmentSprite = attachmentIdentity._attachmentSprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = attachmentSprite;
        gun = GameObject.FindWithTag("Gun");
        gunHandler = gun.GetComponent<GunHandler>();
        thisObj = this.gameObject;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && gunHandler.canPickUpAttachment)
        {
            CheckIfCompatible();
        }
    }

    void CheckIfCompatible()
    {
        if (attachmentIdentity._attachmentType == Attachment.AttachmentType.muzzle && gunHandler.currentAttachmentHandlers[0].attachmentIdentity == null)
        {
            gunHandler.PlayerPickUpAttachment(attachmentIdentity, this);
            Destroy(thisObj);
        }
        else if (attachmentIdentity._attachmentType == Attachment.AttachmentType.optic && gunHandler.currentAttachmentHandlers[1].attachmentIdentity == null)
        {
            gunHandler.PlayerPickUpAttachment(attachmentIdentity, this);
            Destroy(thisObj);
        }
        else if(attachmentIdentity._attachmentType == Attachment.AttachmentType.sideRail && gunHandler.currentAttachmentHandlers[3].attachmentIdentity == null)
        {
            gunHandler.PlayerPickUpAttachment(attachmentIdentity, this);
            Destroy(thisObj);
        }
        else if(attachmentIdentity._attachmentType == Attachment.AttachmentType.lowerRail && gunHandler.currentAttachmentHandlers[2].attachmentIdentity == null)
        {
            gunHandler.PlayerPickUpAttachment(attachmentIdentity, this);
            Destroy(thisObj);
        }
        else
        {

        }
    }
}