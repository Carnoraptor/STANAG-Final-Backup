using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils;

public class InventoryHandler : MonoBehaviour
{
    public Attachment currentlyHeldAttachment;

    [SerializeField] Image muzzleSlot;
    [SerializeField] Image opticSlot;
    [SerializeField] Image lowerRailSlot;
    [SerializeField] Image sideRailSlot;

    [SerializeField] Attachment muzzleSlotAttachment;
    [SerializeField] Attachment opticSlotAttachment;
    [SerializeField] Attachment lowerRailSlotAttachment;
    [SerializeField] Attachment sideRailSlotAttachment;

    [SerializeField] Image inventorySlot1;
    [SerializeField] Image inventorySlot2;
    [SerializeField] Image inventorySlot3;
    [SerializeField] Image inventorySlot4;

    [SerializeField] Attachment inventoryAttachment1;
    [SerializeField] Attachment inventoryAttachment2;
    [SerializeField] Attachment inventoryAttachment3;
    [SerializeField] Attachment inventoryAttachment4;

    [SerializeField] GunHandler gunHandler;

    public Image draggedAttachmentImage;
    Canvas inventoryCanvas;

    void Start()
    {
        gunHandler = GameObject.FindWithTag("Gun").GetComponent<GunHandler>();
    }

    public void OnInventoryOpen()
    {
        if (draggedAttachmentImage == null)
        {
            draggedAttachmentImage = GameObject.Find("DraggedAttachmentImage").GetComponent<Image>();
        }
        if (inventoryCanvas == null)
        {
            inventoryCanvas = GameObject.Find("InventoryCanvas").GetComponent<Canvas>();
        }

        muzzleSlotAttachment = gunHandler.currentAttachmentHandlers[0].attachmentIdentity;
        opticSlotAttachment = gunHandler.currentAttachmentHandlers[1].attachmentIdentity;
        lowerRailSlotAttachment = gunHandler.currentAttachmentHandlers[2].attachmentIdentity;
        sideRailSlotAttachment = gunHandler.currentAttachmentHandlers[3].attachmentIdentity;

        UpdateAttachmentImages();
        InvokeRepeating("DragAttachment", 0f, 0.01f);
    }

    public void OnInventoryClose()
    {
        if (currentlyHeldAttachment != null)
        {
            if (currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.muzzle && muzzleSlot == null) { muzzleSlot.sprite = currentlyHeldAttachment._attachmentSprite; gunHandler.currentAttachmentHandlers[0].attachmentIdentity = currentlyHeldAttachment;} else
            if (currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.optic && opticSlot == null) { opticSlot.sprite = currentlyHeldAttachment._attachmentSprite; gunHandler.currentAttachmentHandlers[1].attachmentIdentity = currentlyHeldAttachment;} else
            if (currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.lowerRail && lowerRailSlot == null) { lowerRailSlot.sprite = currentlyHeldAttachment._attachmentSprite; gunHandler.currentAttachmentHandlers[2].attachmentIdentity = currentlyHeldAttachment;} else
            if (currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.sideRail && sideRailSlot == null) { sideRailSlot.sprite = currentlyHeldAttachment._attachmentSprite; gunHandler.currentAttachmentHandlers[3].attachmentIdentity = currentlyHeldAttachment;} else
            {
                gunHandler.PlayerDropAttachment(currentlyHeldAttachment);
            }

        currentlyHeldAttachment = null;
        }

        gunHandler.currentAttachmentHandlers[0].attachmentIdentity = muzzleSlotAttachment;
        gunHandler.currentAttachmentHandlers[1].attachmentIdentity = opticSlotAttachment;
        gunHandler.currentAttachmentHandlers[2].attachmentIdentity = lowerRailSlotAttachment;
        gunHandler.currentAttachmentHandlers[3].attachmentIdentity = sideRailSlotAttachment;

        draggedAttachmentImage.sprite = GameManager.instance.nullSprite;

        CancelInvoke();
    }

