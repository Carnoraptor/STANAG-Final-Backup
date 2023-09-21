using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/Create New Attachment", order = 2)]
public class Attachment : ScriptableObject
{
    [Header("Main")]
    public string _attachmentName;
    public enum AttachmentType
    {
        muzzle,
        optic,
        lowerRail,
        sideRail
    }
    public AttachmentType _attachmentType;
    public string _attachmentEffectDesc;
    public string _attachmentDescription;
    public int _attachmentID;

    [Header("Percentage Modifiers")]
    public float _damageMod;
    public float _fireRateMod;
    public float _accuracyMod;
    public float _armorPenMod;
    public float _bulletSpeedMod;
    public int _bulletsAtOnceMod;

    [Header("Flat Modifiers")]
    public int _damageModFlat;
    public float _fireRateModFlat;
    public float _accuracyModFlat;
    public int _armorPenModFlat;
    public float _bulletSpeedModFlat;
    public int _bulletsAtOnceModFlat;
    
    [Header("Graphics and Prefabs")]
    public bool _editsBullet;
    public GameObject _newBulletPrefab;
    public Sprite _attachmentSprite;
    public bool _isAnimated;
    public AnimationClip _anim;
    //public Anim gunShooting

    [Header("Misc")]
    public bool altersBulletMovementBehavior;

    [HideInInspector]
    public static GunHandler gunHandler;


    [RuntimeInitializeOnLoadMethod]
    static void GetGunHandler()
    {
        if(SceneManager.GetActiveScene().name == "Floor1" || SceneManager.GetActiveScene().name == "TestingScene" )
        gunHandler = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunHandler>();
    }

    public virtual void PickUp()
    {
        
    }

    public virtual void OnShoot()
    {

    }

    public virtual void OnFinShoot()
    {
        
    }

    public virtual void OnEnemyKill(GameObject enemy)
    {

    }
}
