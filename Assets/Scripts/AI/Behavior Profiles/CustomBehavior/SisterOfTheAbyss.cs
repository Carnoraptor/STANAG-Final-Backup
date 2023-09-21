using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using EnemyGeneric;
using BulletMLLib;
using Pixelnest.BulletML;
using FMODUnity;

public class SisterOfTheAbyss : MonoBehaviour
{
    [Header("Pathing bullshit mostly")]
    public Vector2 target;
    public GameObject player;
    
    public float speed = 0;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb2d;

    [Header("Specifics")]
    public BossAI ai;

    bool readyToAttack = false;
    bool onCooldown = false;
    BossGeneralAI bossAI;

    [SerializeField] Animator animator;

    [Header("Actual Gameplay Variables")]

    public bool enraged = false;

    [SerializeField] GameObject hands;

    BulletSourceScript handsSource;

    public TextAsset attack1;
    public TextAsset attack2;
    public TextAsset attack3;
    public TextAsset attack4;
    public TextAsset attack5;

    public int currentAttack = 0; // 0 means not attacking

    public EventReference bulletSound;

    public bool enemyBulletSoundPlaying = false;

    private BulletManagerScript bulletManager;

    //Backends
    bool doMovement = true;

    bool attackCooldown = false;


        //START AND UPDATE

    void Awake()
    {
        bulletManager = FindObjectOfType<BulletManagerScript>();
        bulletManager.OnBulletSpawned += HandleBulletSpawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        bossAI = GetComponent<BossGeneralAI>();
        
        player = GameObject.FindWithTag("Player");

        if (ai != null)
        {
            bossAI.ai = ai;
        }
        else
        {
            Debug.LogError("Please assign an AI ScriptableObject to " + this.gameObject.name + "'s behavior script!");
        }

        

        //Setting based on scriptable object
        speed = ai._enemySpeed;

        target = new Vector2(player.transform.position.x + Random.Range(-10, 10), player.transform.position.y + Random.Range(-10, 10));
        seeker.StartPath(rb2d.position, target, OnPathComplete);

        InvokeRepeating("UpdatePath", 0f, .5f); //formerly UpdatePath


        //Source grabbing
        handsSource = hands.GetComponent<BulletSourceScript>();
        animator = this.gameObject.GetComponent<Animator>();


        //stop immidiate attack
        StartCoroutine(BossStart());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //CheckDistanceOfPlayer();
        if (doMovement)
        {
            Movement();
        }
    }

    void Update()
    {
        if (currentAttack == 0 && onCooldown == false)
        {
            DecideAttack();
        }
    }


            //MOVEMENT

    void Movement()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb2d.AddForce(force);

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            target = new Vector2(player.transform.position.x + Random.Range(-10, 10), player.transform.position.y + Random.Range(-10, 10));
            seeker.StartPath(rb2d.position, target, OnPathComplete);
        } 

        if (Random.Range(0, 35) == 0)
        {
            doMovement = false;
            StartCoroutine(WaitToMove());
        }
    }

    IEnumerator WaitToMove()
    {
        rb2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        doMovement = true; // will need to fix for freeze effect in future
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }



        //BEHAVIOR



    IEnumerator BeginCooldown()
    {
        float timeToWait = 8f;
        switch(currentAttack)
        {
            case 1:
            //Bomb Helix
            timeToWait = 10f;
            break;
            case 2:
            timeToWait = 12f;
            break;
            //Whirlwind Bombs
            case 3:
            timeToWait = 12f;
            break;
            case 4:
            //Whirlwind Bullets
            timeToWait = 15f;
            break;
            case 5:
            break;
        }
        yield return new WaitForSeconds(timeToWait);
        doMovement = true;
        currentAttack = 0;
        animator.SetInteger("CurrentAttack", currentAttack);
        yield return new WaitForSeconds(3f);
        onCooldown = false;
    }

        //ATTACKING
    int mostRecentAttack = 0;

    void DecideAttack()
    {
        doMovement = false;
        rb2d.velocity = Vector2.zero;
        int decideAttack = Random.Range(1, 1); // should be 1-5 but changed for testing purposes
        if (decideAttack == mostRecentAttack)
        {
            decideAttack = Random.Range(1, 1);
        }
        currentAttack = decideAttack;
        mostRecentAttack = decideAttack;
        animator.SetInteger("CurrentAttack", currentAttack);
        switch(decideAttack)
        {
            case 1:
                //Case 1 is the Void Sigil
                handsSource.xmlFile = attack1;
                handsSource.ParsePattern(false);
                handsSource.Initialize();
                break;
            case 2:
                handsSource.xmlFile = attack2;
                handsSource.ParsePattern(false);
                handsSource.Initialize();
                break;
            case 3:
                handsSource.xmlFile = attack3;
                handsSource.ParsePattern(false);
                handsSource.Initialize();
                break;
            case 4:
                handsSource.xmlFile = attack4;
                handsSource.ParsePattern(false);
                handsSource.Initialize();
                break;
            case 5:
                handsSource.xmlFile = attack1;
                handsSource.ParsePattern(false);
                handsSource.Initialize();
                handsSource.xmlFile = attack3;
                handsSource.ParsePattern(false);
                handsSource.Initialize();
                break;
        }
        onCooldown = true;
        readyToAttack = false;
        StartCoroutine(BeginCooldown());
    }


    IEnumerator BossStart()
    {
        onCooldown = true;
        yield return new WaitForSeconds(3f);
        onCooldown = false;
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