    public void UpdateAttachmentImages()
    {
        if (gunHandler.currentAttachmentHandlers[0].attachmentIdentity != null)
        {
            muzzleSlot.sprite = gunHandler.currentAttachmentHandlers[0].attachmentSprite;
        }
        if (gunHandler.currentAttachmentHandlers[1].attachmentIdentity != null)
        {
            opticSlot.sprite = gunHandler.currentAttachmentHandlers[1].attachmentSprite;
        }
        if (gunHandler.currentAttachmentHandlers[2].attachmentIdentity != null)
        {
            lowerRailSlot.sprite = gunHandler.currentAttachmentHandlers[2].attachmentSprite;
        }
        if (gunHandler.currentAttachmentHandlers[3].attachmentIdentity != null)
        {
            sideRailSlot.sprite = gunHandler.currentAttachmentHandlers[3].attachmentSprite;
        }

        if (inventoryAttachment1 != null) {inventorySlot1.sprite = inventoryAttachment1._attachmentSprite;}
        if (inventoryAttachment2 != null) {inventorySlot2.sprite = inventoryAttachment2._attachmentSprite;}
        if (inventoryAttachment3 != null) {inventorySlot3.sprite = inventoryAttachment3._attachmentSprite;}
        if (inventoryAttachment4 != null) {inventorySlot4.sprite = inventoryAttachment4._attachmentSprite;}

        if (inventoryAttachment1 == null) {inventorySlot1.sprite = GameManager.instance.nullSprite;}
        if (inventoryAttachment2 == null) {inventorySlot2.sprite = GameManager.instance.nullSprite;}
        if (inventoryAttachment3 == null) {inventorySlot3.sprite = GameManager.instance.nullSprite;}
        if (inventoryAttachment4 == null) {inventorySlot4.sprite = GameManager.instance.nullSprite;}
        
        if (currentlyHeldAttachment != null)
        {
            draggedAttachmentImage.sprite = currentlyHeldAttachment._attachmentSprite;
        }
        else
        {
            draggedAttachmentImage.sprite = GameManager.instance.nullSprite;
        }
    }

    public void AttachClick(string atchSlot)
    {
        Debug.Log("Clicked on an attachment slot!");
        if (currentlyHeldAttachment == null)
        {
            if (atchSlot == "muzzle")
            {
                currentlyHeldAttachment = gunHandler.currentAttachmentHandlers[0].attachmentIdentity;
                muzzleSlot.sprite = GameManager.instance.nullSprite;
                gunHandler.currentAttachmentHandlers[0].attachmentIdentity = null;
                muzzleSlotAttachment = null;
                gunHandler.currentAttachmentHandlers[0].attachmentIdentity = muzzleSlotAttachment;
                CloseTooltip();
            }
            if (atchSlot == "optic")
            {
                currentlyHeldAttachment = gunHandler.currentAttachmentHandlers[1].attachmentIdentity;
                opticSlot.sprite = GameManager.instance.nullSprite;
                gunHandler.currentAttachmentHandlers[1].attachmentIdentity = null;
                opticSlotAttachment = null;
                gunHandler.currentAttachmentHandlers[1].attachmentIdentity = muzzleSlotAttachment;
                CloseTooltip();
            }
            if (atchSlot == "lowerRail")
            {
                currentlyHeldAttachment = gunHandler.currentAttachmentHandlers[2].attachmentIdentity;
                lowerRailSlot.sprite = GameManager.instance.nullSprite;
                gunHandler.currentAttachmentHandlers[2].attachmentIdentity = null;
                lowerRailSlotAttachment = null;
                gunHandler.currentAttachmentHandlers[2].attachmentIdentity = muzzleSlotAttachment;
                CloseTooltip();
            }
            if (atchSlot == "sideRail")
            {
                currentlyHeldAttachment = gunHandler.currentAttachmentHandlers[3].attachmentIdentity;
                sideRailSlot.sprite = GameManager.instance.nullSprite;
                gunHandler.currentAttachmentHandlers[3].attachmentIdentity = null;
                sideRailSlotAttachment = null;
                gunHandler.currentAttachmentHandlers[3].attachmentIdentity = muzzleSlotAttachment;
                CloseTooltip();
            }
            UpdateAttachmentImages();
            return;
        }

        if (atchSlot == "muzzle" && currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.muzzle)
        {
            muzzleSlot.sprite = currentlyHeldAttachment._attachmentSprite;
            if (gunHandler.currentAttachmentHandlers[0].attachmentIdentity != null)
            {
                Attachment queuedAttachment = gunHandler.currentAttachmentHandlers[0].attachmentIdentity;
                muzzleSlotAttachment = currentlyHeldAttachment;
                currentlyHeldAttachment = queuedAttachment;
            }
            else
            {
                muzzleSlotAttachment = currentlyHeldAttachment;
                gunHandler.currentAttachmentHandlers[0].attachmentIdentity = currentlyHeldAttachment;
                currentlyHeldAttachment = null;
            }
            gunHandler.currentAttachmentHandlers[0].attachmentIdentity = muzzleSlotAttachment;
        }

        if (atchSlot == "optic" && currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.optic)
        {
            opticSlot.sprite = currentlyHeldAttachment._attachmentSprite;

            if (gunHandler.currentAttachmentHandlers[1].attachmentIdentity != null)
            {
                Attachment queuedAttachment = gunHandler.currentAttachmentHandlers[1].attachmentIdentity;
                opticSlotAttachment = currentlyHeldAttachment;
                currentlyHeldAttachment = queuedAttachment;
            }
            else
            {
                opticSlotAttachment = currentlyHeldAttachment;
                gunHandler.currentAttachmentHandlers[1].attachmentIdentity = currentlyHeldAttachment;
                currentlyHeldAttachment = null;
            }
            gunHandler.currentAttachmentHandlers[1].attachmentIdentity = muzzleSlotAttachment;
        }

        if (atchSlot == "lowerRail" && currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.lowerRail)
        {
            lowerRailSlot.sprite = currentlyHeldAttachment._attachmentSprite;

            if (gunHandler.currentAttachmentHandlers[2].attachmentIdentity != null)
            {
                Attachment queuedAttachment = gunHandler.currentAttachmentHandlers[2].attachmentIdentity;
                lowerRailSlotAttachment = currentlyHeldAttachment;
                currentlyHeldAttachment = queuedAttachment;
            }
            else
            {
                lowerRailSlotAttachment = currentlyHeldAttachment;
                gunHandler.currentAttachmentHandlers[2].attachmentIdentity = currentlyHeldAttachment;
                currentlyHeldAttachment = null;
            }
            gunHandler.currentAttachmentHandlers[2].attachmentIdentity = muzzleSlotAttachment;
        }

        if (atchSlot == "sideRail" && currentlyHeldAttachment._attachmentType == Attachment.AttachmentType.sideRail)
        {
            sideRailSlot.sprite = currentlyHeldAttachment._attachmentSprite;

            if (gunHandler.currentAttachmentHandlers[3].attachmentIdentity != null)
            {
                Attachment queuedAttachment = gunHandler.currentAttachmentHandlers[3].attachmentIdentity;
                sideRailSlotAttachment = currentlyHeldAttachment;
                currentlyHeldAttachment = queuedAttachment;
            }
            else
            {
                lowerRailSlotAttachment = currentlyHeldAttachment;
                gunHandler.currentAttachmentHandlers[3].attachmentIdentity = currentlyHeldAttachment;
                currentlyHeldAttachment = null;
            }
            gunHandler.currentAttachmentHandlers[3].attachmentIdentity = muzzleSlotAttachment;
        }

        if (currentlyHeldAttachment != null)
        {
            draggedAttachmentImage.sprite = currentlyHeldAttachment._attachmentSprite;
        }
        else
        {
            draggedAttachmentImage.sprite = GameManager.instance.nullSprite;
        }

        gunHandler.UpdateGun();
    }


