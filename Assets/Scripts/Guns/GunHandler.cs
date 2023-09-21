using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    public Gun currentGun;
    public Bullet currentBullet;

    [Header("Gun")]
    [Header("Main")]
    public string gunName;
    public Gun.GunType gunType;
    public string gunDesc;

    [Header("Stats")]
    public float damage;
    public float fireRate;
    public float inaccuracy;
    public float armorPen;
    public float bulletSpeed;
    public int bulletsAtOnce;
    public Gun.FireMode fireMode;

    [Header("Graphics and Prefabs")]
    public GameObject bulletPrefab;
    public Sprite gunSprite;
    public Vector2 bulletOriginOffset;
    private GameObject bulletOriginObj;
    public Transform bulletOrigin;
    public Transform gunPos;
    public float radius;
    SpriteRenderer spriteRenderer;
    public Sprite[] muzzleFlashes;
    public GameObject muzzleFlashObj;
    public GameObject generalFlashObj;
    //public GameObject attachmentPrefab;
    //Dropped Object prefabs
    public GameObject droppedGun;
    public GameObject droppedAttachment;


    [Header("Attachments")]
    //public List<GameObject> currentAttachmentObjs = new List<GameObject>();
    public List<Attachment> currentAttachments = new List<Attachment>();
    public List<AttachmentHandler> currentAttachmentHandlers = new List<AttachmentHandler>();
    Vector2 muzzleLoc, opticLoc, lowerRailLoc, sideRailLoc;
    bool hasMuzzleSlot, hasOpticSlot, hasLowerRailSlot, hasSideRailSlot;
    [HideInInspector] public AttachmentHandler muzzleHandler, opticHandler, LRHandler, SRHandler;

    [Header("Backend Variables")]
    //Gun Backend
    Rigidbody2D rb2d;
    public int gunDir; //Direction of gun
    float scaleX = 1;
    float scaleY = 1;
    bool canShoot = true;

    public List<GameObject> mostRecentBullet = new List<GameObject>();

    //Universal Backends
    public Vector2 cursorPos;
    Camera cam;
    public float normalZoom = 9f;

    //Inventory Backends
    public bool canPickUpGun = true;
    public bool canPickUpAttachment = true;

    //Player Backends
    GameObject playerObj;
    Vector2 playerPos;
    Player playerScript;

    [Header("Inventory")]
    public Gun gun1;
    public Gun gun2;
    public Gun gun3;
    public Gun gun4;
    public Gun gun5;
    public Gun gun6;
    public List<Attachment> gun1Attachments = new List<Attachment>();
    public List<Attachment> gun2Attachments = new List<Attachment>();
    public List<Attachment> gun3Attachments = new List<Attachment>();
    public List<Attachment> gun4Attachments = new List<Attachment>();
    public List<Attachment> gun5Attachments = new List<Attachment>();
    public List<Attachment> gun6Attachments = new List<Attachment>();

    InventoryHandler inventoryHandler;
    

