using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGeneric;

public class GeneralAI : MonoBehaviour
{
    public EnemyAI ai;

    [Header("Main")]
    public string enemyName;
    public EnemyAI.EnemyType enemyType;
    public int enemyID;
    //public var enemyBehaviour;

    [Header("Stats")]
    public int enemyHP;
    public int enemyArmor;
    public int enemyDamage;
    public int enemyArmorPierce;
    public float enemySpeed;
    public float enemyAttackRate;
    public int enemyCurrentHealth;

    [Header("Behavior")]
    public bool doesContactDamage;
    public int contactDamage;
    public int contactAP;

    [Header("Graphics and Prefabs")]
    public Sprite enemySprite;

    //Universal Backends (irrelevant to ScriptableObject)
    [Header("Backends")]
    [HideInInspector] public bool isDead;
    float scaleX;
    [HideInInspector] public int direction;
    [HideInInspector] public bool doMovement = true;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public GameObject playerObj;
    [HideInInspector] public Player playerScript;

    [HideInInspector] Vector2 targetPos;
    [HideInInspector] Vector2 thisPos;
    [HideInInspector] public float angleToPlayer;

    public Vector2 startingPosition;
    //public GameObject dummy;
    public EnemyUtils enemyUtils = new EnemyUtils();
    

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag="Enemy"; 
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); //remember to have image as a child of the enemy object
        rb2d = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();

        //Main
        enemyName = ai._enemyName;
        enemyType = ai._enemyType;
        enemyID = ai._enemyID;
        //Stats
        enemyHP = ai._enemyHP;
        enemyArmor = ai._enemyArmor;
        enemyDamage = ai._enemyDamage;
        enemyArmorPierce = ai._enemyArmorPierce;
        enemySpeed = ai._enemySpeed;
        enemyAttackRate = ai._enemyAttackRate;
        enemyCurrentHealth = enemyHP;
        //Behavior
        doesContactDamage = ai._doesContactDamage;
        contactDamage = ai._contactDamage;
        contactAP = ai._contactAP;
        //Graphics & Prefabs
        enemySprite = ai._enemySprite;
        enemyUtils.SetSpriteToReal(this);
        startingPosition = transform.position;
    }

    void FixedUpdate()
    {
        Flip();
    }

    void FindPlayer()
    {
        targetPos = playerObj.transform.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.x;
        angleToPlayer = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToPlayer));
    }

    void Flip()
    {
        scaleX = transform.localScale.x;

        if (transform.rotation.z <= 90f || transform.rotation.z >= -90f)
        {
            direction = 1;
        }
        else 
        {
            direction = -1;
        }

        if (direction == 1)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            //Debug.Log("Flipped right... or tried to");
        }
        else if (direction == -1)
        {
            transform.localScale = new Vector3((-1) * scaleX, transform.localScale.y, transform.localScale.z);
            //Debug.Log("Flipped left... or tried to");
        }
    }





    //Behavior

    //Health
    public virtual void TakeDamage(int damage, int armorPierce)
        {
            int armorLeft = enemyArmor - armorPierce;
            if (armorLeft < 0)
            {
                armorLeft = 0;
            }
            //Debug.Log(enemyName + " has " + armorLeft + " armor unpierced");
            int damagePassed = damage - armorLeft;
            if (damagePassed < 0)
            {
                damagePassed = 0;
            }
            //Debug.Log(enemyName + " takes " + damagePassed + " damage"); 
            enemyCurrentHealth -= damagePassed;
            Debug.Log(this.gameObject.name + " takes " + damagePassed + "!");
            if (enemyCurrentHealth <= 0)
            {
                enemyCurrentHealth = 0;
                Die();
            }
        }

        public void Die()
        {
            GameObject.FindWithTag("Gun").GetComponent<GunHandler>().OnEnemyKill(this.gameObject);

            isDead = true;
            Destroy(this.gameObject.GetComponent<Collider2D>());
            InvokeRepeating("DeathFade", 0f, 0.02f);
            //genAI.StartCoroutine(ItShallPerish());
            StartDestroyTimer();
        }

    
    //Contact Damage
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && doesContactDamage == true)
        {
            enemyUtils.DealDamage(contactDamage, contactAP);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && doesContactDamage == true)
        {
            enemyUtils.DealDamage(contactDamage, contactAP);
        }
    }

    public void StopMoving(float scale)
    {
        Vector2 opposite = new Vector2((rb2d.velocity.x * scale) * -1, (rb2d.velocity.y *  scale) * -1);
        rb2d.AddForce(opposite * Time.deltaTime);
    }




    //sadly this has to be in genAI since it cant invokerepeating and have parameters :(
    void DeathFade()
    {
        Color tmp = spriteRenderer.color;
        tmp.a -= Time.deltaTime;
        spriteRenderer.color = tmp;
    }

    //sadly this has to be in genAI since namespaces dont use monobehaviour
    public void StartDestroyTimer()
    {
        StartCoroutine(DestroyTimer());
        if (GameState.testing == false)
        {
            GameState.currentRoom.EnemyWithinKilled();
        }
    }

    public IEnumerator DestroyTimer()
    {
        Debug.Log("kil");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

