using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyGeneric;
using BulletMLLib;
using Pixelnest.BulletML;

public class TalosBehavior : MonoBehaviour
{

    [Header("Specifics")]
    public BossAI ai;

    bool readyToAttack = false;
    bool onCooldown = false;
    BossGeneralAI bossAI;

    [SerializeField] Animator animator;

    [Header("Actual Gameplay Variables")]

    bool doMovement = true;
    public bool enraged = false;

    [SerializeField] GameObject sparkEffect;

    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;

    BulletSourceScript leftHandSource;
    BulletSourceScript rightHandSource;

    SpriteRenderer leftHandRenderer;
    SpriteRenderer rightHandRenderer;

    public TextAsset attack1;
    public TextAsset attack2;
    public TextAsset attack3;
    public TextAsset attack4;
    public TextAsset attack5;

    public int currentAttack = 0; // 0 means not attacking

    [Header("SFX")]
    public EventReference bulletSound;
    public EventReference windUpSound;
    public EventReference slamSound;

    public bool enemyBulletSoundPlaying = false;

    private BulletManagerScript bulletManager;

    bool attackCooldown = false;

    public float topHandY;
    public float bottomHandY;

    [Header("Sprites")]
    [SerializeField] Sprite leftHandLiftSprite;
    [SerializeField] Sprite leftHandSlamSprite;
    [SerializeField] Sprite rightHandLiftSprite;
    [SerializeField] Sprite rightHandSlamSprite;




