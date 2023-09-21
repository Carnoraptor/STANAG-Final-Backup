using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using EnemyGeneric;
using Pixelnest.BulletML;

public class BonePriestBehavior : MonoBehaviour
{
    [Header("Generics")]
    public Vector2 target;
    
    public float speed = 0;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb2d;

    [Header("Specifics")]
    public GunnerAI ai;

    public float stopDist;
    public float range;

    bool readyToAttack = false;
    bool onCooldown = false;

    [Header("Prefab Generals")]
    GeneralAI genAI;

    //Backends
    bool doMovement = true;

    bool attackCooldown = false;

    BulletSourceScript bulletSource;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        genAI = GetComponent<GeneralAI>();

        player = GameObject.FindWithTag("Player");

        if (ai != null)
        {
            genAI.ai = ai;
        }
        else
        {
            Debug.LogError("Please assign an AI ScriptableObject to " + this.gameObject.name + "'s behavior script!");
        }

        //Setting based on scriptable object
        speed = ai._enemySpeed;
        stopDist = ai._stopDist;
        range = ai._range;

        bulletSource = this.gameObject.transform.GetChild(0).GetComponent<BulletSourceScript>();


        seeker.StartPath(rb2d.position, target, OnPathComplete);

        InvokeRepeating("UpdatePath", 0f, 2f); //formerly UpdatePath
        InvokeRepeating("CheckDistanceOfPlayer", 1f, 0.2f);
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
        Vector3 force = direction * speed;

        this.gameObject.transform.position += force * Time.deltaTime;

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




    //Specific Behavior
    void CheckDistanceOfPlayer()
    {
        float dist = Vector3.Distance(this.gameObject.transform.position, target);

        if (dist > stopDist)
        {
            doMovement = true;
        }
        else
        {
            doMovement = false;
            //this.gameObject.GetComponent<GeneralAI>().StopMoving(1);
        }

        if (dist < range && readyToAttack)
        {
            DecideAttack();
        }
        else if (dist < range && onCooldown == false)
        {
            if (Random.Range(0, 1) == 0)
            {
                readyToAttack = true;   
            }
        }
    }

    IEnumerator BeginCooldown()
    {
        yield return new WaitForSeconds(5f);
        onCooldown = false;
    }



    void DecideAttack()
    {
        int decideAttack = Random.Range(0, 8);
        if (decideAttack < 4)
        {
            SpikeStream();
        }
        if (decideAttack > 3 && decideAttack < 7)
        {
            SpikeBarrage();
        }
        if (decideAttack > 6)
        {
            OrbCircle();
        }
        onCooldown = true;
        readyToAttack = false;
        StartCoroutine(BeginCooldown());
    }

    [SerializeField] TextAsset spikeStreamFile;
    void SpikeStream()
    {
        bulletSource.xmlFile = spikeStreamFile;
    }

    [SerializeField] TextAsset spikeFile;
    void SpikeBarrage()
    {
        bulletSource.xmlFile = spikeFile;
    }

    [SerializeField] GameObject creatureCircle;
    void OrbCircle()
    {
        Instantiate(creatureCircle, this.gameObject.transform.position, Quaternion.identity);
    }
}