//░██████╗████████╗░█████╗░██████╗░████████╗ 
//██╔════╝╚══██╔══╝██╔══██╗██╔══██╗╚══██╔══╝ 
//╚█████╗░░░░██║░░░███████║██████╔╝░░░██║░░░ 
//░╚═══██╗░░░██║░░░██╔══██║██╔══██╗░░░██║░░░ 
//██████╔╝░░░██║░░░██║░░██║██║░░██║░░░██║░░░ 
//╚═════╝░░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░

    private static GunHandler gunInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (gunInstance == null) 
        {
            gunInstance = this;
        } 
        else 
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        //Gun Assignments
        gunName = currentGun._gunName;
        gunType = currentGun._gunType; 
        currentBullet = currentGun._bullet;
        gunDesc = currentGun._gunDesc; 

        //Base Stat Assignments
        damage = currentGun._damage; 
        fireRate = currentGun._fireRate; 
        inaccuracy = currentGun._inaccuracy; 
        armorPen = currentGun._armorPen; 
        bulletSpeed = currentGun._bulletSpeed;
        bulletsAtOnce = currentGun._bulletsAtOnce;
        fireMode = currentGun._fireMode;

        //Graphics/Prefabs Assignments
        bulletPrefab = currentGun._bulletPrefab;
        gunSprite = currentGun._gunSprite;
        bulletOriginOffset = currentGun._bulletOriginOffset;
        radius = currentGun._radius;
        bulletOriginObj = GameObject.Find("BulletOrigin");
        bulletOrigin = bulletOriginObj.transform;
        gunPos = this.gameObject.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = gunSprite;

        muzzleFlashObj = GameObject.Find("MuzzleFlash");
        generalFlashObj = GameObject.Find("GeneralFlash");
        generalFlashObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        muzzleFlashObj.GetComponent<SpriteRenderer>().enabled = false;
        muzzleFlashObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        //Add in the bullet origin's position being modified by the gun's + offset, including direction modifiers

        //Attachment Assignments
        muzzleLoc = currentGun._muzzleLoc;
        opticLoc = currentGun._opticLoc;
        lowerRailLoc = currentGun._lowerRailLoc;
        sideRailLoc = currentGun._sideRailLoc;

        hasMuzzleSlot = currentGun._hasMuzzleSlot;
        hasOpticSlot = currentGun._hasOpticSlot;
        hasLowerRailSlot = currentGun._hasLowerRailSlot;
        hasSideRailSlot = currentGun._hasSideRailSlot;

        GameObject[] allAttachments = GameObject.FindGameObjectsWithTag("Attachment");
        foreach (GameObject g in allAttachments)
        {
            //Here for later convenience
        }

        muzzleHandler = GameObject.Find("Muzzle Handler").GetComponent<AttachmentHandler>();
        opticHandler = GameObject.Find("Optic Handler").GetComponent<AttachmentHandler>();
        LRHandler = GameObject.Find("Lower Rail Handler").GetComponent<AttachmentHandler>();
        SRHandler = GameObject.Find("Side Rail Handler").GetComponent<AttachmentHandler>();
        currentAttachmentHandlers.Add(muzzleHandler); 
        currentAttachmentHandlers.Add(opticHandler); 
        currentAttachmentHandlers.Add(LRHandler); 
        currentAttachmentHandlers.Add(SRHandler); 

        //Backend Assignments
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();

        gun1 = currentGun;
        UpdateGun();

        inventoryHandler = GameObject.Find("Inventory").GetComponent<InventoryHandler>();
    }

//██╗░░░██╗██████╗░██████╗░░█████╗░████████╗███████╗
//██║░░░██║██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝██╔════╝
//██║░░░██║██████╔╝██║░░██║███████║░░░██║░░░█████╗░░
//██║░░░██║██╔═══╝░██║░░██║██╔══██║░░░██║░░░██╔══╝░░
//╚██████╔╝██║░░░░░██████╔╝██║░░██║░░░██║░░░███████╗
//░╚═════╝░╚═╝░░░░░╚═════╝░╚═╝░░╚═╝░░░╚═╝░░░╚══════╝

    void Update()
    {

        //Flipping sprite based on X
        Flip();

        SwitchGun();

        if (Input.GetButton("Fire1") && canShoot && inventoryIsOpen == false)
        {
            currentGun.Shoot();
            if (hasMuzzleSlot && muzzleHandler.attachmentIdentity != null){muzzleHandler.attachmentIdentity.OnShoot();}
            if (hasOpticSlot && opticHandler.attachmentIdentity != null){opticHandler.attachmentIdentity.OnShoot();}
            if (hasLowerRailSlot && LRHandler.attachmentIdentity != null){LRHandler.attachmentIdentity.OnShoot();}
            if (hasSideRailSlot && SRHandler.attachmentIdentity != null){SRHandler.attachmentIdentity.OnShoot();}
        }

        if (Input.GetKey(KeyCode.Tab) && canOpenInventory && !inventoryIsOpen)
        {
            OpenInventory();
        }
    }

    void FixedUpdate()
    {
        //Looking towards mouse
        LookAtCursor();

        Vector2 playerPos = new Vector2(playerScript.position.x, playerScript.position.y + 0.3f);

        //Code to make the gun rotate on a set track around the player
        Vector2 playerToCursor = cursorPos - playerPos;
        Vector2 dir = playerToCursor.normalized;
        Vector2 cursorVector = dir * radius;

        //Constant Updating Assignments and Functions

        transform.position = playerPos + cursorVector;
    }