    public void InventorySlotClick(int slot)
    {
        Debug.Log(slot);
        
        if (currentlyHeldAttachment != null && slot == 1 && inventoryAttachment1 == null) { inventoryAttachment1 = currentlyHeldAttachment; currentlyHeldAttachment = null;} else
        if (currentlyHeldAttachment != null && slot == 2 && inventoryAttachment2 == null) { inventoryAttachment2 = currentlyHeldAttachment; currentlyHeldAttachment = null;} else
        if (currentlyHeldAttachment != null && slot == 3 && inventoryAttachment3 == null) { inventoryAttachment3 = currentlyHeldAttachment; currentlyHeldAttachment = null;} else
        if (currentlyHeldAttachment != null && slot == 4 && inventoryAttachment4 == null) { inventoryAttachment4 = currentlyHeldAttachment; currentlyHeldAttachment = null;} else

        if (currentlyHeldAttachment != null && slot == 1 && inventoryAttachment1 != null) { Attachment queuedAttachment = inventoryAttachment1; inventoryAttachment1 = currentlyHeldAttachment; currentlyHeldAttachment = queuedAttachment;} else
        if (currentlyHeldAttachment != null && slot == 2 && inventoryAttachment2 != null) { Attachment queuedAttachment = inventoryAttachment2; inventoryAttachment2 = currentlyHeldAttachment; currentlyHeldAttachment = queuedAttachment;} else
        if (currentlyHeldAttachment != null && slot == 3 && inventoryAttachment3 != null) { Attachment queuedAttachment = inventoryAttachment3; inventoryAttachment3 = currentlyHeldAttachment; currentlyHeldAttachment = queuedAttachment;} else
        if (currentlyHeldAttachment != null && slot == 4 && inventoryAttachment4 != null) { Attachment queuedAttachment = inventoryAttachment4; inventoryAttachment4 = currentlyHeldAttachment; currentlyHeldAttachment = queuedAttachment;} else
        
        if (currentlyHeldAttachment == null && slot == 1 && inventoryAttachment1 != null) { currentlyHeldAttachment = inventoryAttachment1; inventoryAttachment1 = null; } else
        if (currentlyHeldAttachment == null && slot == 2 && inventoryAttachment2 != null) { currentlyHeldAttachment = inventoryAttachment2; inventoryAttachment2 = null; } else
        if (currentlyHeldAttachment == null && slot == 3 && inventoryAttachment3 != null) { currentlyHeldAttachment = inventoryAttachment3; inventoryAttachment3 = null; } else
        if (currentlyHeldAttachment == null && slot == 4 && inventoryAttachment4 != null) { currentlyHeldAttachment = inventoryAttachment4; inventoryAttachment4 = null; }

        UpdateAttachmentImages();
    }

    
    public void DragAttachment()
    {
        if (currentlyHeldAttachment != null)
        {
            CanvasScaler scaler = inventoryCanvas.GetComponent<CanvasScaler>();
            draggedAttachmentImage.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((Input.mousePosition.x * scaler.referenceResolution.x / Screen.width) - (scaler.referenceResolution.x / 2), (Input.mousePosition.y * scaler.referenceResolution.y / Screen.height)  - (scaler.referenceResolution.y / 2));
            // original line above (causes weird offset) -- draggedAttachmentImage.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);
        }
    }

