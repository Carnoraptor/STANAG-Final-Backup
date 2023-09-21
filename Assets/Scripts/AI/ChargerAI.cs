using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAI : MonoBehaviour
{
    [Header("Main")]
    public GeneralAI genAI;
    public GameObject player;

    [Header("Dash Variables")]
    public float maxDashRange;
    public float minDashRange;
    public float dashCooldown;
    public float dashForce;
    public float dashHesitate;
    public float dashRecovery;
    //Backend
    float untilNextDash;
    bool canDash = true;
    bool isDashing = false;

    Vector2 originalPos;
    bool freezePos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        genAI = GetComponent<GeneralAI>();
    }

    // Update is called once per frame
    void Update()
    {
        genAI.rb2d.AddForce(transform.forward * dashForce, ForceMode2D.Impulse);

        //transform.right = target.position - transform.position;
        if (untilNextDash <= dashCooldown)
        {
            canDash = false;
            untilNextDash += Time.deltaTime;
        }
        else
        {
            canDash = true;
        }

        Mathf.Clamp(untilNextDash, 0f, dashCooldown);

        if (Vector2.Distance(transform.position, player.transform.position) < maxDashRange && Vector2.Distance(transform.position, player.transform.position) > minDashRange && canDash == true && genAI.isDead == false)
        {
            if (Random.Range(1, 10) == 1)
            {
                StartCoroutine(Dash());
            }
        }
        
    }

    IEnumerator Dash()
    {
        Debug.Log("dash time");
        isDashing = true;
        canDash = false;
        untilNextDash = 0f;
        FreezePos();
        yield return new WaitForSeconds(dashHesitate);
        genAI.rb2d.AddForce(transform.forward * dashForce, ForceMode2D.Impulse);
        isDashing = false;
        yield return new WaitForSeconds(dashRecovery);
        genAI.doMovement = true;
        freezePos = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && genAI.isDead == false)
        {
            //PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            //playerMovement.TakeDamage(genAI.damage); //the current amount of damage Dashers do
        }
    }

    public void FreezePos()
    {
        originalPos = new Vector2(transform.position.x, transform.position.y);
        genAI.doMovement = false;
    }
}

//Charger currently has a shit ton of issues, work on fixing these up tomorrow