//██╗███╗░░██╗██╗░░░██╗███████╗███╗░░██╗████████╗░█████╗░██████╗░██╗░░░██╗
//██║████╗░██║██║░░░██║██╔════╝████╗░██║╚══██╔══╝██╔══██╗██╔══██╗╚██╗░██╔╝
//██║██╔██╗██║╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░██║░░██║██████╔╝░╚████╔╝░
//██║██║╚████║░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░██║░░██║██╔══██╗░░╚██╔╝░░
//██║██║░╚███║░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░╚█████╔╝██║░░██║░░░██║░░░
//╚═╝╚═╝░░╚══╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝░░░╚═╝░░░

    public void UpdateGun()
    {
        Debug.Log("Updating Gun");

        //Gun Assignments
        gunName = currentGun._gunName;
        gunType = currentGun._gunType; 
        currentBullet = currentGun._bullet;
        gunDesc = currentGun._gunDesc; 

        //Base Stat Assignments
        damage = currentGun._damage; 
        fireRate = currentGun._fireRate; 
        inaccuracy = currentGun._inaccuracy; 
        armorPen = currentGun._armorPen; 
        bulletSpeed = currentGun._bulletSpeed;

        //Graphics/Prefabs Assignments
        bulletPrefab = currentGun._bulletPrefab;
        gunSprite = currentGun._gunSprite;
        bulletOriginOffset = currentGun._bulletOriginOffset;
        radius = currentGun._radius;
        bulletsAtOnce = currentGun._bulletsAtOnce;
        bulletOriginObj = GameObject.Find("BulletOrigin");
        bulletOrigin = bulletOriginObj.transform;
        gunPos = this.gameObject.transform;

        //Attachment Assignments
        muzzleLoc = currentGun._muzzleLoc;
        opticLoc = currentGun._opticLoc;
        lowerRailLoc = currentGun._lowerRailLoc;
        sideRailLoc = currentGun._sideRailLoc;

        hasMuzzleSlot = currentGun._hasMuzzleSlot;
        hasOpticSlot = currentGun._hasOpticSlot;
        hasLowerRailSlot = currentGun._hasLowerRailSlot;
        hasSideRailSlot = currentGun._hasSideRailSlot;

        //Backend Assignments
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = gunSprite;

        //Bullet Origin Adjustment
        Quaternion quat = gunPos.rotation;
        gunPos.rotation = Quaternion.identity;
        transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
        Vector2 desiredGunPos;
        desiredGunPos.x = gunPos.position.x;
        desiredGunPos.y = gunPos.position.y;
        Vector2 BODesPos = desiredGunPos + bulletOriginOffset;
        float desiredY = bulletOriginOffset.y;
        if (desiredY != bulletOriginOffset.y)
        {
            Vector2 absBOPos;
            absBOPos.x = BODesPos.x;
            absBOPos.y = desiredY;
            bulletOrigin.position = absBOPos;
            //Debug.Log("Adjusted Y pos");
        }
        else
        {
            bulletOrigin.position = BODesPos;
        }
        gunPos.rotation = quat;

        //Cam Specific Adjustments
        if (cam.orthographicSize > normalZoom)
        {
            ZoomCamIn(normalZoom, 0.02f);
        }
        else if (cam.orthographicSize < normalZoom)
        {
            ZoomCamOut(normalZoom, 0.02f);
        }

        if (gunType == Gun.GunType.DMR)
        {
            Debug.Log("Zoomin out");
            ZoomCamOut((normalZoom * 1.5f), 0.02f);
        }

        if (currentGun == gun1)
        {
            currentAttachments = gun1Attachments;
        }
        else if (currentGun == gun2)
        {
            currentAttachments = gun2Attachments;
        }
        else if (currentGun == gun3)
        {
            currentAttachments = gun3Attachments;
        }
        else if (currentGun == gun4)
        {
            currentAttachments = gun4Attachments;
        }
        else if (currentGun == gun5)
        {
            currentAttachments = gun5Attachments;
        }
        else if (currentGun == gun6)
        {
            currentAttachments = gun6Attachments;
        }

        UpdateAttachments(null);
    }

    //Gun Dropping/Picking Up
    public void PlayerPickUpGun(Gun newGun, DroppedGun dG)
    {
        canPickUpGun = false;
        StartCoroutine(PickupCooldown(0));
        if (NumGunsHeld() == 6)
        {
            PlayerDropGun(currentGun);
        }
        else
        {
            SetLowestIntGun(newGun);
        }
        currentGun = newGun;
        UpdateGun();
        Destroy(dG.thisObj);
    }    

    public void PlayerDropGun(Gun gunToDrop)
    {
        GameObject gunDroppedObj = Instantiate(droppedGun, gunPos.position, Quaternion.identity);
        Rigidbody2D gunRB = gunDroppedObj.GetComponent<Rigidbody2D>();
        Vector2 gunDropDir;
        gunDropDir.x = Mathf.Clamp(cursorPos.x, -3, 3);
        gunDropDir.y = Mathf.Clamp(cursorPos.y, -3, 3);
        gunRB.AddForce(gunDropDir * 2, ForceMode2D.Impulse);
        DroppedGun dG = gunDroppedObj.GetComponent<DroppedGun>();
        dG.GenerateIdentity(gunToDrop);
    }

    public int NumGunsHeld()
    {
        int numGuns = 0;
        if (gun1 != null)
        {
            numGuns++;
        }
        if (gun2 != null)
        {
            numGuns++;
        }
        if (gun3 != null)
        {
            numGuns++;
        }
        if (gun4 != null)
        {
            numGuns++;
        }
        if (gun5 != null)
        {
            numGuns++;
        }
        if (gun6 != null)
        {
            numGuns++;
        }
        return numGuns;
    }

    public void SetLowestIntGun(Gun newGun)
    {
        if (gun1 == null)
        {
            gun1 = newGun;
            return;
        }
        if (gun2 == null)
        {
            gun2 = newGun;
            return;
        }
        if (gun3 == null)
        {
            gun3 = newGun;
            return;
        }
        if (gun4 == null)
        {
            gun4 = newGun;
            return;
        }
        if (gun5 == null)
        {
            gun5 = newGun;
            return;
        }
        if (gun6 == null)
        {
            gun6 = newGun;
            return;
        }
    }

    public void SwitchGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && gun1 != null)
        {
            StoreAttachments();
            currentGun = gun1;
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && gun2 != null)
        {
            currentGun = gun2;
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && gun3 != null)
        {
            currentGun = gun3;
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && gun4 != null)
        {
            currentGun = gun4;
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && gun5 != null)
        {
            currentGun = gun5;
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && gun6 != null)
        {
            currentGun = gun6;
            UpdateGun();
        }
    }

    public void StoreAttachments()
    {
        if (currentGun == gun1)
        {
            gun1Attachments = currentAttachments;
        }
        else if (currentGun == gun2)
        {
            gun2Attachments = currentAttachments;
        }
        else if (currentGun == gun3)
        {
            gun2Attachments = currentAttachments;
        }
        else if (currentGun == gun4)
        {
            gun2Attachments = currentAttachments;
        }
        else if (currentGun == gun5)
        {
            gun2Attachments = currentAttachments;
        }
        else if (currentGun == gun6)
        {
            gun2Attachments = currentAttachments;
        }
    }

    //Attachment Dropping/Picking Up
    public void PlayerPickUpAttachment(Attachment newAttachment, DroppedAttachment dA)
    {
        canPickUpAttachment = false;
        StartCoroutine(PickupCooldown(1));
        UpdateAttachments(newAttachment);
        newAttachment.PickUp();


        Destroy(dA.thisObj);
    }    

    public void PlayerDropAttachment(Attachment attachmentToDrop)
    {
        GameObject attachmentDroppedObj = Instantiate(droppedAttachment, gunPos.position, Quaternion.identity);
        Rigidbody2D atchRB = attachmentDroppedObj.GetComponent<Rigidbody2D>();
        Vector2 gunDropDir;
        gunDropDir.x = Mathf.Clamp(cursorPos.x, -3, 3);
        gunDropDir.y = Mathf.Clamp(cursorPos.y, -3, 3);
        atchRB.AddForce(gunDropDir * 2, ForceMode2D.Impulse);
        DroppedAttachment dA = attachmentDroppedObj.GetComponent<DroppedAttachment>();
        dA.GenerateIdentity(attachmentToDrop);
        switch(attachmentToDrop._attachmentType)
        {
            case Attachment.AttachmentType.muzzle:
            muzzleHandler.attachmentIdentity = null;
            break;
            case Attachment.AttachmentType.optic:
            opticHandler.attachmentIdentity = null;
            break;
            case Attachment.AttachmentType.lowerRail:
            LRHandler.attachmentIdentity = null;
            break;
            case Attachment.AttachmentType.sideRail:
            SRHandler.attachmentIdentity = null;
            break;
        }
        //UpdateGun();
    }

    IEnumerator PickupCooldown(int PickupIdentity) //1 is attachment, 0 is gun
    {
        yield return new WaitForSeconds(2f);
        switch(PickupIdentity)
        {
            case 0:
            canPickUpGun = true;
            break;
            case 1:
            canPickUpAttachment = true;
            break;
        }
    }



    