    public void OpenTooltip(string slot)
    {
        Debug.Log("HOVERING OVER " + slot);

        switch(slot)
        {
            case "MuzzleTip":
            if (muzzleSlotAttachment != null) {
                
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(muzzleSlotAttachment._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(muzzleSlotAttachment._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(muzzleSlotAttachment._attachmentDescription));
            }
            break;
            case "OpticTip":
            if (opticSlotAttachment != null) {
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(opticSlotAttachment._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(opticSlotAttachment._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(opticSlotAttachment._attachmentDescription));
            }
            break;
            case "LowerRailTip":
            if (lowerRailSlotAttachment != null) {
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(lowerRailSlotAttachment._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(lowerRailSlotAttachment._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(lowerRailSlotAttachment._attachmentDescription));
            }
            break;
            case "SideRailTip":
            if (sideRailSlotAttachment != null) {
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(sideRailSlotAttachment._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(sideRailSlotAttachment._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(sideRailSlotAttachment._attachmentDescription));
            }
            break;
            case "InventorySlot1Tip":
            if (inventoryAttachment1 != null) {
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(inventoryAttachment1._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(inventoryAttachment1._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(inventoryAttachment1._attachmentDescription));
            }
            break;
            case "InventorySlot2Tip":
            if (inventoryAttachment2 != null) {
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(inventoryAttachment2._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(inventoryAttachment2._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(inventoryAttachment2._attachmentDescription));
            }
            break;
            case "InventorySlot3Tip":
            if (inventoryAttachment3 != null) {
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(inventoryAttachment3._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(inventoryAttachment3._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(inventoryAttachment3._attachmentDescription));
            }
            break;
            case "InventorySlot4Tip":
            if (inventoryAttachment4 != null) {
                gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(inventoryAttachment4._attachmentName);

                gunHandler.gunDescText.text = (Utils.GenUtils.PeriodToLineBreak(inventoryAttachment4._attachmentEffectDesc) + "////////////////////////\n\n" + Utils.GenUtils.PeriodToLineBreak(inventoryAttachment4._attachmentDescription));
            }
            break;
        }
    }

    public void CloseTooltip()
    {
        gunHandler.gunNameText.text = Utils.GenUtils.StringToLowerCase(gunHandler.gunName);

        string literallyJustAPeriod = ".";
        char[] aMerePeriod = literallyJustAPeriod.ToCharArray();

        string updatedDesc = Utils.GenUtils.PeriodToLineBreak(gunHandler.gunDesc);

        string finalDesc = "";
        finalDesc = "Damage: " + gunHandler.damage + "\nFire Rate: " + (gunHandler.fireRate * 60) + "RPM" + "\nAccuracy: " + (100 - gunHandler.inaccuracy) + "\nArmor Penetration: " + gunHandler.armorPen + "\n\n////////////////////////\n\n" + updatedDesc;


        gunHandler.gunDescText.text = finalDesc;

    }




    
}