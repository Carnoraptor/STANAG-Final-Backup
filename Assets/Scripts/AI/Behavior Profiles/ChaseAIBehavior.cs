using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using EnemyGeneric;

public class ChaseAIBehavior : MonoBehaviour
{
    public Transform target;
    
    public float speed;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;

    [Header("Specifics")]
    public ChaseAI ai;
    GeneralAI genAI;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        genAI = GetComponent<GeneralAI>();

        //Setting based on scriptable object
        speed = ai._enemySpeed;

        if (ai != null)
        {
            genAI.ai = ai;
        }
        else
        {
            Debug.LogError("Please assign an AI ScriptableObject to " + this.gameObject.name + "'s behavior script!");
        }

        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        seeker.StartPath(rb2d.position, target.position, OnPathComplete);

        InvokeRepeating("UpdatePath", 0f, .05f);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
}