//██╗███╗░░██╗██╗░░░██╗███████╗███╗░░██╗████████╗░█████╗░██████╗░██╗░░░██╗  ██╗░░░██╗██╗
//██║████╗░██║██║░░░██║██╔════╝████╗░██║╚══██╔══╝██╔══██╗██╔══██╗╚██╗░██╔╝  ██║░░░██║██║
//██║██╔██╗██║╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░██║░░██║██████╔╝░╚████╔╝░  ██║░░░██║██║
//██║██║╚████║░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░██║░░██║██╔══██╗░░╚██╔╝░░  ██║░░░██║██║
//██║██║░╚███║░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░╚█████╔╝██║░░██║░░░██║░░░  ╚██████╔╝██║
//╚═╝╚═╝░░╚══╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝░░░╚═╝░░░  ░╚═════╝░╚═╝
    
    [Header("Inventory UI Variables")]

    public GameObject inventoryUI;
    public bool canOpenInventory = true;

    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI gunDescText;

    public Image gunImage;

    bool inventoryIsOpen = false;
    bool InventoryIsOpen
    {
    get { return inventoryIsOpen; }
    set {
        inventoryIsOpen = value;
        }
    }

    void OpenInventory()
    {
        inventoryUI.SetActive(true);
        inventoryIsOpen = true;

        InvokeRepeating("CheckForCloseInventory", 0f, 0.01f);

        string lowerGunName = "";
        for (var i = 0; i < gunName.Length; i++)
        {
            if (char.IsLetter(gunName[i]))
            {
                lowerGunName += char.ToLower(gunName[i]);
            }
            else
            {
                lowerGunName += gunName[i];
            }
        }


        gunNameText.text = lowerGunName;

        string literallyJustAPeriod = ".";
        char[] aMerePeriod = literallyJustAPeriod.ToCharArray();

        string updatedDesc = "";
        for (var j = 0; j < gunDesc.Length; j++)
        {
            if (gunDesc[j] == aMerePeriod[0])
            {
                updatedDesc += ".\n\n";
            }
            else
            {
                updatedDesc += gunDesc[j];
            }
        }

        string finalDesc = "";
        finalDesc = "Damage: " + damage + "\nFire Rate: " + (fireRate * 60) + "RPM" + "\nAccuracy: " + (100 - inaccuracy) + "\nArmor Penetration: " + armorPen + "\n\n////////////////////////\n\n" + updatedDesc;


        gunDescText.text = finalDesc;

        gunImage.sprite = gunSprite;

        if (inventoryHandler == null)
        {
            inventoryHandler = GameObject.Find("Inventory").GetComponent<InventoryHandler>();
        }

        inventoryHandler.OnInventoryOpen();

    }

    void CloseInventory()
    {
        inventoryUI.SetActive(false);
        inventoryIsOpen = false;
        inventoryHandler.OnInventoryClose();
        CancelInvoke();
        UpdateGun();
    }

    void CheckForCloseInventory()
    {
        if (!Input.GetKey(KeyCode.Tab))
        {
            CloseInventory();
        }
    }

