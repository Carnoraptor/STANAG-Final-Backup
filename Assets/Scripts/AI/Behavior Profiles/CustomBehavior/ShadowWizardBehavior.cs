using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using EnemyGeneric;

public class ShadowWizardBehavior : MonoBehaviour
{
    [Header("Generics")]
    public Transform target;
    
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


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        genAI = GetComponent<GeneralAI>();

        if (ai != null)
        {
            genAI.ai = ai;
        }
        else
        {
            Debug.LogError("Please assign an AI ScriptableObject to " + this.gameObject.name + "'s behavior script!");
        }

        target = GameObject.FindWithTag("Player").gameObject.transform;

        //Setting based on scriptable object
        speed = ai._enemySpeed;
        stopDist = ai._stopDist;
        range = ai._range;


        seeker.StartPath(rb2d.position, target.position, OnPathComplete);

        InvokeRepeating("UpdatePath", 0f, .5f); //formerly UpdatePath
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
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
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
        float dist = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

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

    void PointAtPlayer()
    {
        float offset = 0; //can be messed with

        Vector2 targetPos = target.position;
        Vector2 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }

    IEnumerator BeginCooldown()
    {
        yield return new WaitForSeconds(2f);
        onCooldown = false;
    }



    void DecideAttack()
    {
        int decideAttack = Random.Range(0, 8);
        if (decideAttack < 4)
        {
            SummonSpirit();
        }
        if (decideAttack > 3 && decideAttack < 7)
        {
            EnergyBarrage();
        }
        if (decideAttack > 6)
        {
            OrbCircle();
        }
        onCooldown = true;
        readyToAttack = false;
        StartCoroutine(BeginCooldown());
    }

    [SerializeField] GameObject spirit;
    void SummonSpirit()
    {
        Instantiate(spirit, transform.position, Quaternion.identity);
    }

    [SerializeField] GameObject energyProjectile;
    void EnergyBarrage()
    {
        PointAtPlayer();
        Instantiate(energyProjectile, transform.position, transform.rotation);
    }

    [SerializeField] GameObject orbCircle;
    void OrbCircle()
    {
        Instantiate(orbCircle, target.position, Quaternion.identity);
    }
}