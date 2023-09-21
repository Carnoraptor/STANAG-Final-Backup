using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FirstGearGames.SmoothCameraShaker;
using FMODUnity;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/Create New Gun", order = 1)]
public class Gun : ScriptableObject
{
    [Header("Main")]
    public string _gunName;
    public enum GunType
    {
        Pistol,
        AssaultRifle,
        MachineGun,
        SubmachineGun,
        Shotgun,
        DMR,
        Explosive,
        Unique
    }
    public GunType _gunType;
    public int gunID;
    public string _gunDesc;

    [Header("Stats")]
    public int _damage;
    public float _fireRate;
    public float _inaccuracy;
    public int _armorPen;
    public float _bulletSpeed;
    public float _radius;
    public int _bulletsAtOnce;
    public Bullet _bullet;
    public enum FireMode
    {
        SemiAutomatic,
        Automatic,
        Burst,
        Unique
    }
    public FireMode _fireMode;

    [Header("Graphics and Prefabs")]
    public GameObject _bulletPrefab;
    public Sprite _gunSprite;
    public Vector2 _bulletOriginOffset;

    public EventReference _shootSFX;

    public GameObject _gunSmoke;
    //public Anim gunShooting

    [Header("Attachments")]
    public bool _hasMuzzleSlot;
    public bool _hasOpticSlot;
    public bool _hasLowerRailSlot;
    public bool _hasSideRailSlot;
    //public bool _hasStockSlot;

    public Vector2 _muzzleLoc;
    public Vector2 _opticLoc;
    public Vector2 _lowerRailLoc;
    public Vector2 _sideRailLoc;
    //public Vector2 _stockLoc;

    public static GunHandler gunHandler;

    //Functions
    [RuntimeInitializeOnLoadMethod]
    static void GetGunHandler()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LevelDesigner" && SceneManager.GetActiveScene().name != "RoomDesignScene")
        gunHandler = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunHandler>();
    }

    public virtual void Shoot()
    {
        //Generates inaccuracy dynamically
        //Creates a bullet, gets it's rigidbody
        //GameObject.FindWithTag("MainCamera").GetComponent<CameraController>().CameraShake(_shakeData);
        //FIX SCREENSHAKE
        AudioManager.instance.PlayOneShot(_shootSFX, gunHandler.gameObject.transform.position);

        gunHandler.ClearMRB();

        for (int b = 0; b<gunHandler.bulletsAtOnce; b++)
        {
            gunHandler.RandomAngle();
            GameObject bullet = Instantiate(gunHandler.bulletPrefab, gunHandler.bulletOrigin.position, gunHandler.bulletOrigin.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bullet.GetComponent<BulletHandler>().thisBullet = _bullet;

            //Shoots the bullet
            gunHandler.SetMRB(bullet);
            if (gunHandler.CheckIfBulletBehaviorChange() == false)
            {
                bulletRB.AddForce(gunHandler.bulletOrigin.right * gunHandler.bulletSpeed, ForceMode2D.Impulse);
            }
        }
        gunHandler.FinShootTrigger();
        gunHandler.StartCoroutine(gunHandler.FireRateCheck());

        if (gunHandler.gunDir == 1)
        {
            gunHandler.muzzleFlashObj.transform.position = new Vector2(gunHandler.bulletOrigin.position.x + Random.Range(-0.07f, 0f), gunHandler.bulletOrigin.position.y + Random.Range(-0.02f, 0.02f));
        }
        else
        {
            gunHandler.muzzleFlashObj.transform.position = new Vector2(gunHandler.bulletOrigin.position.x + Random.Range(0.07f, 0f), gunHandler.bulletOrigin.position.y + Random.Range(-0.02f, 0.02f));
        }

        //gunHandler.muzzleFlashObj.GetComponent<SpriteRenderer>().enabled = true;
        gunHandler.muzzleFlashObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;
        gunHandler.generalFlashObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;
        gunHandler.muzzleFlashObj.GetComponent<SpriteRenderer>().sprite = gunHandler.muzzleFlashes[Random.Range(0, gunHandler.muzzleFlashes.Length)];
        
        if (_gunSmoke != null)
        {GameObject gunSmokeSpawned = Instantiate(_gunSmoke, gunHandler.muzzleFlashObj.transform.position, Quaternion.identity);
        gunSmokeSpawned.GetComponent<ParticleSystem>().Play();}
        gunHandler.StartCoroutine(gunHandler.MuzzleFlashCooldown());

        //MeshParticleSystem meshPS = FindObjectOfType<MeshParticleSystem>();
        //meshPS.SpawnCasing();
    }

    [Header("Misc Which Are Usually Useless")]
    public float btwnBulletInBurstTime;
}