//░█████╗░░█████╗░░██████╗███╗░░░███╗███████╗████████╗██╗░█████╗░░██████╗
//██╔══██╗██╔══██╗██╔════╝████╗░████║██╔════╝╚══██╔══╝██║██╔══██╗██╔════╝
//██║░░╚═╝██║░░██║╚█████╗░██╔████╔██║█████╗░░░░░██║░░░██║██║░░╚═╝╚█████╗░
//██║░░██╗██║░░██║░╚═══██╗██║╚██╔╝██║██╔══╝░░░░░██║░░░██║██║░░██╗░╚═══██╗
//╚█████╔╝╚█████╔╝██████╔╝██║░╚═╝░██║███████╗░░░██║░░░██║╚█████╔╝██████╔╝
//░╚════╝░░╚════╝░╚═════╝░╚═╝░░░░░╚═╝╚══════╝░░░╚═╝░░░╚═╝░╚════╝░╚═════╝░

    void LookAtCursor()
    {
        cursorPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = cursorPos - rb2d.position;
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg /*- 90f*/;
        rb2d.rotation = angle;
    }

    void GunPositioning()
    {
        //Code to make the gun rotate on a set track around the player
        Vector2 playerToCursor = cursorPos - playerPos;
        Vector2 dir = playerToCursor.normalized;
        Vector2 cursorVector = dir * radius;

        if (playerToCursor.magnitude < cursorVector.magnitude) // detect if mouse is in inner radius
            {cursorVector = playerToCursor;}

        transform.position = playerPos + cursorVector;
    }

    void Flip()
    {
        if (gunPos.position.x > playerScript.position.x)
        {
            transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
            gunDir = 1;
            playerScript.FlipLeft();
        }
        else
        {
            transform.localScale = new Vector3(scaleX, (-1) * scaleY, transform.localScale.z);
            gunDir = -1;
            playerScript.FlipRight();
        }
    }

    public IEnumerator MuzzleFlashCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        muzzleFlashObj.GetComponent<SpriteRenderer>().enabled = false;
        muzzleFlashObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        generalFlashObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
    }

