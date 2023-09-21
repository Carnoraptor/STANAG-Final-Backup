using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPursuitMovement
{
    
    /////////////   BOILERPLATE FOR AI THAT USE PURSUIT MOVEMENT   /////////////

    //Grabs parent class

    //public var parent;



    public Transform target;
    
    public float speed = 0f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;

    void Start()
    {
        /*seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();*/

        seeker.StartPath(rb2d.position, target.position, OnPathComplete);

        //InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
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


//TODO: Interfaces  &  Inheritance