    // Start is called before the first frame update
    void Start()
    {
        bossAI = GetComponent<BossGeneralAI>();

        leftHandSource = leftHand.GetComponent<BulletSourceScript>();
        rightHandSource = rightHand.GetComponent<BulletSourceScript>();

        leftHandRenderer = leftHand.GetComponent<SpriteRenderer>();
        rightHandRenderer = rightHand.GetComponent<SpriteRenderer>();

        StartCoroutine(BossStart());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doMovement)
        {
            FollowPlayer();
        }
    }



    IEnumerator BeginCooldown()
    {
        float timeToWait = 8f;
        switch(currentAttack)
        {
            case 1:
            //slampede
            timeToWait = 18f;
            break;
            //giga
            case 2:
            timeToWait = 20f;
            break;
            //seismic
            case 3:
            timeToWait = 22f;
            break;
            case 4:
            //Whirlwind Bullets
            timeToWait = 15f;
            break;
            case 5:
            break;
        }
        yield return new WaitForSeconds(timeToWait);
        currentAttack = 0;
        yield return new WaitForSeconds(3f);
        onCooldown = false;
        DecideAttack();
    }

    //ATTACKING
    int mostRecentAttack = 0;

    void DecideAttack()
    {
        int decideAttack = Random.Range(1, 3);
        if (decideAttack == mostRecentAttack)
        {
            decideAttack = Random.Range(1, 3);
        }
        currentAttack = decideAttack;
        mostRecentAttack = decideAttack;
        switch(decideAttack)
        {
            //Slam Stampede
            case 1:
                StartCoroutine(SlamStampede());
                break;
            case 2:
                StartCoroutine(GigaSlam());
                break;
            case 3:
                StartCoroutine(SeismicWave());
                break;
            case 4:
                //rightHandSource.xmlFile = attack4;
                //rightHandSource.ParsePattern(false);
                //rightHandSource.Initialize();
                break;
            case 5:
                /*leftHandSource.xmlFile = attack1;
                leftHandSource.ParsePattern(false);
                leftHandSource.Initialize();
                rightHandSource.xmlFile = attack3;
                rightHandSource.ParsePattern(false);
                rightHandSource.Initialize();*/
                break;
        }
        onCooldown = true;
        readyToAttack = false;
        StartCoroutine(BeginCooldown());
    }



        //ATTACKS

    IEnumerator SlamStampede()
    {
        doMovement = false;
        StartCoroutine(StampedeLeft());
        StartCoroutine(StampedeRight());
        yield return new WaitForSeconds(0.6f);
        leftHandSource.xmlFile = attack1;
        yield return new WaitForSeconds(0.6f);
        rightHandSource.xmlFile = attack1;
        yield return new WaitForSeconds(12f);
        doMovement = true;
    }


    IEnumerator StampedeLeft()
    {
    for (var k = 0; k < 20; k++)
    {
        leftHandRenderer.sprite = leftHandLiftSprite;
        for (var i = 0; i < 30; i++)
        {
            LiftLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        leftHandRenderer.sprite = leftHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, leftHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        leftHandSource.ParsePattern(false);
        leftHandSource.Initialize();
        yield return new WaitForSeconds(0.2f);
    }
    }


    IEnumerator StampedeRight()
    {
        yield return new WaitForSeconds(0.4f);

        for (var k = 0; k < 20; k++)
    {
        rightHandRenderer.sprite = rightHandLiftSprite;
        for (var i = 0; i < 30; i++)
        {
            LiftRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        rightHandRenderer.sprite = rightHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, rightHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        rightHandSource.ParsePattern(false);
        rightHandSource.Initialize();
        yield return new WaitForSeconds(0.2f);
    }
    }

    //ATTACK 2: GIGA SLAM
    IEnumerator GigaSlam()
    {
        doMovement = false;
        yield return new WaitForSeconds(0.6f);
        leftHandRenderer.sprite = leftHandLiftSprite;
        for (var i = 0; i < 100; i++)
        {
            LiftLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        leftHandRenderer.sprite = leftHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, leftHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        leftHandSource.xmlFile = attack2;
        yield return new WaitForSeconds(0.6f);
        rightHandRenderer.sprite = rightHandLiftSprite;
        for (var i = 0; i < 100; i++)
        {
            LiftRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        rightHandRenderer.sprite = rightHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, rightHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        rightHandSource.xmlFile = attack2;
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(GigaLeft());
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(GigaRight());
        yield return new WaitForSeconds(12f);
        doMovement = true;
    }

    IEnumerator GigaLeft()
    {
    for (var k = 0; k < 10; k++)
    {
        leftHandRenderer.sprite = leftHandLiftSprite;
        for (var i = 0; i < 50; i++)
        {
            LiftLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        leftHandRenderer.sprite = leftHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, leftHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        leftHandSource.ParsePattern(false);
        leftHandSource.Initialize();
        yield return new WaitForSeconds(0.2f);
    }
    }


    IEnumerator GigaRight()
    {
        for (var k = 0; k < 10; k++)
    {
        rightHandRenderer.sprite = rightHandLiftSprite;
        for (var i = 0; i < 50; i++)
        {
            LiftRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        rightHandRenderer.sprite = rightHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, rightHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        rightHandSource.ParsePattern(false);
        rightHandSource.Initialize();
        yield return new WaitForSeconds(0.2f);
    }
    }


    //ATTACK 3: SEISMIC WAVE

    IEnumerator SeismicWave()
    {

        doMovement = false;
        leftHandRenderer.sprite = leftHandLiftSprite;
        for (var i = 0; i < 100; i++)
        {
            LiftLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        leftHandRenderer.sprite = leftHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, leftHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        Instantiate(sparkEffect, new Vector2(leftHand.transform.position.x, 10.1f), Quaternion.identity);
        leftHandSource.xmlFile = attack3;
        doMovement = true;
        yield return new WaitForSeconds(1f);
        doMovement = false;
        rightHandRenderer.sprite = rightHandLiftSprite;
        for (var i = 0; i < 100; i++)
        {
            LiftRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        rightHandRenderer.sprite = rightHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, rightHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        Instantiate(sparkEffect, new Vector2(rightHand.transform.position.x, 10.1f), Quaternion.identity);
        rightHandSource.xmlFile = attack3;
        doMovement = true;
        yield return new WaitForSeconds(1f);

    for (var i = 0; i < 5; i++)
    {
        doMovement = false;
        StartCoroutine(SeismicLeft());
        yield return new WaitForSeconds(2f);
        doMovement = true;
        yield return new WaitForSeconds(1f);
        doMovement = false;
        StartCoroutine(SeismicRight());
        yield return new WaitForSeconds(2f);
        doMovement = true;
        yield return new WaitForSeconds(1f);
    }
    }


    IEnumerator SeismicLeft()
    {
        leftHandRenderer.sprite = leftHandLiftSprite;
        for (var i = 0; i < 100; i++)
        {
            LiftLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        leftHandRenderer.sprite = leftHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, leftHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamLeftHand();
            yield return new WaitForSeconds(0.01f);
        }
        Instantiate(sparkEffect, new Vector2(leftHand.transform.position.x, 10.1f), Quaternion.identity);
        leftHandSource.ParsePattern(false);
        leftHandSource.Initialize();
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator SeismicRight()
    {
        rightHandRenderer.sprite = rightHandLiftSprite;
        for (var i = 0; i < 100; i++)
        {
            LiftRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        rightHandRenderer.sprite = rightHandSlamSprite;
        AudioManager.instance.PlayOneShot(slamSound, rightHand.transform.position);
        for (var j = 0; j < 10; j++)
        {
            SlamRightHand();
            yield return new WaitForSeconds(0.01f);
        }
        Instantiate(sparkEffect, new Vector2(rightHand.transform.position.x, 10.1f), Quaternion.identity);
        rightHandSource.ParsePattern(false);
        rightHandSource.Initialize();
        yield return new WaitForSeconds(0.2f);
    }






    void LiftLeftHand()
    {
        leftHand.transform.position = Vector3.Lerp(leftHand.transform.position, new Vector3(leftHand.transform.position.x, topHandY, leftHand.transform.position.z), Time.deltaTime * 4);
    }

    void SlamLeftHand()
    {
        leftHand.transform.position = Vector3.Lerp(leftHand.transform.position, new Vector3(leftHand.transform.position.x, bottomHandY, leftHand.transform.position.z), Time.deltaTime * 15);
    }

    void LiftRightHand()
    {
        rightHand.transform.position = Vector3.Lerp(rightHand.transform.position, new Vector3(rightHand.transform.position.x, topHandY, rightHand.transform.position.z), Time.deltaTime * 4);
    }

    void SlamRightHand()
    {
        rightHand.transform.position = Vector3.Lerp(rightHand.transform.position, new Vector3(rightHand.transform.position.x, bottomHandY, rightHand.transform.position.z), Time.deltaTime * 15);
    }














    void FollowPlayer()
    {
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, new Vector3(GameObject.FindWithTag("Player").transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Time.deltaTime);
    }




    IEnumerator BossStart()
    {
        onCooldown = true;
        yield return new WaitForSeconds(3f);
        onCooldown = false;
        DecideAttack();
    }

    




    public BulletScript HandleBulletSpawn(BulletObject bullet, string bulletName)
    {
        BulletScript bulletScript = null;
        bulletScript = bulletManager.CreateBulletFromBank(bullet, bulletName);
        if (enemyBulletSoundPlaying == false)
        {
            AudioManager.instance.PlayOneShot(bulletSound, this.gameObject.transform.position);
            enemyBulletSoundPlaying = true;
            StartCoroutine(StopSoundSpam());
        }
        return bulletScript;
    }

    IEnumerator StopSoundSpam()
    {
        yield return new WaitForSeconds(0.06f);
        enemyBulletSoundPlaying = false;
    }
}