//    
//░██████╗░░█████╗░███╗░░░███╗███████╗██████╗░██╗░░░░░░█████╗░██╗░░░██╗
//██╔════╝░██╔══██╗████╗░████║██╔════╝██╔══██╗██║░░░░░██╔══██╗╚██╗░██╔╝
//██║░░██╗░███████║██╔████╔██║█████╗░░██████╔╝██║░░░░░███████║░╚████╔╝░
//██║░░╚██╗██╔══██║██║╚██╔╝██║██╔══╝░░██╔═══╝░██║░░░░░██╔══██║░░╚██╔╝░░
//╚██████╔╝██║░░██║██║░╚═╝░██║███████╗██║░░░░░███████╗██║░░██║░░░██║░░░
//░╚═════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚══════╝╚═╝░░░░░╚══════╝╚═╝░░╚═╝░░░╚═╝░░░
    
    /*public virtual void Shoot()
    {
        //Generates inaccuracy dynamically
        //Creates a bullet, gets it's rigidbody
        for (int b = 0; b<bulletsAtOnce; b++)
        {
            RandomAngle();
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            //Shoots the bullet
            bulletRB.AddForce(bulletOrigin.right * bulletSpeed, ForceMode2D.Impulse);
        }
        StartCoroutine(FireRateCheck());
    }*/

    public IEnumerator FireRateCheck()
    {
        canShoot = false;
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
    }

//░█████╗░░█████╗░███╗░░██╗██╗░░░██╗███████╗███╗░░██╗██╗███████╗███╗░░██╗░█████╗░███████╗
//██╔══██╗██╔══██╗████╗░██║██║░░░██║██╔════╝████╗░██║██║██╔════╝████╗░██║██╔══██╗██╔════╝
//██║░░╚═╝██║░░██║██╔██╗██║╚██╗░██╔╝█████╗░░██╔██╗██║██║█████╗░░██╔██╗██║██║░░╚═╝█████╗░░
//██║░░██╗██║░░██║██║╚████║░╚████╔╝░██╔══╝░░██║╚████║██║██╔══╝░░██║╚████║██║░░██╗██╔══╝░░
//╚█████╔╝╚█████╔╝██║░╚███║░░╚██╔╝░░███████╗██║░╚███║██║███████╗██║░╚███║╚█████╔╝███████╗
//░╚════╝░░╚════╝░╚═╝░░╚══╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝╚═╝╚══════╝╚═╝░░╚══╝░╚════╝░╚══════╝


    //Generates angle variation on bullet origin to simulate gun inaccuracy (stops everything from being a laser beam)
    public void RandomAngle()
    {
        Vector3 vector;

        vector.x = transform.localRotation.x;
        vector.y = transform.localRotation.y;
        vector.z = transform.localRotation.z;
        float minRot = transform.rotation.z + (inaccuracy * -1f);
        float maxRot = transform.rotation.z + inaccuracy;
        float randomNum = Random.Range(minRot, maxRot);
        
        if (gunDir > 0f)
        {
            vector.z = transform.rotation.z + randomNum;
        }
        else
        {
            vector.z = transform.rotation.z - randomNum;
        }
        
        Quaternion quaternion = Quaternion.Euler(vector);

        bulletOrigin.transform.localRotation = quaternion;
    }

    //Make the zooms apply only once per frame

    public void ZoomCamOut(float desCamSize, float zoomSpeed)
    {
        while (cam.orthographicSize < desCamSize)
        {
            float newSize = Mathf.MoveTowards(cam.orthographicSize, desCamSize, zoomSpeed * Time.deltaTime);
            cam.orthographicSize = newSize;
        }
    }

    public void ZoomCamIn(float desCamSize, float zoomSpeed)
    {
        while (cam.orthographicSize > desCamSize)
        {
            float newSize = Mathf.MoveTowards(cam.orthographicSize, desCamSize, zoomSpeed * Time.deltaTime);
            cam.orthographicSize = newSize;
        }
    }


    public IEnumerator WaitTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

