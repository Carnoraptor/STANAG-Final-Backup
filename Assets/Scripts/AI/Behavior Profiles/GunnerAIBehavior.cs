using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using EnemyGeneric;

public class GunnerAIBehavior : MonoBehaviour
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
    public int bulletsAtOnce;
    public float bulletSpeed;
    public float inaccuracy;
    public GameObject projectilePrefab;

    [Header("Prefab Generals")]
    public GameObject projOrigin;
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
        bulletsAtOnce = ai._bulletsAtOnce;
        bulletSpeed = ai._bulletSpeed;
        inaccuracy = ai._inaccuracy;
        projectilePrefab = ai._projectilePrefab;

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Projectile Origin"))
            {
                projOrigin = child.gameObject;
            }
        }

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

        if (dist < range && !attackCooldown)
        {
            Shoot();
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
        projOrigin.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }

    void Shoot()
    {
        PointAtPlayer();
        for (int b = 0; b<bulletsAtOnce; b++)
        {
            //RandomAngle();
            GameObject projectile = Instantiate(projectilePrefab, projOrigin.transform.position, projOrigin.transform.rotation);
            Rigidbody2D projRB = projectile.GetComponent<Rigidbody2D>();
            //Shoots the bullet
            projRB.AddForce(projOrigin.transform.right * bulletSpeed, ForceMode2D.Impulse);
        }
        attackCooldown = true;
        StartCoroutine(BeginCooldown());
    }

    public void RandomAngle()
    {
        Vector3 vector;

        vector.x = transform.localRotation.x;
        vector.y = transform.localRotation.y;
        vector.z = transform.localRotation.z;
        float minRot = transform.rotation.z + (inaccuracy * -1f);
        float maxRot = transform.rotation.z + inaccuracy;
        float randomNum = Random.Range(minRot, maxRot);
        
        if (genAI.direction > 0f)
        {
            vector.z = transform.rotation.z + randomNum;
        }
        else
        {
            vector.z = transform.rotation.z - randomNum;
        }
        
        Quaternion quaternion = Quaternion.Euler(vector);

        projOrigin.transform.rotation = quaternion;
    }

    IEnumerator BeginCooldown()
    {
        yield return new WaitForSeconds(ai._enemyAttackRate);
        attackCooldown = false;
    }
}