//    
//░█████╗░████████╗████████╗░█████╗░░█████╗░██╗░░██╗███╗░░░███╗███████╗███╗░░██╗████████╗░██████╗
//██╔══██╗╚══██╔══╝╚══██╔══╝██╔══██╗██╔══██╗██║░░██║████╗░████║██╔════╝████╗░██║╚══██╔══╝██╔════╝
//███████║░░░██║░░░░░░██║░░░███████║██║░░╚═╝███████║██╔████╔██║█████╗░░██╔██╗██║░░░██║░░░╚█████╗░
//██╔══██║░░░██║░░░░░░██║░░░██╔══██║██║░░██╗██╔══██║██║╚██╔╝██║██╔══╝░░██║╚████║░░░██║░░░░╚═══██╗
//██║░░██║░░░██║░░░░░░██║░░░██║░░██║╚█████╔╝██║░░██║██║░╚═╝░██║███████╗██║░╚███║░░░██║░░░██████╔╝
//╚═╝░░╚═╝░░░╚═╝░░░░░░╚═╝░░░╚═╝░░╚═╝░╚════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚══════╝╚═╝░░╚══╝░░░╚═╝░░░╚═════╝░

    public void UpdateAttachments(Attachment newAttachmentToAdd) //call with 'null' if no new attachments
    {
        currentAttachments.Clear();
        foreach (AttachmentHandler a in currentAttachmentHandlers)
        {
            if (a.attachmentIdentity != null)
            {
                currentAttachments.Add(a.attachmentIdentity);
            }
        }
        if (newAttachmentToAdd != null)
        {
            currentAttachments.Add(newAttachmentToAdd);
        }

        foreach (Attachment atch in currentAttachments)
        {
            switch (atch._attachmentType)
            {
                case Attachment.AttachmentType.muzzle:
                muzzleHandler.attachmentIdentity = atch;
                break;

                case Attachment.AttachmentType.optic:
                opticHandler.attachmentIdentity = atch;
                break;

                case Attachment.AttachmentType.lowerRail:
                LRHandler.attachmentIdentity = atch;
                break;
                
                case Attachment.AttachmentType.sideRail:
                SRHandler.attachmentIdentity = atch;
                break;
            }
        }
        

        foreach (AttachmentHandler a in currentAttachmentHandlers)
        {
            //Calculate Location On Gun
            Quaternion quat = gunPos.rotation;
            gunPos.rotation = Quaternion.identity;
            transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);

            //Desired gun position
            Vector2 desiredGunPos;
            desiredGunPos.x = gunPos.position.x;
            desiredGunPos.y = gunPos.position.y;

            //Variables for the following section (assigned to avoid CS0165)
            Vector2 attachmentLoc = Vector2.zero;
            Vector2 atchDesPos = Vector2.zero;
            Vector2 absAtchPos = Vector2.zero;

            if (a.attachmentIdentity == null)
            {
                a.ClearAttachments();
            }
            else
            {
                a.UpdateAttachments();
            }

            //Checking attachment type and the desired attachment location
            switch (a.attachmentType)
            {
                case Attachment.AttachmentType.muzzle:
                if (hasMuzzleSlot)
                {
                attachmentLoc = currentGun._muzzleLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                muzzleHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.muzzle)
                {
                    muzzleHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (muzzleHandler.attachmentIdentity != null)
                {
                    muzzleHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;

                case Attachment.AttachmentType.optic:
                if (hasOpticSlot)
                {
                attachmentLoc = currentGun._opticLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                opticHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.optic)
                {
                    opticHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (opticHandler.attachmentIdentity != null)
                {
                    opticHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;

                case Attachment.AttachmentType.lowerRail:
                if (hasLowerRailSlot)
                {
                attachmentLoc = currentGun._lowerRailLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                LRHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.lowerRail)
                {
                    LRHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (LRHandler.attachmentIdentity != null)
                {
                    LRHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;
                
                case Attachment.AttachmentType.sideRail:
                if (hasSideRailSlot)
                {
                attachmentLoc = currentGun._sideRailLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                SRHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.sideRail)
                {
                    SRHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (SRHandler.attachmentIdentity != null)
                {
                    SRHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;
            }

            gunPos.rotation = quat;
        
            //Clear Empty Attachments & update new ones
            if (a.attachmentIdentity == null)
            {
                a.ClearAttachments();
            }
            else
            {
                a.UpdateAttachments();
            }

            //Apply Modifiers

            //Multiplicative
            if (a.damageMod != 0){damage *= a.damageMod;}
            if (a.fireRateMod != 0){fireRate *= a.fireRateMod;}
            if (a.accuracyMod != 0){inaccuracy *= a.accuracyMod;}
            if (a.armorPenMod != 0){armorPen *= a.armorPenMod;}
            if (a.bulletSpeedMod != 0){bulletSpeed *= a.bulletSpeedMod;}
            if (a.bulletsAtOnceMod != 0){bulletsAtOnce *= a.bulletsAtOnceMod;}

            //Flat
            damage += a.damageModFlat;
            fireRate += a.fireRateModFlat;
            inaccuracy += a.accuracyModFlat;
            armorPen += a.armorPenModFlat;
            bulletSpeed += a.bulletSpeedModFlat;
            bulletsAtOnce += a.bulletsAtOnceModFlat;

            //Floor the stats
            FloorStats();

            //Apply Cosmetics
            if (a.attachmentIdentity != null)
            {
                a.spriteRenderer.sprite = a.attachmentSprite;
                a.UpdateAttachments();
            }
            else
            {
                a.spriteRenderer.sprite = null;
            }
        }
    }



    public bool CheckIfBulletBehaviorChange()
    {
        foreach (AttachmentHandler a in currentAttachmentHandlers)
        {
            if (a.attachmentIdentity != null)
            {
                if (a.altersBulletMovementBehavior == true)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public void FinShootTrigger()
    {
        if (hasMuzzleSlot && muzzleHandler.attachmentIdentity != null){muzzleHandler.attachmentIdentity.OnFinShoot();}
        if (hasOpticSlot && opticHandler.attachmentIdentity != null){opticHandler.attachmentIdentity.OnFinShoot();}
        if (hasLowerRailSlot && LRHandler.attachmentIdentity != null){LRHandler.attachmentIdentity.OnFinShoot();}
        if (hasSideRailSlot && SRHandler.attachmentIdentity != null){SRHandler.attachmentIdentity.OnFinShoot();}
    }


    
//███╗░░░███╗██╗░██████╗░█████╗░███████╗██╗░░░░░██╗░░░░░░█████╗░███╗░░██╗███████╗░█████╗░██╗░░░██╗░██████╗
//████╗░████║██║██╔════╝██╔══██╗██╔════╝██║░░░░░██║░░░░░██╔══██╗████╗░██║██╔════╝██╔══██╗██║░░░██║██╔════╝
//██╔████╔██║██║╚█████╗░██║░░╚═╝█████╗░░██║░░░░░██║░░░░░███████║██╔██╗██║█████╗░░██║░░██║██║░░░██║╚█████╗░
//██║╚██╔╝██║██║░╚═══██╗██║░░██╗██╔══╝░░██║░░░░░██║░░░░░██╔══██║██║╚████║██╔══╝░░██║░░██║██║░░░██║░╚═══██╗
//██║░╚═╝░██║██║██████╔╝╚█████╔╝███████╗███████╗███████╗██║░░██║██║░╚███║███████╗╚█████╔╝╚██████╔╝██████╔╝
//╚═╝░░░░░╚═╝╚═╝╚═════╝░░╚════╝░╚══════╝╚══════╝╚══════╝╚═╝░░╚═╝╚═╝░░╚══╝╚══════╝░╚════╝░░╚═════╝░╚═════╝░

    public void BulletFire()
    {
        Debug.Log("Pew");
        for (int b = 0; b<bulletsAtOnce; b++)
        {
            RandomAngle();
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            //Shoots the bullet
            bulletRB.AddForce(bulletOrigin.right * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    public IEnumerator Burst(int howManyBullets)
    {
        for (int i = 0; i < howManyBullets; i++)
        {
            BulletFire();
            yield return new WaitForSeconds(currentGun.btwnBulletInBurstTime);
        }
    }

    void FloorStats()
    {
        if (damage < 1)
        {
            damage = 1;
        }
        if (fireRate <= 0)
        {
            fireRate = 0.001f;
        }
        if (inaccuracy <= 0)
        {
            inaccuracy = 0;
        }
        if (armorPen <= 0)
        {
            armorPen = 1;
        }
        if (bulletSpeed <= 0)
        {
            bulletSpeed = 0;
        }
        if (bulletsAtOnce <= 0)
        {
            bulletsAtOnce = 0;
        }
    }

    public void SetMRB(GameObject bullet)
    {
        mostRecentBullet.Add(bullet);
    }

    public void ClearMRB()
    {
        mostRecentBullet.Clear();
    }

    public void OnEnemyKill(GameObject enemy)
    {
        foreach (AttachmentHandler atchHandler in currentAttachmentHandlers)
        {
            if (atchHandler.attachmentIdentity != null)
                atchHandler.attachmentIdentity.OnEnemyKill(enemy);
        }
    }
    
}